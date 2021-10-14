using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BocciaPlayer
{
    public class PlayerController : MonoBehaviour
    {
        private TouchManager touchManager;
        private ThrowBallControler throwBallControler;
        private ThrowAngleController throwAngleController;
        private BallHolderController ballHolderController;
        private TeamFlowScript TeamFlow;         //次投げるチームの判定。
        private Vector2 m_touchStartPos = new Vector2(0.0f,0.0f);     //触り始めた座標。

        public bool isThrowBallNone { get; set; } = false;   //ボールを投げ切ったかどうかのフラグ。trueで投げ切った。

        private void Awake()
        {
            throwBallControler = this.gameObject.GetComponentInChildren<ThrowBallControler>();
            throwAngleController = this.gameObject.GetComponentInChildren<ThrowAngleController>();
            ballHolderController = this.gameObject.GetComponentInChildren<BallHolderController>();
            touchManager = TouchManager.GetInstance();
            throwBallControler.enabled = false;
            throwAngleController.enabled = false;
            this.gameObject.SetActive(false);
            this.enabled = false;

            var gameFlow = GameObject.FindGameObjectWithTag("GameFlow");
            if(gameFlow)
            {
                TeamFlow = gameFlow.GetComponent<TeamFlowScript>();
            }
        }
        // Start is called before the first frame update
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
            if (throwBallControler.IsDecision() ||
                TeamFlow.GetIsMoving())
            {
                SwichActiveGameObjects.GetInstance().SwitchGameObject(false);
                return;
            }
            if(touchManager.GetPhase() == TouchInfo.Began)
            {
                SwichActiveGameObjects.GetInstance().SwitchGameObject(false);
                m_touchStartPos = touchManager.GetTouchPosInScreen();
                //有効化フラグを切り替える。
                if (m_touchStartPos.y > 0.2f)
                {
                    throwBallControler.enabled = true;
                }
                else if (m_touchStartPos.y <= 0.2f)
                {
                    throwAngleController.enabled = true;
                }
            }
            else if(touchManager.GetPhase() == TouchInfo.None)
            {
                throwBallControler.enabled = false;
                throwAngleController.enabled = false;
                SwichActiveGameObjects.GetInstance().SwitchGameObject(true);
            }
        }

        public void SwitchPlayer(bool isEnable)
        {
            if(isEnable == true)
            {
                //プレイヤーが切り替わる時にカメラの位置を合わせる。
                throwAngleController.ChangeCamPos();
                SwichActiveGameObjects.GetInstance().SwitchGameObject(true);
            }
            this.enabled = isEnable;
        }

        /// <summary>
        /// プレイヤーをリセット。
        /// </summary>
        public void ResetPlayer()
        {
            ballHolderController.ResetBall();
        }
    }
}
