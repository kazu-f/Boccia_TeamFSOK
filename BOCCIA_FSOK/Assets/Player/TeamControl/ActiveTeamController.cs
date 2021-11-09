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

    private NetworkLauncherScript netLauncher = null;
    private BocciaPlayer.IPlayerController RedPlayerCon = null;
    private BocciaPlayer.IPlayerController BluePlayerCon = null;

    Team currentTeam;                              //���݂̃v���C���[�B
    Team playerTeamCol;                             //�v���C���[�̃`�[���J���[�B
    ThrowTeamState throwState = ThrowTeamState.throwBall;

    private void Awake()
    {

    }
    // Start is called before the first frame update
    void Start()
    {
        var netObj = GameObject.FindGameObjectWithTag("Network");
        var isUseNetwork = netObj.GetComponent<IsUseNetwork>();
        if (!isUseNetwork.IsUseAI())
        {
            netLauncher = netObj.GetComponent<NetworkLauncherScript>();
            //�����`���[���擾�B
            if (netLauncher != null)
            {
                if (netLauncher.IsMasterClient())
                {
                    RedTeamPlayer.AddComponent<BocciaPlayer.PlayerController>();
                    BlueTeamPlayer.AddComponent<BocciaPlayer.PhotonPlayerController>();
                }
                else
                {
                    RedTeamPlayer.AddComponent<BocciaPlayer.PhotonPlayerController>();
                    BlueTeamPlayer.AddComponent<BocciaPlayer.PlayerController>();
                }
            }
        }
        else
        {
            RedTeamPlayer.AddComponent<BocciaPlayer.PlayerController>();
            BlueTeamPlayer.AddComponent<BocciaPlayer.PlayerController>();
        }
        RedPlayerCon = RedTeamPlayer.GetComponent<BocciaPlayer.IPlayerController>();
        BluePlayerCon = BlueTeamPlayer.GetComponent<BocciaPlayer.IPlayerController>();
        //������v���C���[��؂�ւ��B
        ChangeActivePlayer();
    }

    // Update is called once per frame
    void Update()
    {
        if(EndFlow.GetIsEnd() && throwState != ThrowTeamState.finishEnd)
        {
            StopThrow();
            throwState = ThrowTeamState.finishEnd;
        }
        switch(throwState)
        {
            case ThrowTeamState.throwBall:
                if (TeamFlow.GetIsMoving())
                {
                    StopThrow();
                    throwState = ThrowTeamState.waitStopBall;
                }

                break;
            case ThrowTeamState.waitStopBall:
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
        //�v���C���[�̃��Z�b�g�B
        RedPlayerCon.ResetPlayer();
        BluePlayerCon.ResetPlayer();

        //�v���C���[�̗L���t���O�؂�ւ��B
        ChangeActivePlayer();
    }

    /// <summary>
    /// ��������v���C���[�ɗL���t���O��؂�ւ���B
    /// </summary>
    public void ChangeActivePlayer()
    {
        //���ɓ�����`�[���擾�B
        currentTeam = TeamFlow.GetNowTeam();
        if (currentTeam == Team.Red)
        {
            RedPlayerCon.SwitchPlayer(true);
            BluePlayerCon.SwitchPlayer(false);
        }
        else if (currentTeam == Team.Blue)
        {
            RedPlayerCon.SwitchPlayer(false);
            BluePlayerCon.SwitchPlayer(true);
        }
    }
    /// <summary>
    /// �v���C���[���~�߂�B
    /// </summary>
    public void StopThrow()
    {
        RedPlayerCon.SwitchPlayer(false);
        BluePlayerCon.SwitchPlayer(false);
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
