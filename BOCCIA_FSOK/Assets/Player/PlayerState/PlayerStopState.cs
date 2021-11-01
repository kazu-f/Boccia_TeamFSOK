using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BocciaPlayer
{
    //何も処理をしないステート。
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

        //有効になっているか？
        override public bool IsEnable()
        {
            return false;
        }
        //処理を止めているか？
        override public bool IsStop()
        {
            return true;
        }

    }

}

