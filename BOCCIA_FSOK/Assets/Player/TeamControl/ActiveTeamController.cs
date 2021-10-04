using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveTeamController : MonoBehaviour
{
    enum ThrowTeamState 
    {
        throwBall,          //�{�[���𓊂���B
        waitStopBall,       //�{�[�����~�܂�܂ő҂B
        throwBallNone,      //������{�[�����Ȃ��B
        finishEnd,          //�G���h�I���B
        State_Num
    };

    public BallFlowScript BallFlow;         //�W���b�N�{�[���̔���B
    public TeamFlowScript TeamFlow;         //��������`�[���̔���B
    public EndFlowScript EndFlow;         //�G���h�i�s�B

    public GameObject RedTeamPlayer;        //�ԃ`�[���v���C���[�B
    public GameObject BlueTeamPlayer;       //�`�[���v���C���[�B

    private BocciaPlayer.PlayerController RedPlayerCon;
    private BocciaPlayer.PlayerController BluePlayerCon;

    Team currentTeam;                              //���݂̃v���C���[�B
    ThrowTeamState throwState = ThrowTeamState.throwBall;

    // Start is called before the first frame update
    void Start()
    {
        RedPlayerCon = RedTeamPlayer.GetComponent<BocciaPlayer.PlayerController>();
        BluePlayerCon = BlueTeamPlayer.GetComponent<BocciaPlayer.PlayerController>();
        //������v���C���[��؂�ւ��B
        ChangeActivePlayer();
    }

    // Update is called once per frame
    void Update()
    {
        if(EndFlow.GetIsEnd() && throwState != ThrowTeamState.finishEnd)
        {
            throwState = ThrowTeamState.finishEnd;
        }
        switch(throwState)
        {
            case ThrowTeamState.throwBall:
                if (TeamFlow.GetIsMoving())
                {
                    throwState = ThrowTeamState.waitStopBall;
                }

                break;
            case ThrowTeamState.waitStopBall:
                StopThrow();
                if (!TeamFlow.GetIsMoving())
                {
                    throwState = ThrowTeamState.throwBall;
                    ChangeActivePlayer();
                }

                break;
            case ThrowTeamState.finishEnd:
                StopThrow();

                break;
            default:

                break;

        }
    }

    //������G���h�s���B
    public void RestartGame()
    {
        //�{�[���𓊂���B
        throwState = ThrowTeamState.throwBall;
        //�v���C���[�̗L���t���O�؂�ւ��B
        ChangeActivePlayer();
    }

    /// <summary>
    /// ��������v���C���[�ɗL���t���O��؂�ւ���B
    /// </summary>
    void ChangeActivePlayer()
    {
        //���ɓ�����`�[���擾�B
        currentTeam = TeamFlow.GetNowTeam();
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
