using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BocciaPlayer {

    //入力待機中。
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
                //画面のタッチ位置を取得。
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
            else if(moveButton.IsPressAnyButton())
            {
                //移動ボタンが押されたら移動ステートに切り替える。
                m_player.ChangeState(EnPlayerState.enMove);
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
