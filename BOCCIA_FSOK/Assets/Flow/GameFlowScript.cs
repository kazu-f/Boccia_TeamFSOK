using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
/// <summary>
/// ゲーム進行制御
/// １．エンド数の管理
/// ２．エンド終了時のリセット
/// ３．全エンド終了時シーン遷移
/// </summary>

public class GameFlowScript : MonoBehaviour
{
    private SwichActiveGameObjects switchActiveObjs;
    private EndFlowScript endFlow;   //エンド進行制御スクリプト。
    private TeamFlowScript teamFlow;   //投げるチームを決定するスクリプト。
    public ActiveTeamController activePlayerController;    //プレイヤー制御。
    private ChangeSceneScript changeScene;              //シーン切り替え制御スクリプト。
    private GameScore.GameScoreScript gameScore;        //スコア記録用スクリプト。

    [Range(2,6)]public int GAME_FINISH_END = 2;    //1ゲーム辺りのエンド数。
    private int currentEndNo = 0;               //現在のエンド数。

    private bool waitFlag = false;  //処理を待機する。
    private const float WAIT_TIME = 2.0f;   //待機時間。

    // Start is called before the first frame update
    void Start()
    {
        switchActiveObjs = SwichActiveGameObjects.GetInstance();
        endFlow = this.gameObject.GetComponent<EndFlowScript>();                //エンド制御スクリプト取得。
        teamFlow = this.gameObject.GetComponent<TeamFlowScript>();              //投げるチームを決定するスクリプト取得。
        changeScene = this.gameObject.GetComponent<ChangeSceneScript>();        //シーン切り替え制御スクリプト取得。
        gameScore = this.gameObject.GetComponent<GameScore.GameScoreScript>();  //スコア記録スクリプト取得。

        //最終エンド数を設定。
        gameScore.SetFinalEndNum(GAME_FINISH_END);
    }

    // Update is called once per frame
    void Update()
    {
        if(waitFlag)
        {
            return;
        }

        //そのエンドが終了した。
        if(endFlow.GetCalced() && !waitFlag)
        {
            //そのエンドのリザルト記録。
            RecordEndResult();
            //エンド数を進める。
            currentEndNo++;
            waitFlag = true;
            if (currentEndNo < GAME_FINISH_END)
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
        //else
        //{
        //    //ボールが動いている。
        //    if (teamFlow.GetIsMoving())
        //    {
        //        //特定のオブジェクトを止める。
        //        switchActiveObjs.SwitchGameObject(false);
        //    }
        //    else
        //    {
        //        //ボールが動いてなければ戻す。
        //        switchActiveObjs.SwitchGameObject(true);
        //    }
        //}
        
    }

    /// <summary>
    /// エンドリザルトを記録する。
    /// </summary>
    private void RecordEndResult()
    {
        //勝利チーム情報取得。
        var vicTeam = endFlow.GetVictoryTeam();
        GameScore.EndResult endResult = new GameScore.EndResult();      //記録する変数。
        //勝利チーム判定。
        if(vicTeam.GetNearestTeam() == Team.Red)
        {
            endResult.redTeamScore = vicTeam.GetScore();
        }
        else if(vicTeam.GetNearestTeam() == Team.Blue)
        {
            endResult.blueTeamScore = vicTeam.GetScore();
        }
        //変数を記録。
        gameScore.RecordResult(endResult, currentEndNo);
    }

    /// <summary>
    /// ゲームが終了した。
    /// </summary>
    private void FinishGame()
    {
        //シーンを切り替える。
        changeScene.ChangeSceneInvoke(false, WAIT_TIME);
        //シーン切り替え時の処理を追加。
        SceneManager.sceneLoaded += gameScore.SendScoreNextScene;
    }

    /// <summary>
    /// エンドをリスタート。
    /// </summary>
    private void RestartEnd()
    {
        if (currentEndNo % 2 == 0)
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
}
