using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BocciaPlayer
{
    public class PlayerMoveState : IPlayerState
    {
        private const float MOVE_SPEED = 0.1f;
        private PlayerMoveButton moveButton = null;
        private Transform playerTransform = null;
        private Vector3 moveSpeed = Vector3.zero;       //�ړ����x�B
        override public void Init(PlayerController controller)
        {
            m_player = controller;
            if(m_player)
            {
                playerTransform = m_player.transform;
            }
        }
        override public void Enter()
        {
        }
        override public void Leave()
        {
        }
        override public void Execute()
        {
            if(moveButton.IsPressAnyButton())
            {

            }
            else
            {
                //�ҋ@�X�e�[�g�ɕύX�B
                m_player.ChangeState(EnPlayerState.enIdle);
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
