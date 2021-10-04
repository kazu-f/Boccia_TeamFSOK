using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// ゲーム進行制御
/// １．エンド数の管理
/// ２．エンド終了時のリセット
/// ３．全エンド終了時シーン遷移
/// </summary>

public class GameFlowScript : MonoBehaviour
{
    public GameObject[] gameObjects;    //ボールが動いている間止めるオブジェクトたち。
    private EndFlowScript endFlow;   //エンド進行制御スクリプト。
    private TeamFlowScript teamFlow;   //投げるチームを決定するスクリプト。
    public ActiveTeamController activePlayerController;    //プレイヤー制御。
    private ChangeSceneScript changeScene;              //シーン切り替え制御スクリプト。

    [Range(2,6)]public int GAME_FINISH_END = 2;    //1ゲーム辺りのエンド数。
    private int nowEndNo = 0;               //現在のエンド数。

    private bool waitFlag = false;  //処理を待機する。
    private const float WAIT_TIME = 2.0f;   //待機時間。

    // Start is called before the first frame update
    void Start()
    {
        endFlow = this.gameObject.GetComponent<EndFlowScript>();
        teamFlow = this.gameObject.GetComponent<TeamFlowScript>();
        changeScene = this.gameObject.GetComponent<ChangeSceneScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if(waitFlag)
        {
            return;
        }

        //そのエンドが終了した。
        if(endFlow.GetIsEnd() && !waitFlag)
        {
            nowEndNo++;
            waitFlag = true;
            if (nowEndNo < GAME_FINISH_END)
            {
                //エンドを再スタート。
                Invoke("RestartEnd", WAIT_TIME);
            }
            else
            {
                //ゲーム終了。
                FinishGame();
            }
        }
        else
        {
            //ボールが動いている。
            if (teamFlow.GetIsMoving())
            {
                //特定のオブジェクトを止める。
                SwitchGameObject(false);
            }
            else
            {
                //ボールが動いてなければ戻す。
                SwitchGameObject(true);
            }
        }
        
    }

    /// <summary>
    /// ゲームが終了した。
    /// </summary>
    private void FinishGame()
    {
        //シーンを2秒後切り替える。
        changeScene.ChangeScene(false, WAIT_TIME);
    }

    /// <summary>
    /// エンドをリスタート。
    /// </summary>
    private void RestartEnd()
    {
        if (nowEndNo % 2 == 0)
        {
            teamFlow.SetFirstTeam(Team.Red);
        }
        else
        {
            teamFlow.SetFirstTeam(Team.Blue);
        }
        endFlow.ResetVar();                         //エンド進行制御をリセット。
        activePlayerController.RestartGame();       //プレイヤーをリセット。  

        waitFlag = false;
    }
    /// <summary>
    /// ボールが動いている間ゲームオブジェクトを動かさない。
    /// </summary>
    private void SwitchGameObject(bool isActive)
    {
        if(!(gameObjects.Length > 0))
        {
            return;
        }

        foreach(var obj in gameObjects)
        {
            obj.gameObject.SetActive(isActive);
        }
    }
}
