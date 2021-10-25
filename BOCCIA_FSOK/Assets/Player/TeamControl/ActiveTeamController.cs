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
        finishEnd,          //エンド終了。
        State_Num
    };

    public BallFlowScript BallFlow;         //ジャックボールの判定。
    public TeamFlowScript TeamFlow;         //次投げるチームの判定。
    public EndFlowScript EndFlow;         //エンド進行。

    public GameObject RedTeamPlayer;        //赤チームプレイヤー。
    public GameObject BlueTeamPlayer;       //青チームプレイヤー。

    private BocciaPlayer.IPlayerController RedPlayerCon;
    private BocciaPlayer.IPlayerController BluePlayerCon;

    Team currentTeam;                              //現在のプレイヤー。
    ThrowTeamState throwState = ThrowTeamState.throwBall;

    // Start is called before the first frame update
    void Start()
    {
        RedPlayerCon = RedTeamPlayer.GetComponent<BocciaPlayer.IPlayerController>();
        BluePlayerCon = BlueTeamPlayer.GetComponent<BocciaPlayer.IPlayerController>();
        //投げるプレイヤーを切り替え。
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

    //もう一エンド行う。
    public void RestartGame()
    {
        //ボールを投げる。
        throwState = ThrowTeamState.throwBall;
        //プレイヤーのリセット。
        RedPlayerCon.ResetPlayer();
        BluePlayerCon.ResetPlayer();

        //プレイヤーの有効フラグ切り替え。
        ChangeActivePlayer();
    }

    /// <summary>
    /// 次投げるプレイヤーに有効フラグを切り替える。
    /// </summary>
    void ChangeActivePlayer()
    {
        //次に投げるチーム取得。
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
    /// プレイヤーを止める。
    /// </summary>
    void StopThrow()
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
