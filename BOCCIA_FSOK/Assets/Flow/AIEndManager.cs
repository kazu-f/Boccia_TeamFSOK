using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class AIEndManager : MonoBehaviour
{
    [SerializeField] private TeamFlowScript m_TeamFlow = null;
    [SerializeField] private EndFlowScript m_EndFlow = null;
    [SerializeField] private TeamFlowDelayScript m_Delay = null;
    [SerializeField] private BallFlowScript m_BallFlow = null;
    [SerializeField] private TimerFillScript m_Timer = null;
    private Team MyTeamCol = Team.Num;
    
    //private bool SyncFlag = false;      //全員が同期で来た時に立てるフラグ
    private bool m_IsUseAI = false;
    private GameObject Failed = null;   //場外のオブジェクト
    int m_turnNo = 0;
    private void Awake()
    {

    }
    // Start is called before the first frame update
    void Start()
    {
        MyTeamCol = GameObject.Find("IsNetWorkObj").GetComponent<IsUseNetwork>().GetPlayerCol();
        m_IsUseAI = GameObject.Find("IsNetWorkObj").GetComponent<IsUseNetwork>().IsUseAI();
        if (MyTeamCol != Team.Red && MyTeamCol != Team.Blue)
        {
            Debug.LogError("チームのカラーが取得できませんでした。ネットワークオブジェが初期化されていない可能性があります");
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch (m_TeamFlow.GetState())
        {
            case TeamFlowState.Start:
                Debug.Log("TeamFlowState.Start");
                GameObject.Find("GameCamera").GetComponent<GameCameraScript>().SetIfFollow(false);
                //ボールを投げ始めるとき
                //一番初めなのでジャックプリーズと出す
                GameObject.Find("JackPlease").GetComponent<JackPleaseScript>().StartSlide();
                m_TeamFlow.SetState(TeamFlowState.Wait);
                //タイマーをスタートする
             //   Debug.Log("初めにタイマーをスタートします");
                m_Timer.SyncStartTimer();
                break;

            case TeamFlowState.ThrowStart:

                Debug.Log("TeamFlowState.ThrowStart");
                if (m_BallFlow.IsPreparedJack() == true)
                {
                   // Debug.Log("各種情報をセットします。");
                    //残りボール数のテキストのαを更新
                    GameObject.Find("RemainBallText").GetComponent<RemainBallNumScript>().UpdateAlpha();
                    //カメラを追従カメラから切り替える
                    GameObject.Find("GameCamera").GetComponent<GameCameraScript>().SetIfFollow(false);
                    //次に投げるチームをセット
                    m_TeamFlow.SetNextTeamForClass();
                    //残りボール数のテキストを更新
                    GameObject.Find("RemainBallText").GetComponent<RemainBallNumScript>().UpdateRemainText();
                }


                //Waitに移行
                //投げ始める。
                //Debug.Log("エンドが終わっていないのでタイマーをスタートします");
                m_Timer.SyncStartTimer();
                m_TeamFlow.SetState(TeamFlowState.Wait);
                
                break;

            case TeamFlowState.Wait:
                Debug.Log("TeamFlowState.Wait");

                //プレイヤーが投げるまで
                if (m_Timer.IsTimerStart() == false)
                {
                    //タイマーがまだスタートしていない
                    //Debug.Log("タイマーがまだスタートしていません。");
                    return;
                }

                if (m_Timer.IsTimeUp()/* || m_IsUseAI*/)
                {
                    //Debug.Log("タイムアップしているのでステートをCulcに変更");
                    if (m_BallFlow.IsPreparedJack())
                    {
                        //ジャックボールが準備されているとき
                        //プレイヤーの持ち球を減らす
                        //Debug.Log("タイムアップしたのでボールを減らします");
                        m_TeamFlow.DecreaseBalls();
                    }
                    m_TeamFlow.SetState(TeamFlowState.Caluc);
                }
                break;

            case TeamFlowState.Throw:
                Debug.Log("TeamFlowState.Throw");
                //ボールを投げた時
                //タイマーを止める
                m_Timer.TimerStop();
                //ステートをMoveにする
                m_TeamFlow.SetState(TeamFlowState.Move);
                break;

            case TeamFlowState.Move:
                Debug.Log("TeamFlowState.Move");
                //ボールがまだ動いている
                //全てのボールが停止しているか調べる
                if (m_TeamFlow.IsStopAllBalls())
                {
                    //全てのボールが停止しているとき
                    //ステートを停止にする
                    m_TeamFlow.SetState(TeamFlowState.Stop);
                }
                break;

            case TeamFlowState.Stop:
                Debug.Log("TeamFlowState.Stop");
                //全てのボールが停止している
                //遅延を開始させる
                m_Delay.DelayStart();
                m_TeamFlow.SetState(TeamFlowState.Delay);
                break;

            case TeamFlowState.Delay:
                Debug.Log("TeamFlowState.Delay");
                //遅延中                
                if (!m_Delay.IsDelay())
                {
                    Failed = GameObject.Find("Failed");
                    Failed.GetComponent<FailedMoveScript>().FontAlphaZero();
                    //遅延が終了した
                    m_TeamFlow.SetState(TeamFlowState.Caluc);
                    m_TeamFlow.ThrowEnd();
                }
                break;

            case TeamFlowState.Caluc:
                // 次のターンへ。
                m_turnNo++;
                Debug.Log("TeamFlowState.Caluc");
                //投げたチームかAI戦の時のみが計算をする
                //次に投げるボールを計算する
                if (m_BallFlow.IsPreparedJack() == false)
                {
                    //ジャックボールが用意されていないのでジャックボールを投げるのをミスしている
                    //ジャックボールを投げるチームを変更
                    m_TeamFlow.ChangeJackThrowTeam();
                    //計算ができなかったのでステートをThrowEndにする
                    //m_TeamFlow.SetState(TeamFlowState.ThrowEnd);
                }
                else
                {
                    //残りボール数のテキストのαを更新
                    //GameObject.Find("RemainBallText").GetComponent<RemainBallNumScript>().UpdateAlpha();
                    //ジャックボールは用意されているので次に投げるチームを計算
                    m_TeamFlow.CalucNextTeam();

                }

                //投げたチームは同期データ送信に移行
                m_TeamFlow.SetState(TeamFlowState.SyncEnd);
                break;

            case TeamFlowState.SyncEnd:
                Debug.Log("TeamFlowState.SyncEnd");

                bool endflag = true;
                //どちらとも投げ終えているときエンド終了に移行する
                foreach (int i in m_TeamFlow.GetRemainBalls())
                {
                    if (i != 0)
                    {
                        //まだ球があるので抜ける
                        //Debug.Log("まだ球があるのでゲームを続けます。");
                        endflag = false;
                        break;
                    }
                }
               
                if (endflag == true)
                {
                    //投げ終えているのでステートをEndにする
                    //Debug.Log("もうボールがないんでエンドを終了します");
                    m_TeamFlow.SetState(TeamFlowState.End);
                    break;
                }

                //ThrowStartにステートをセットする
                if (m_BallFlow.IsPreparedJack() == false)
                {
                    m_TeamFlow.SetState(TeamFlowState.Start);
                }
                else
                {
                    m_TeamFlow.SetState(TeamFlowState.ThrowStart);
                }

                break;

            case TeamFlowState.End:
                Debug.Log("TeamFlowState.End");
                //エンドが終わった時
                //エンドが終わったフラグを立てる
                m_EndFlow.GameEnd();
                break;

            default:
                return;
        }
    }
}
