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

    private NetworkLauncherScript netLauncher = null;
    private BocciaPlayer.IPlayerController RedPlayerCon = null;
    private BocciaPlayer.IPlayerController BluePlayerCon = null;

    Team currentTeam;                              //現在のプレイヤー。
    Team playerTeamCol;                             //プレイヤーのチームカラー。
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
            //ランチャーを取得。
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
    public void ChangeActivePlayer()
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
