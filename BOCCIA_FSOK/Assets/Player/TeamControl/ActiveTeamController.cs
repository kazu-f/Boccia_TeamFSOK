using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveTeamController : MonoBehaviour
{
    enum ThrowTeamState 
    {
        throwJack,          //�W���b�N�{�[���B
        throwFirstBall,     //�ŏ��̈ꋅ�B
        throwAnyBall,       //���̑��̃{�[���B
        throwBallNone,      //������{�[�����Ȃ��B
        State_Num
    };

    public BallFlowScript BallFlow;         //�W���b�N�{�[���̔���B
    public TeamFlowScript TeamFlow;         //��������`�[���̔���B

    public GameObject RedTeamPlayer;        //�ԃ`�[���v���C���[�B
    public GameObject BlueTeamPlayer;       //�`�[���v���C���[�B

    private BocciaPlayer.PlayerController RedPlayerCon;
    private BocciaPlayer.PlayerController BluePlayerCon;

    public Team startTeam = Team.Red;              //��s�̃v���C���[�B
    Team currentTeam;                              //���݂̃v���C���[�B
    ThrowTeamState throwState = ThrowTeamState.throwJack;

    // Start is called before the first frame update
    void Start()
    {
        RedPlayerCon = RedTeamPlayer.GetComponent<BocciaPlayer.PlayerController>();
        BluePlayerCon = BlueTeamPlayer.GetComponent<BocciaPlayer.PlayerController>();
        currentTeam = startTeam;
        //������v���C���[�ɐ؂�ւ��B
        ChangeActivePlayer();
    }

    // Update is called once per frame
    void Update()
    {
        if(TeamFlow.GetIsMoving())
        {
            StopThrow();
            return;
        }
        switch(throwState)
        {
            case ThrowTeamState.throwJack:
                if(BallFlow.IsPreparedJack())
                {
                    throwState = ThrowTeamState.throwFirstBall;
                }

                break;
            case ThrowTeamState.throwFirstBall:
                if (!TeamFlow.GetIsMoving())
                {
                    throwState = ThrowTeamState.throwAnyBall;
                }

                break;
            case ThrowTeamState.throwAnyBall:
                if (!TeamFlow.GetIsMoving())
                {
                    currentTeam = TeamFlow.GetNowTeam();
                    ChangeActivePlayer();
                }

                break;
            default:

                break;

        }
    }

    //������G���h�s���B
    public void ResetGame()
    {
        //�W���b�N�{�[�����瓊����B
        throwState = ThrowTeamState.throwJack;
        //��s�̃v���C���[��ς���B
        ChangeFirstPlayer();
    }

    //��s�̃v���C���[��ύX�B
    void ChangeFirstPlayer()
    {
        //��s�̃v���C���[��ύX�B
        if (startTeam == Team.Red)
        {
            startTeam = Team.Blue;              //��s�̃v���C���[�B
        }
        else
        {
            startTeam = Team.Red;              //��s�̃v���C���[�B
        }
    }

    /// <summary>
    /// ��������v���C���[�ɗL���t���O��؂�ւ���B
    /// </summary>
    void ChangeActivePlayer()
    {
        if (currentTeam == Team.Red)
        {
            RedTeamPlayer.SetActive(true);
            RedPlayerCon.SwitchPlayer(true);
            BlueTeamPlayer.SetActive(false);
            BluePlayerCon.SwitchPlayer(false);
        }
        else if (currentTeam == Team.Blue)
        {
            RedTeamPlayer.SetActive(false);
            RedPlayerCon.SwitchPlayer(false);
            BlueTeamPlayer.SetActive(true);
            BluePlayerCon.SwitchPlayer(true);
        }
    }
    /// <summary>
    /// �v���C���[���~�߂�B
    /// </summary>
    void StopThrow()
    {
        RedTeamPlayer.SetActive(false);
        RedPlayerCon.enabled = false;
        BlueTeamPlayer.SetActive(false);
        BluePlayerCon.enabled = false;
    }

    public void SwichActiveThrow()
    {
        if (this.gameObject.activeSelf)
        {
            StopThrow();
            this.gameObject.SetActive(false);
        }
        else
        {
            ChangeActivePlayer();
            this.gameObject.SetActive(true);
        }
    }
}