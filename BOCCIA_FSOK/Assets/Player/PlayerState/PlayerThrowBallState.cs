using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BocciaPlayer
{
    public class PlayerThrowBallState : IPlayerState
    {
        private TouchManager touchManager = null;
        private ThrowBallControler throwBall = null;
        private NetworkSendManagerScript netSendManager = null;
        //private�B
        private Vector2 m_touchStartPos = new Vector2(0.0f, 0.0f);    //�G��n�߂����W�B
        private Vector2 m_touchEndPos = new Vector2(0.0f, 0.0f);      //�����؂������W�B
        private Vector2 m_endToStart = new Vector2(0.0f, 0.0f);       //�J�n���W��������؂������W�܂ł̃x�N�g���B
        private Vector2 m_touchPosInScreen = new Vector2(0.0f, 0.0f); //���݂̃^�b�`���Ă�����W(�X�N���[�����W�n�H)
        private Vector2 m_throwPow = new Vector2(0.0f, 0.0f);         //�������
        private ThrowGaugeScript throwGauge = null;

        override public void Init(PlayerController controller)
        {
            m_player = controller;
            touchManager = TouchManager.GetInstance();
            throwGauge = ThrowGaugeScript.GetInstance();
            if(m_player)
            {
                throwBall = m_player.GetThrowBallController();
                throwBall.enabled = false;
                netSendManager = m_player.GetNetSendManager();
            }
        }
        override public void Enter()
        {
            SwichActiveGameObjects.GetInstance().SwitchGameObject(false);
            m_touchStartPos = touchManager.GetTouchPosInScreen();
            //�^�b�`�ʒu���������B
            m_touchStartPos = touchManager.GetTouchPosInScreen();
            m_touchEndPos = touchManager.GetTouchPosInScreen();
            m_throwPow.x = 0.0f;
            m_throwPow.y = 0.0f;
            throwBall.StartThrowBall(m_touchStartPos);
            throwBall.enabled = true;
            if (netSendManager != null)
            {
                netSendManager.SendThrowGaugePosition(m_touchStartPos);
                netSendManager.SendState((int)EnPlayerDataState.enPlayerData_Gauge);
            }
        }
        override public void Leave()
        {
            throwBall.enabled = false;
            if (netSendManager != null)
            {
                netSendManager.SendState((int)EnPlayerDataState.enPlayerData_None);
            }
        }
        override public void Execute()
        {
            if (touchManager.IsTouch())
            {
                //�O�t���[�����瓮��������B
                if (touchManager.GetPhase() != TouchInfo.Stationary)
                {
                    m_touchPosInScreen = touchManager.GetTouchPosInScreen();
                }

                if (touchManager.GetPhase() == TouchInfo.Moved)
                {
                    //�ړ�������̍��W�B
                    m_touchEndPos = m_touchPosInScreen;
                    m_endToStart = m_touchStartPos - m_touchEndPos;
                    m_throwPow.x = m_endToStart.x / throwGauge.GetGaugeSize().x / 1.2f;
                    m_throwPow.x = Mathf.Min(1.0f, Mathf.Max(m_throwPow.x, -1.0f));
                    m_throwPow.y = m_endToStart.y / throwGauge.GetGaugeSize().y;
                    m_throwPow.y = Mathf.Min(1.0f, Mathf.Max(m_throwPow.y, 0.0f));

                    //������͂��Z�b�g�B
                    throwBall.SetThrowPow(m_throwPow);
                    if(netSendManager != null)
                    {
                        netSendManager.SendThrowPow(m_throwPow);
                        netSendManager.SendState((int)EnPlayerDataState.enPlayerData_Gauge);
                    }
                }
                else if (touchManager.GetPhase() == TouchInfo.Ended)
                {
                    if (m_throwPow.y > 0.0f)
                    {
                        //�{�[���𓊂���B
                        throwBall.ThrowBall();
                        //if (netSendManager != null)
                        //{
                        //    netSendManager.SendThrowPos(throwBall.GetThrowPosition());
                        //    netSendManager.SendState((int)EnPlayerDataState.enPlayerData_Throw);
                        //}
                        //�X�e�[�g�ύX�B
                        m_player.ChangeState(EnPlayerState.enWait);
                    }
                    else
                    {
                        //if (netSendManager != null)
                        //{
                        //    netSendManager.SendState((int)EnPlayerDataState.enPlayerData_None);
                        //}
                        //�X�e�[�g�ύX�B
                        m_player.ChangeState(EnPlayerState.enIdle);
                    }
                }
            }

        }

        //�L���ɂȂ��Ă��邩�H
        override public bool IsEnable()
        {
            return true;
        }
        //�������~�߂Ă��邩�H
        override public bool IsStop()
        {
            return false;
        }
    }

}
