using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BocciaPlayer
{
    //�{�[�����~�܂�܂őҋ@����B
    public class PlayerWaitBallState : IPlayerState
    {
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
        }
        override public void Enter()
        {
            isStop = true;
        }
        override public void Leave()
        {

        }
        override public void Execute()
        {
            if(!teamFlow.GetIsMoving())
            {
                isStop = false;
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
