using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BocciaPlayer
{
    public class PlayerController : MonoBehaviour
    {
        private TouchManager touchManager;
        public ThrowBallControler throwBallControler;
        public ThrowAngleController throwAngleController;
        private Vector2 m_touchStartPos = new Vector2(0.0f,0.0f);     //触り始めた座標。

        public bool isThrowBallNone { get; set; } = false;   //ボールを投げ切ったかどうかのフラグ。trueで投げ切った。

        private void Awake()
        {
            touchManager = TouchManager.GetInstance();
            //タッチ情報を持つインスタンスを渡す。
            throwBallControler.SetTouchManager(touchManager);
            throwAngleController.SetTouchManager(touchManager);
            throwBallControler.ThrowBallDisable();
            throwAngleController.ThrowAngleDisable();
        }
        // Start is called before the first frame update
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
            if(touchManager.IsTouch())
            {
                if(touchManager.GetPhase() == TouchPhase.Began)
                {
                    m_touchStartPos = touchManager.GetTouchPosInScreen();
                    //有効化フラグを切り替える。
                    if (m_touchStartPos.y > 0.2f)
                    {
                        throwBallControler.ThrowBallEnable();
                    }
                    else if (m_touchStartPos.y <= 0.2f)
                    {
                        throwAngleController.ThrowAngleEnable();
                    }
                }
            }
            else
            {
                throwBallControler.ThrowBallDisable();
                throwAngleController.ThrowAngleDisable();
            }
        }

        public void SwitchPlayer(bool isEnable)
        {
            if(isEnable == true && this.enabled != isEnable)
            {
                //プレイヤーが切り替わる時にカメラの位置を合わせる。
                throwAngleController.ChangeCamPos();
            }
            this.enabled = isEnable;
        }

        /// <summary>
        /// ボールを投げたか？
        /// </summary>
        public bool IsThrowing()
        {
            return throwBallControler.IsThrowing();
        }
    }
}
