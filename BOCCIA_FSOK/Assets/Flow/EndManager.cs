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
    private Team MyTeamCol = Team.Num;

    private void Awake()
    {

    }
    // Start is called before the first frame update
    void Start()
    {
        MyTeamCol = GameObject.Find("IsNetWorkObj").GetComponent<IsUseNetwork>().GetPlayerCol();
        if (MyTeamCol != Team.Red && MyTeamCol != Team.Blue)
        {
            Debug.LogError("チームのカラーが取得できませんでした。ネットワークオブジェが初期化されていない可能性があります");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!PhotonNetwork.OfflineMode)
        {
            if (MyTeamCol != m_TeamFlow.GetNowTeam())
            {
                //SendManagerの取得
                NetworkSendManagerScript manager = GameObject.Find("SendNetWorkObj").GetComponent<NetworkSendManagerScript>();
                var IsFollow = manager.ReceiveIsCameraFollow();
                //追従カメラかどうかをマスタークライアントに合わせる
                GameObject.Find("GameCamera").GetComponent<GameCameraScript>().SetIfFollow(IsFollow);
                var NextTeam = manager.ReceiveNextTeam();
                //次のチームをマスタークライアントに合わせる
                if (NextTeam != Team.Num)
                {
                    m_TeamFlow.SetNextTeam(NextTeam);
                    m_TeamFlow.SetNextTeamForClass();
                }
                return;
            }
        }
        switch (m_TeamFlow.GetState())
        {
            case TeamFlowState.Start:
                //ボールを投げ始めるとき
                //一番初めなのでジャックプリーズと出す
                GameObject.Find("JackPlease").GetComponent<JackPleaseScript>().StartSlide();
                m_TeamFlow.SetState(TeamFlowState.Wait);
                //タイマーをスタートする
                GameObject.Find("Timer").GetComponent<TimerFillScript>().TimerStart();
                break;

            case TeamFlowState.Wait:
                //プレイヤーが投げるまで
                if(GameObject.Find("Timer").GetComponent<TimerFillScript>().IsTimeUp())
                {
                    if(m_BallFlow.IsPreparedJack())
                    {
                        //ジャックボールが準備されているとき
                        //プレイヤーの持ち球を減らす
                        m_TeamFlow.DecreaseBalls();
                    }
                    m_TeamFlow.SetState(TeamFlowState.Caluc);
                }
                break;

            case TeamFlowState.Throw:
                //ボールを投げた時
                //タイマーを止める
                GameObject.Find("Timer").GetComponent<TimerFillScript>().TimerStop();
                //ステートをMoveにする
                m_TeamFlow.SetState(TeamFlowState.Move);
                break;

            case TeamFlowState.Move:
                //ボールがまだ動いている
                //全てのボールが停止しているか調べる
                if(m_TeamFlow.IsStopAllBalls())
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
                m_TeamFlow.ThrowEnd();
                break;

            case TeamFlowState.Delay:
                //遅延中                
                if(!m_Delay.IsDelay())
                {
                    //遅延が終了した
                    m_TeamFlow.SetState(TeamFlowState.Caluc);
                }
                break;

            case TeamFlowState.Caluc:
                //次に投げるボールを計算する
                if(m_BallFlow.IsPreparedJack() == false)
                {
                    //ジャックボールが用意されていないのでジャックボールを投げるのをミスしている
                    //ジャックボールを投げるチームを変更
                    m_TeamFlow.ChangeJackThrowTeam();
                    //計算ができなかったのでステートをEndにする
                    m_TeamFlow.SetState(TeamFlowState.ThrowEnd);
                }
                else
                {
                    //ジャックボールは用意されているので次に投げるチームを計算
                    if (m_TeamFlow.CalucNextTeam() == true)
                    {
                        //計算ができたのでステートをCalucedにする
                        m_TeamFlow.SetState(TeamFlowState.Caluced);
                    }
                    else
                    {
                        //計算ができなかったのでステートをEndにする
                        m_TeamFlow.SetState(TeamFlowState.End);
                    }
                }
                break;

            case TeamFlowState.Caluced:
                //次に投げるボールが計算できた時
                m_TeamFlow.SetState(TeamFlowState.ThrowEnd);
                break;

            case TeamFlowState.ThrowEnd:
                //投げ終わり
                //カメラを追従カメラから切り替える
                GameObject.Find("GameCamera").GetComponent<GameCameraScript>().SetIfFollow(false);
                m_TeamFlow.SetState(TeamFlowState.ChangeTeam);
                break;

            case TeamFlowState.ChangeTeam:
                GameObject.Find("SendNetWorkObj").GetComponent<NetworkSendManagerScript>().SendNextTeam(m_TeamFlow.GetNowTeam());
                //次に投げるチームをセット
                m_TeamFlow.SetNextTeamForClass();
                //タイマースタート
                GameObject.Find("Timer").GetComponent<TimerFillScript>().TimerStart();
                m_TeamFlow.SetState(TeamFlowState.ChangeEnd);
                break;

            case TeamFlowState.ChangeEnd:
                //チーム変え終わった
                m_TeamFlow.SetState(TeamFlowState.Wait);
                break;

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
