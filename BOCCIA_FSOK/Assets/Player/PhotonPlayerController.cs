using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BocciaPlayer
{
    /// <summary>
    /// プレイヤーのデータステート。
    /// </summary>
    public enum EnPlayerDataState 
    { 
        enPlayerData_None = 0,
        enPlayerData_Gauge = 1 << 0,
        enPlayerData_Throw = 1 << 1,
    }


    public class PhotonPlayerController : IPlayerController
    {
        private Vector3 startPosition = new Vector3();           //開始時点の座標。
        private Quaternion startRotation = new Quaternion();     //開始時点の回転。
        private Vector3 oldPosition = new Vector3();           //前フレームの座標。
        private Quaternion oldRotation = new Quaternion();           //前フレームの座標。
        private bool isUpdating = false;

        private void Awake()
        {
            startPosition = this.gameObject.transform.position;
            startRotation = this.gameObject.transform.rotation;
            oldPosition = startPosition;
            InitPlayerScript();
            //ボール投げるスクリプトを切る。
            throwBallControler.enabled = false;
            playerMoveScript.enabled = false;
        }
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (!isUpdating) return;
            //座標の値が変化した。
            if(oldPosition != netSendManager.ReceivePlayerPos())
            {
                playerMoveScript.enabled = true;
                this.gameObject.transform.position = netSendManager.ReceivePlayerPos();
                oldPosition = netSendManager.ReceivePlayerPos();
            }
            else
            {
                playerMoveScript.enabled = false;
            }
            //回転の値が変化した。
            if(oldRotation != netSendManager.ReveiveQuaternion())
            {
                oldRotation = netSendManager.ReveiveQuaternion();
                throwAngleController.SetAngle(oldRotation.eulerAngles.y);
            }
            //画面を触っている。
            if(netSendManager.ReceiveState() == (int)EnPlayerDataState.enPlayerData_Gauge)
            {
                throwBallControler.enabled = true;
                throwBallControler.StartThrowBall(netSendManager.ReceiveThrowGaugePos());
                throwBallControler.SetThrowPow(netSendManager.ReceiveThrowPower());
            }
            //ボールを投げた。
            else if(netSendManager.ReceiveState() == (int)EnPlayerDataState.enPlayerData_Throw)
            {
                throwBallControler.SetThrowPosition(netSendManager.ReceiveThrowPos());
                throwBallControler.ThrowBall();         //ボールを投げる。
            }
            else
            {
                throwBallControler.enabled = false;
            }

        }

        override public void SwitchPlayer(bool isEnable)
        {
            isUpdating = isEnable;
            //開始時点のトランスフォームへ移動。
            if (isEnable == true)
            {
                //プレイヤーが切り替わる時にカメラの位置を合わせる。
                throwAngleController.ChangeCamPos();
            }
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
