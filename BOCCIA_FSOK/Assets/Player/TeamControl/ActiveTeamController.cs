using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveTeamController : MonoBehaviour
{
    enum ThrowTeamState 
    {
        throwBall,          //ボールを投げる。
        waitStopBall,       //ボールが止まるまで待つ。
        throwBallNone,      //投げるボールがない。
        State_Num
    };

    public BallFlowScript BallFlow;         //ジャックボールの判定。
    public TeamFlowScript TeamFlow;         //次投げるチームの判定。

    public GameObject RedTeamPlayer;        //赤チームプレイヤー。
    public GameObject BlueTeamPlayer;       //青チームプレイヤー。

    private BocciaPlayer.PlayerController RedPlayerCon;
    private BocciaPlayer.PlayerController BluePlayerCon;

    public Team startTeam = Team.Red;              //先行のプレイヤー。
    Team currentTeam;                              //現在のプレイヤー。
    ThrowTeamState throwState = ThrowTeamState.throwBall;

    // Start is called before the first frame update
    void Start()
    {
        RedPlayerCon = RedTeamPlayer.GetComponent<BocciaPlayer.PlayerController>();
        BluePlayerCon = BlueTeamPlayer.GetComponent<BocciaPlayer.PlayerController>();
        currentTeam = startTeam;
        //投げるプレイヤーに切り替え。
        ChangeActivePlayer();
    }

    // Update is called once per frame
    void Update()
    {
        //if(TeamFlow.GetIsMoving())
        //{
        //    return;
        //}
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
                    currentTeam = TeamFlow.GetNowTeam();
                    throwState = ThrowTeamState.throwBall;
                    ChangeActivePlayer();
                }

                break;
            default:

                break;

        }
    }

    //もう一エンド行う。
    public void ResetGame()
    {
        //ジャックボールから投げる。
        throwState = ThrowTeamState.throwBall;
        //先行のプレイヤーを変える。
        ChangeFirstPlayer();
    }

    //先行のプレイヤーを変更。
    void ChangeFirstPlayer()
    {
        //先行のプレイヤーを変更。
        if (startTeam == Team.Red)
        {
            startTeam = Team.Blue;              //先行のプレイヤー。
        }
        else
        {
            startTeam = Team.Red;              //先行のプレイヤー。
        }
    }

    /// <summary>
    /// 次投げるプレイヤーに有効フラグを切り替える。
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
    /// プレイヤーを止める。
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
