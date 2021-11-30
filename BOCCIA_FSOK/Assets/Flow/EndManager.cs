using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class EndManager : MonoBehaviour
{
    [SerializeField] private TeamFlowScript m_TeamFlow = null;
    [SerializeField] private EndFlowScript m_EndFlow = null;
    [SerializeField] private TeamFlowDelayScript m_Delay = null;
    [SerializeField] private BallFlowScript m_BallFlow = null;
    [SerializeField] private TimerFillScript m_Timer = null;
    private Team MyTeamCol = Team.Num;
    private bool[] SyncFlags = new bool[2];     //全員用の同期フラグ
    //private bool SyncFlag = false;      //全員が同期で来た時に立てるフラグ
    private bool m_IsUseAI = false;
    [SerializeField] private NetworkSendManagerScript m_SendManager = null;
    private bool Client = false;
    private GameObject Failed = null;   //場外のオブジェクト
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
                //ボールを投げ始めるとき
                //一番初めなのでジャックプリーズと出す
                GameObject.Find("JackPlease").GetComponent<JackPleaseScript>().StartSlide();
                m_TeamFlow.SetState(TeamFlowState.Wait);
                //タイマーをスタートする
                Debug.Log("初めにタイマーをスタートします");
                m_Timer.SyncStartTimer();
                break;

            case TeamFlowState.ThrowStart:
                Client = false;
                if (m_BallFlow.IsPreparedJack() == true)
                {
                    Debug.Log("各種情報をセットします。");
                    //残りボール数のテキストのαを更新
                    GameObject.Find("RemainBallText").GetComponent<RemainBallNumScript>().UpdateAlpha();
                    //カメラを追従カメラから切り替える
                    GameObject.Find("GameCamera").GetComponent<GameCameraScript>().SetIfFollow(false);
                    //次に投げるチームをセット
                    m_TeamFlow.SetNextTeamForClass();
                    //残りボール数のテキストを更新
                    GameObject.Find("RemainBallText").GetComponent<RemainBallNumScript>().UpdateRemainText();
                }

                //投げ始める。
                Debug.Log("エンドが終わっていないのでタイマーをスタートします");
                m_Timer.SyncStartTimer();
                //同期フラグをfalseにする
                SyncFlags[0] = false;
                SyncFlags[1] = false;
                //Waitに移行
                m_TeamFlow.SetState(TeamFlowState.Wait);
                break;

            case TeamFlowState.Wait:
                //プレイヤーが投げるまで
                //if (m_Timer.IsTimerStart() == false)
                //{
                //    //タイマーがまだスタートしていない
                //    Debug.Log("タイマーがまだスタートしていません。");
                //    return;
                //}

                if (m_Timer.IsTimeUp() || m_IsUseAI)
                {
                    if (m_Timer.IsTimeUpForAI())
                    {
                        Debug.Log("タイムアップしているのでステートをCulcに変更");
                        if (m_BallFlow.IsPreparedJack())
                        {
                            //ジャックボールが準備されているとき
                            //プレイヤーの持ち球を減らす
                            Debug.Log("タイムアップしたのでボールを減らします");
                            m_TeamFlow.DecreaseBalls();
                        }
                        m_TeamFlow.SetState(TeamFlowState.Caluc);
                    }
                }
                break;

            case TeamFlowState.Throw:
                //ボールを投げた時
                //タイマーを止める
                m_Timer.TimerStop();
                //ステートをMoveにする
                m_TeamFlow.SetState(TeamFlowState.Move);
                break;

            case TeamFlowState.Move:
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
                //全てのボールが停止している
                //遅延を開始させる
                m_Delay.DelayStart();
                m_TeamFlow.SetState(TeamFlowState.Delay);
                break;

            case TeamFlowState.Delay:
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
                Debug.Log("計算開始");
                if (m_TeamFlow.GetNowTeam() == MyTeamCol|| m_IsUseAI)
                {
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
                        //if (m_TeamFlow.CalucNextTeam() == true)
                        //{
                        ////計算ができたのでステートをCalucedにする
                        //m_TeamFlow.SetState(TeamFlowState.Caluced);
                        //}
                    }
                    //投げたチームは次に投げるチームを計算で来たのでSendInfoに移行する
                    m_TeamFlow.SetState(TeamFlowState.SendInfo);
                    break;
                }
                //投げていないチームはSyncに移行
                m_TeamFlow.SetState(TeamFlowState.Sync);
                break;

            //case TeamFlowState.Caluced:
            //    //次に投げるボールが計算できた時
            //    //次に投げるチームをセット
            //    //m_TeamFlow.SetNextTeamForClass();
            //    //m_TeamFlow.SetState(TeamFlowState.ThrowEnd);
            //    break;

            case TeamFlowState.SendInfo:
                Debug.Log("情報を送ります。");
                //情報を送る側なので0番目はtrue
                SyncFlags[0] = true;

                //情報を送る
                m_SendManager.SendRemainBalls(m_TeamFlow.GetRemainBalls());
                m_SendManager.SendNextTeam((int)m_TeamFlow.GetNowTeam());

                //m_SendManager.SendSyncFlag(SyncFlags);
                m_SendManager.SendMasterSyncFlag(SyncFlags[0]);

                //同期ステートに移行
                m_TeamFlow.SetState(TeamFlowState.SyncWait);
                break;

            case TeamFlowState.Sync:
                Client = true;

                //if (SyncFlag == false)
                //{
                //SyncFlags = m_SendManager.ReceiveSyncFlag();
                SyncFlags[0] = m_SendManager.ReceiveMasterSyncFlag();
                //まだ同期がとれていないとき
                //まず前に投げた人の情報が送られているかどうかを調べる
                if (SyncFlags[0] == false)
                {
                    //まだ送られていない。
                    break;
                }
                //////////////////////////////////////////////////////////
                ////////////ここから下は情報が送られているとき////////////
                //////////////////////////////////////////////////////////
                
                Debug.Log("同期開始");

                //自分が次に投げるとき前の情報を同期させる。
                //同期させる情報
                //｛残りのボールの個数、次に投げるチーム、｝
                m_TeamFlow.SetRemainBalls(m_SendManager.ReceiveRemainBalls());
                m_TeamFlow.SetNextTeam(m_SendManager.ReceiveNextTeam());
                //同期したのでフラグをセットする
                SyncFlags[1] = true;

                //同期が終わったことを知らせる。
                //m_SendManager.SendSyncFlag(SyncFlags);
                m_SendManager.SendClientSyncFlag(SyncFlags[1]);

                //同期が終わったのでSyncWaitステートに移行する
                Debug.Log("SyncWaitステートに移行");
                m_TeamFlow.SetState(TeamFlowState.SyncWait);

                //    SyncFlag = true;
                //}

                break;

            case TeamFlowState.SyncWait:
                //同期したかどうか調べる
                //SyncFlags = m_SendManager.ReceiveSyncFlag();
                if (Client)
                {
                    SyncFlags[0] = m_SendManager.ReceiveMasterSyncFlag();
                }
                else
                {
                    SyncFlags[1] = m_SendManager.ReceiveClientSyncFlag();
                }

                if (m_IsUseAI == false)
                {
                    //AI戦じゃ無いとき
                    foreach (bool flag in SyncFlags)
                    {
                        if (flag == false)
                        {
                            //まだ同期が終わっていない
                            Debug.Log("まだ同期が終わっていません");
                            return;
                        }
                    }
                }

                //////////////////////////////////////////////////////////
                /////////ここから下は同期が終わっているときの処理/////////
                //////////////////////////////////////////////////////////

                bool endflag = true;
                //どちらとも投げ終えているときエンド終了に移行する
                foreach(int i in m_TeamFlow.GetRemainBalls())
                {
                    if(i != 0)
                    {
                        //まだ球があるので抜ける
                        Debug.Log("まだ球があるのでゲームを続けます。");
                        endflag = false;
                        break;
                    }
                }
                if(endflag == true)
                {
                    //投げ終えているのでステートをEndにする
                    Debug.Log("もうボールがないんドでエンドを終了します");
                    m_TeamFlow.SetState(TeamFlowState.End);
                    break;
                }

                //////////////////////////////////////////////////////////
                //////////ここから下はまだ投げ終わっていないとき//////////
                //////////////////////////////////////////////////////////

                //SyncEndにステートをセットする
                m_TeamFlow.SetState(TeamFlowState.SyncEnd);

                break;

            case TeamFlowState.SyncEnd:
                //同期が完了しているフラグをリセットする
                if (Client == false)
                {
                    if (m_IsUseAI == false)
                    {
                        if (m_SendManager.ReceiveClientSyncFlag())
                        {
                            //クライアントの同期が終わっている
                            Debug.Log("ホストだよ！");
                            Debug.Log("同期フラグ送ったよ！");
                            SyncFlags[0] = false;
                            SyncFlags[1] = false;
                            m_SendManager.SendMasterSyncFlag(SyncFlags[0]);
                            m_SendManager.SendClientSyncFlag(SyncFlags[1]);
                        }
                        else
                        {
                            //クライアントの同期が終わっていない
                            return;
                        }
                    }
                }
                SyncFlags[0] = m_SendManager.ReceiveMasterSyncFlag();
                SyncFlags[1] = m_SendManager.ReceiveClientSyncFlag();

                if (m_IsUseAI == false)
                {
                    //AI戦じゃ無いとき
                    foreach (bool flag in SyncFlags)
                    {
                        if (flag == true)
                        {
                            //まだ同期が終わっていない
                            Debug.Log("まだ同期が終わっていません");
                            return;
                        }
                    }
                }

                //フラグリセットが完了したのを確認したので
                //ThrowStartにステートをセットする
                m_TeamFlow.SetState(TeamFlowState.ThrowStart);

                break;

            //case TeamFlowState.ThrowEnd:
            //    //投げ終わり
            //    //カメラを追従カメラから切り替える
            //    GameObject.Find("GameCamera").GetComponent<GameCameraScript>().SetIfFollow(false);
            //    m_TeamFlow.SetState(TeamFlowState.ChangeEnd);
            //    break;

            //case TeamFlowState.ChangeEnd:
            //    //チーム変え終わった
            //    //残りボール数のテキストを更新
            //    GameObject.Find("RemainBallText").GetComponent<RemainBallNumScript>().UpdateRemainText();

            //    break;

            case TeamFlowState.End:
                //エンドが終わった時
                //エンドが終わったフラグを立てる
                m_EndFlow.GameEnd();
                break;

            default:
                return;
        }
    }
}
