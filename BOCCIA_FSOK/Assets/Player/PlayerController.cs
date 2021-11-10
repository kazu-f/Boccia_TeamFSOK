using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BocciaPlayer
{
    public enum EnPlayerState { 
        enIdle,     //タッチ入力待ち。
        enMove,     //移動。
        enAngle,    //角度を決める処理。
        enThrow,    //投げる処理。
        enWait,     //ボールが止まるまで待機。
        enStop,     //処理停止。
        enStateNum  //ステートの数。
    }

    public class PlayerController : IPlayerController
    {
        private IPlayerState[] playerStateList;                 //ステートのリスト。

        private IPlayerState currentState = null;
        private EnPlayerState enCurrentState = EnPlayerState.enStateNum;
        private Vector3 startPosition = new Vector3();           //開始時点の座標。
        private Quaternion startRotation = new Quaternion();     //開始時点の回転。
        private Vector3 oldPosition = new Vector3();           //前フレームの座標。

        private void Awake()
        {
            startPosition = this.gameObject.transform.position;
            startRotation = this.gameObject.transform.rotation;
            oldPosition = startPosition;
            InitPlayerScript();
            //ステート初期化。
            playerStateList = new IPlayerState[(int)EnPlayerState.enStateNum];

            playerStateList[(int)EnPlayerState.enIdle] = new PlayerIdleState();
            playerStateList[(int)EnPlayerState.enMove] = new PlayerMoveState();
            playerStateList[(int)EnPlayerState.enAngle] = new PlayerThrowAngleState();
            playerStateList[(int)EnPlayerState.enThrow] = new PlayerThrowBallState();
            playerStateList[(int)EnPlayerState.enWait] = new PlayerWaitBallState();
            playerStateList[(int)EnPlayerState.enStop] = new PlayerStopState();
            playerStateList[(int)EnPlayerState.enIdle].Init(this);
            playerStateList[(int)EnPlayerState.enMove].Init(this);
            playerStateList[(int)EnPlayerState.enAngle].Init(this);
            playerStateList[(int)EnPlayerState.enThrow].Init(this);
            playerStateList[(int)EnPlayerState.enWait].Init(this);
            playerStateList[(int)EnPlayerState.enStop].Init(this);

            currentState = playerStateList[(int)EnPlayerState.enStop];
        }
        // Start is called before the first frame update
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
            //ステートを実行。
            currentState.Execute();
            //座標変化があれば送信。
            if(oldPosition != this.transform.position)
            {
                if (netSendManager != null)
                {
                    netSendManager.SendPlayerPos(this.transform.position);
                }
                oldPosition = this.transform.position;
            }
        }

        override public void SwitchPlayer(bool isEnable)
        {
            //開始時点のトランスフォームへ移動。
            this.gameObject.transform.position = startPosition;
            this.gameObject.transform.rotation = startRotation;
            oldPosition = startPosition;
            if (isEnable == true)
            {
                //プレイヤーが切り替わる時にカメラの位置を合わせる。
                throwAngleController.ChangeCamPos();
                if (netSendManager != null)
                {
                    netSendManager.SendPlayerPos(this.transform.position);
                }
                ChangeState(EnPlayerState.enIdle);
            }
            else
            {
                ChangeState(EnPlayerState.enStop);
            }
        }

        /// <summary>
        /// ステートを変更。
        /// </summary>
        /// <param name="enState">ステート変数。</param>
        public void ChangeState(EnPlayerState enState)
        {
            //ステートが変わっていない。
            if (enCurrentState == enState)
            {
                return;
            }
            
            if (currentState != null)
            {
                //終了処理。
                currentState.Leave();
            }
            enCurrentState = enState;
            //ステート変更。
            currentState = playerStateList[(int)enState];
            //開始処理。
            currentState.Enter();
        }

        /// <summary>
        /// プレイヤーをリセット。
        /// </summary>
        override public void ResetPlayer()
        {
            ballHolderController.ResetBall();
        }
    }
}
