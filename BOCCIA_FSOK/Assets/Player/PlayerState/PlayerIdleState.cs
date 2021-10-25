using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BocciaPlayer {
    public class PlayerIdleState : IPlayerState
    {
        private TouchManager touchManager;
        private Vector3 m_touchStartPos;

        override public void Init(PlayerController controller)
        {
            m_player = controller;
            touchManager = TouchManager.GetInstance();
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
            if (touchManager.GetPhase() == TouchInfo.Began)
            {
                m_touchStartPos = touchManager.GetTouchPosInScreen();
                //ステートを切り替える。
                if (m_touchStartPos.y > 0.2f)
                {
                    m_player.ChangeState(EnPlayerState.enThrow);
                }
                else if (m_touchStartPos.y <= 0.2f)
                {
                    m_player.ChangeState(EnPlayerState.enAngle);
                }
            }

        }

        //有効になっているか？
        override public bool IsEnable()
        {
            return true;
        }
        //処理を止めているか？
        override public bool IsStop()
        {
            return false;
        }

    }

}
