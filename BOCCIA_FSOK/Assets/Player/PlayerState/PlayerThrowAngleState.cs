using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BocciaPlayer
{
    public class PlayerThrowAngleState : IPlayerState
    {
        private TouchManager touchManager;
        private ThrowAngleController angleController;
        private float xSpeed = 0.0f;

        override public void Init(PlayerController controller)
        {
            m_player = controller;
            touchManager = TouchManager.GetInstance();
            if (m_player)
            {
                angleController = m_player.GetThrowAngleController();
            }
            angleController.enabled = false;
        }
        override public void Enter()
        {
            SwichActiveGameObjects.GetInstance().SwitchGameObject(false);
            angleController.enabled = true;

        }
        override public void Leave()
        {
            angleController.enabled = false;

        }
        override public void Execute()
        {
            if (touchManager.IsTouch())
            {
                if (touchManager.GetPhase() == TouchInfo.Moved)
                {
                    xSpeed = touchManager.GetDeltaPosInScreen().x;
                    angleController.AngleRotation(xSpeed);
                }
                else if(touchManager.GetPhase() == TouchInfo.Ended)
                {
                    //�X�e�[�g�ύX�B
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
