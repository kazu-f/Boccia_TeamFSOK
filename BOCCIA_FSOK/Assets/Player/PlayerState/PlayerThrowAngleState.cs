using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BocciaPlayer
{
    public class PlayerThrowAngleState : IPlayerState
    {
        private TouchManager touchManager;
        private NetworkSendManagerScript netSendManager;
        private ThrowAngleController angleController;   //向きを変更するスクリプト。
        private float xSpeed = 0.0f;                    //回転量。

        override public void Init(PlayerController controller)
        {
            m_player = controller;
            touchManager = TouchManager.GetInstance();
            if (m_player)
            {
                //向きを変更するスクリプトを取得。
                angleController = m_player.GetThrowAngleController();
                //向きを変更するスクリプトを無効化。
                angleController.enabled = false;
                netSendManager = m_player.GetNetSendManager();
            }
        }
        override public void Enter()
        {
            //もろもろuGUIを無効化。
            SwichActiveGameObjects.GetInstance().SwitchGameObject(false);
            //向きを変更するスクリプトを有効化。
            angleController.enabled = true;

        }
        override public void Leave()
        {
            //向きを変更するスクリプトを無効化。
            angleController.enabled = false;

        }
        override public void Execute()
        {
            if (touchManager.IsTouch())
            {
                if (touchManager.GetPhase() == TouchInfo.Moved)
                {
                    //タッチが移動したら速度を求める。
                    xSpeed = touchManager.GetDeltaPosInScreen().x;
                    angleController.AngleRotation(xSpeed);
                    netSendManager.SendQuaternion(m_player.transform.rotation);
                }
                else if(touchManager.GetPhase() == TouchInfo.Ended)
                {
                    //指を離したらステート変更。
                    m_player.ChangeState(EnPlayerState.enIdle);
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
