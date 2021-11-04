using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BocciaPlayer
{
    public class PlayerThrowAngleState : IPlayerState
    {
        private TouchManager touchManager;
        private NetworkSendManagerScript netSendManager;
        private ThrowAngleController angleController;   //������ύX����X�N���v�g�B
        private float xSpeed = 0.0f;                    //��]�ʁB

        override public void Init(PlayerController controller)
        {
            m_player = controller;
            touchManager = TouchManager.GetInstance();
            if (m_player)
            {
                //������ύX����X�N���v�g���擾�B
                angleController = m_player.GetThrowAngleController();
                //������ύX����X�N���v�g�𖳌����B
                angleController.enabled = false;
                netSendManager = m_player.GetNetSendManager();
            }
        }
        override public void Enter()
        {
            //�������uGUI�𖳌����B
            SwichActiveGameObjects.GetInstance().SwitchGameObject(false);
            //������ύX����X�N���v�g��L�����B
            angleController.enabled = true;

        }
        override public void Leave()
        {
            //������ύX����X�N���v�g�𖳌����B
            angleController.enabled = false;

        }
        override public void Execute()
        {
            if (touchManager.IsTouch())
            {
                if (touchManager.GetPhase() == TouchInfo.Moved)
                {
                    //�^�b�`���ړ������瑬�x�����߂�B
                    xSpeed = touchManager.GetDeltaPosInScreen().x;
                    angleController.AngleRotation(xSpeed);
                    netSendManager.SendQuaternion(m_player.transform.rotation);
                }
                else if(touchManager.GetPhase() == TouchInfo.Ended)
                {
                    //�w�𗣂�����X�e�[�g�ύX�B
                    m_player.ChangeState(EnPlayerState.enIdle);
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
