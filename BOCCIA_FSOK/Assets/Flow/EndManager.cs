using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndManager : MonoBehaviour
{
    [SerializeField] TeamFlowScript m_TeamFlow;
    [SerializeField] EndFlowScript m_EndFlow;
    [SerializeField] TeamFlowDelayScript m_Delay;
    [SerializeField] BallFlowScript m_BallFlow;
    // Start is called before the first frame update
    void Start()
    {
        
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
                break;

            case TeamFlowState.Throw:
                //ボールを投げた時
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
                //次に投げるチームをセット
                m_TeamFlow.SetNextTeam();
                //カメラを追従カメラから切り替える
                GameObject.Find("GameCamera").GetComponent<GameCameraScript>().SwitchCamera();
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
