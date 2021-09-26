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
        private Vector2 m_touchStartPos = new Vector2(0.0f,0.0f);     //�G��n�߂����W�B

        public bool isThrowBallNone { get; set; } = false;   //�{�[���𓊂��؂������ǂ����̃t���O�Btrue�œ����؂����B

        private void Awake()
        {
            touchManager = TouchManager.GetInstance();
            //�^�b�`�������C���X�^���X��n���B
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

        public void SwitchPlayer(bool isEnable)
        {
            if(isEnable == true && this.enabled != isEnable)
            {
                //�v���C���[���؂�ւ�鎞�ɃJ�����̈ʒu�����킹��B
                throwAngleController.ChangeCamPos();
            }
            this.enabled = isEnable;
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
