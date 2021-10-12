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
        private Vector2 m_touchStartPos = new Vector2(0.0f,0.0f);     //�G��n�߂����W�B

        public bool isThrowBallNone { get; set; } = false;   //�{�[���𓊂��؂������ǂ����̃t���O�Btrue�œ����؂����B
        private bool isTouch = false;

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
        }
        // Start is called before the first frame update
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
            if (throwBallControler.IsDecision())
            {
                return;
            }
            if(touchManager.IsTouch())
            {
                if (touchManager.GetPhase() == TouchInfo.Began)
                {
                    SwichActiveGameObjects.GetInstance().SwitchGameObject(false);
                    m_touchStartPos = touchManager.GetTouchPosInScreen();
                    //�L�����t���O��؂�ւ���B
                    if (m_touchStartPos.y > 0.2f)
                    {
                        throwBallControler.enabled = true;
                    }
                    else if (m_touchStartPos.y <= 0.2f)
                    {
                        throwAngleController.enabled = true;
                    }
                    isTouch = true;
                }
            }
            else if(isTouch)
            {
                throwBallControler.enabled = false;
                throwAngleController.enabled = false;
                SwichActiveGameObjects.GetInstance().SwitchGameObject(true);
                isTouch = false;
            }
        }

        public void SwitchPlayer(bool isEnable)
        {
            if(isEnable == true)
            {
                //�v���C���[���؂�ւ�鎞�ɃJ�����̈ʒu�����킹��B
                throwAngleController.ChangeCamPos();
            }
            this.enabled = isEnable;
        }

        /// <summary>
        /// �v���C���[�����Z�b�g�B
        /// </summary>
        public void ResetPlayer()
        {
            ballHolderController.ResetBall();
        }
    }
}
