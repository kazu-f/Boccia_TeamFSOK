using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BocciaPlayer
{
    public class PlayerController : MonoBehaviour
    {
        public TouchManager touchManager;
        public ThrowBallControler throwBallControler;
        public ThrowAngleController throwAngleController;
        private Vector2 m_touchStartPos = new Vector2(0.0f,0.0f);     //�G��n�߂����W�B

        // Start is called before the first frame update
        void Start()
        {
            throwBallControler.enabled = false;
            throwAngleController.enabled = false;
        }

        // Update is called once per frame
        void Update()
        {
            if(touchManager.IsTouch())
            {
                if(touchManager.GetPhase() == TouchPhase.Began)
                {
                    m_touchStartPos = touchManager.GetTouchPosInScreen();
                    //�L�����t���O��؂�ւ���B
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
        /// <summary>
        /// �{�[���𓊂������H
        /// </summary>
        public bool IsThrowing()
        {
            return throwBallControler.IsThrowing();
        }
    }
}
