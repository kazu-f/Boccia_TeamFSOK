using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BocciaPlayer
{
    //ボールが止まるまで待機する。
    public class PlayerWaitBallState : IPlayerState
    {
        NetworkSendManagerScript netSendManager = null;
        TeamFlowScript teamFlow;
        bool isStop = true;
        override public void Init(PlayerController controller)
        {
            m_player = controller;
            var gameFlow = GameObject.FindGameObjectWithTag("GameFlow");
            if (gameFlow)
            {
                teamFlow = gameFlow.GetComponent<TeamFlowScript>();
            }
            if (m_player)
            {
                netSendManager = m_player.GetNetSendManager();
            }
        }
        override public void Enter()
        {
            //もろもろuGUIを無効化。
            SwichActiveGameObjects.GetInstance().SwitchGameObject(false);
            isStop = true;
        }
        override public void Leave()
        {

        }
        override public void Execute()
        {
            if (!teamFlow.GetIsMoving())
            {
                isStop = false;
                ////ステートを元に戻す。
                //if (netSendManager != null)
                //{
                //    netSendManager.SendState((int)EnPlayerDataState.enPlayerData_None);
                //}
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
            return isStop;
        }
    }
}
