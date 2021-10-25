using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BocciaPlayer
{
    public class PlayerStopState : IPlayerState
    {
        override public void Init(PlayerController controller)
        {
            m_player = controller;
        }
        override public void Enter()
        {
        }
        override public void Leave()
        {
        }
        override public void Execute()
        {
        }

        //�L���ɂȂ��Ă��邩�H
        override public bool IsEnable()
        {
            return false;
        }
        //�������~�߂Ă��邩�H
        override public bool IsStop()
        {
            return true;
        }

    }

}

