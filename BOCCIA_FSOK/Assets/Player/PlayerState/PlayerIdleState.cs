using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BocciaPlayer {

    //���͑ҋ@���B
    public class PlayerIdleState : IPlayerState
    {
        private TouchManager touchManager;
        private PlayerMoveButton moveButton;
        private Vector3 m_touchStartPos;

        override public void Init(PlayerController controller)
        {
            m_player = controller;
            touchManager = TouchManager.GetInstance();
            moveButton = PlayerMoveButton.GetInstance();
        }
        override public void Enter()
        {
            SwichActiveGameObjects.GetInstance().SwitchGameObject(true);

        }
        override public void Leave()
        {
        }
        override public void Execute()
        {
            if (touchManager.IsTouch() &&
                touchManager.GetPhase() == TouchInfo.Began)
            {
                //��ʂ̃^�b�`�ʒu���擾�B
                m_touchStartPos = touchManager.GetTouchPosInScreen();
                //�X�e�[�g��؂�ւ���B
                if (m_touchStartPos.y > 0.2f)
                {
                    m_player.ChangeState(EnPlayerState.enThrow);
                }
                else if (m_touchStartPos.y <= 0.2f)
                {
                    m_player.ChangeState(EnPlayerState.enAngle);
                }
            }
            else if(moveButton.IsPressAnyButton())
            {
                //�ړ��{�^���������ꂽ��ړ��X�e�[�g�ɐ؂�ւ���B
                m_player.ChangeState(EnPlayerState.enMove);
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
