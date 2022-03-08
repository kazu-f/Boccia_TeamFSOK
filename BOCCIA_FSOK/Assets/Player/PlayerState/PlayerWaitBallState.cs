using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BocciaPlayer
{
    //�{�[�����~�܂�܂őҋ@����B
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
            //�������uGUI�𖳌����B
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
                ////�X�e�[�g�����ɖ߂��B
                //if (netSendManager != null)
                //{
                //    netSendManager.SendState((int)EnPlayerDataState.enPlayerData_None);
                //}
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
            return isStop;
        }
    }
}
