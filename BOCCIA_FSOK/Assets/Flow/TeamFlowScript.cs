using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TeamFlowState
{
    Start,
    Throw,
    Move,
    Stop,
    Delay,
    Caluc,
    Caluced,
    End,
    Num,
}
public class TeamFlowScript : MonoBehaviour
{
    public TeamFlowState m_state;
    public Team m_NextTeam = Team.Num;
    private Team m_firstTeam = Team.Red;
    private BallFlowScript m_BallFlow = null;
    private GameObject m_Jack = null;
    private BallState m_JackState = BallState.Num;
    private Vector3 m_JackPos = Vector3.zero;
    private Vector2Int m_RemainBalls = Vector2Int.one;
    private int m_Remain = 6;
    private EndFlowScript m_GameFlowScript = null;
    private bool m_IsMoving = false;
    private bool IsThrow = false;
    private int m_Frame = 0;
    private GameObject m_NextBallImage = null;

    private void Awake()
    {
        m_BallFlow = GetComponent<BallFlowScript>();
        m_GameFlowScript = GetComponent<EndFlowScript>();
        m_NextBallImage = GameObject.Find("NextBallImage");
        if(m_NextBallImage == null)
        {
            Debug.LogError("NextBallImageの取得に失敗しました");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //初めに投げるチームを決定
        m_NextTeam = m_firstTeam;
        //ボールの所持数を増やす
        m_RemainBalls *= m_Remain;
        ////一番初めなのでジャックプリーズと出す
        //GameObject.Find("JackPlease").GetComponent<JackPleaseScript>().StartSlide();
        //ログを流す
        NextTeamLog();
    }

    // Update is called once per frame
    void Update()
    {
        if (IsThrow)
        {
            //ステートを投げた状態にする
            m_state = TeamFlowState.Throw;
            ////1フレーム目だとうまくいかないので少し遅延させる
            //if (m_Frame < 10)
            //{
            //    m_Frame++;
            //    return;
            //}

            ////ボールを投げた時
            //IsStopAllBalls();
            if (m_IsMoving == false)
            {
                //if (m_BallFlow.IsPreparedJack() == false)
                //{
                //    ChangeJackThrowTeam();
                //    return;
                //}
                //全てのボールが止まっているとき
                //IsThrow = !CalucNextTeam();
                //if (CalucNextTeam())
                //{
                //    //次に投げるチームのボールのスプライトを変更
                //    m_NextBallImage.GetComponent<ChangeBallSprite>().ChangeSprite(m_NextTeam);
                //    //エンドがまだ終わっていないとき
                //    if (m_GameFlowScript.GetIsEnd() == false)
                //    {
                //        //パドルスクリプトを取得
                //        PaddleScript paddle = GameObject.Find("Paddle").GetComponent<PaddleScript>();
                //        //パドルのスプライトを変更
                //        paddle.SetTeam(m_NextTeam);
                //        paddle.PaddleStart();
                //    }
                //    //投げ終わり
                //    IsThrow = false;
                //    //カメラ変更
                //    GameObject.Find("GameCamera").GetComponent<GameCameraScript>().SwitchCamera();
                //    m_Frame = 0;

                //}
            }
        }
    }

    public Team GetNowTeam()
    {
        return m_NextTeam;
    }

    /// <summary>
    /// 次に投げるチームを計算する
    /// </summary>
    /// <returns>戻り値は計算ができたかどうか</returns>
    public bool CalucNextTeam()
    {
        //ballというタグのついたゲームオブジェクトを配列に入れる
        GameObject[] m_balls;
        m_balls = GameObject.FindGameObjectsWithTag("Ball");
        if (m_balls.Length == 0)
        {
            if (m_RemainBalls.x == 0 && m_RemainBalls.y == 0)
            {
                //ボールがなくなったので次に投げるチームを計算しない
                return false;
            }

            if (m_RemainBalls.x == m_RemainBalls.y)
            {
                m_NextTeam = m_firstTeam;
            }
            else
            {
                ChangeNextTeam();
            }
            NextTeamLog();
            return true;
        }

        float[] m_BallsDist;
        //ボールの長さ分の配列を確保
        m_BallsDist = new float[m_balls.Length];
        Team[] Teams;
        Teams = new Team[m_balls.Length];

        //最も近いボールまでの距離
        float[] NearDist;
        //赤チームと青チームの最も近いボールの距離を保存しておくので配列のサイズは2
        NearDist = new float[2];
        NearDist[0] = 10000;
        NearDist[1] = 10000;

        for (int ballnum = 0; ballnum < m_balls.Length;ballnum++)
        {
            //チームを取得
            Teams[ballnum] = m_balls[ballnum].GetComponent<TeamDivisionScript>().GetTeam();
            //ボールからジャックボールへの距離
            Vector3 Dir = m_balls[ballnum].GetComponent<Rigidbody>().position - m_JackPos;
            float dist = Dir.magnitude;
            //ジャックボールまでの距離を代入
            m_BallsDist[ballnum] = dist;

            if (Teams[ballnum] == Team.Red)
            {
                //距離がより近いものを設定する
                if (m_BallsDist[ballnum] < NearDist[0])
                {
                    NearDist[0] = m_BallsDist[ballnum];
                }
            }
            else
            {
                //距離がより近いものを設定する
                if (m_BallsDist[ballnum] < NearDist[1])
                {
                    NearDist[1] = m_BallsDist[ballnum];
                }
            }


        }

        if (NearDist[0] < NearDist[1])
        {
            m_NextTeam = Team.Blue;
        }
        else
        {
            m_NextTeam = Team.Red;
        }

        float SecondNearDist = Mathf.Max(NearDist[0], NearDist[1]);
        for (int ballnum = 0; ballnum < m_balls.Length; ballnum++)
        {
            m_balls[ballnum].gameObject.layer = 9;
            if (m_BallsDist[ballnum] < SecondNearDist)
            {
                //もう片方の一番近いボールよりも近い位置にあるのでレイヤーを変える
                m_balls[ballnum].gameObject.layer = 0;
            }
        }

        if (m_RemainBalls.x == 0 && m_RemainBalls.y == 0)
        {
            //両方投げ終わっているのでゲームエンド
            m_GameFlowScript.GameEnd();
            m_NextBallImage.GetComponent<ChangeBallSprite>().GameEnd();
        }
        else if(m_RemainBalls.x == 0)
        {
            //赤チームが投げ終わっているので次は青チーム
            m_NextTeam = Team.Blue;
        }
        else if(m_RemainBalls.y == 0)
        {
            //青チームが投げ終わっているので次は赤チーム
            m_NextTeam = Team.Red;
        }

        NextTeamLog();
        return true;
    }

    /// <summary>
    /// 全てのボールが止まっているかどうか判定
    /// </summary>
    public bool IsStopAllBalls()
    {
        //ジャックボールを取得
        m_Jack = m_BallFlow.GetJackBall();
        //ジャックボールのステートを取得
        m_JackState = m_Jack.GetComponent<BallStateScript>().GetState();
        switch (m_JackState)
        {
            case BallState.Move:
                //ジャックボールが停止していない
                m_IsMoving = true;
                return !m_IsMoving;
            case BallState.Stop:
                //m_Jack.GetComponent<BallOperateScript>().EndThrowing();
                break;
            default:
                //ジャックボールが停止していない
                m_IsMoving = true;
                return !m_IsMoving;
        }

        //ボールを取得
        GameObject[] m_balls;
        m_balls = GameObject.FindGameObjectsWithTag("Ball");
        //ボールを取得できた
        if (m_balls.Length != 0)
        {
            //一つ以上ボールがある
            for (int num = 0; num < m_balls.Length; num++)
            {
                //ボールの状態を取得
                if (m_balls[num].GetComponent<BallStateScript>().GetState() == BallState.Stop)
                {
                    //m_balls[num].GetComponent<BallOperateScript>().EndThrowing();
                }
                else
                {
                    //止まっていないボールがある
                    m_IsMoving = true;
                    return !m_IsMoving;
                }
            }
        }

        //全てのボールが止まっている
        m_IsMoving = false;
        m_state = TeamFlowState.Stop;

        return !m_IsMoving;
    }

    /// <summary>
    /// ボールを投げ終わる処理を走らせる
    /// </summary>
    public void ThrowEnd()
    {
        //投げ終わり
        m_Jack.GetComponent<BallOperateScript>().EndThrowing();
        //ボールを取得
        GameObject[] m_balls;
        m_balls = GameObject.FindGameObjectsWithTag("Ball");
        //ボールを取得できた
        if (m_balls.Length != 0)
        {
            for (int num = 0; num < m_balls.Length; num++)
            {
                m_balls[num].GetComponent<BallOperateScript>().EndThrowing();
            }
        }
    }
    /// <summary>
    /// ジャックボールの位置を保存
    /// </summary>
    /// <param name="pos">位置</param>
    public void SetJackPos(Vector3 pos)
    {
        m_JackPos = pos;
    }

    /// <summary>
    /// 残りの球数を減らす
    /// </summary>
    public void DecreaseBalls()
    {
        switch (m_NextTeam)
        {
            case Team.Red:
                //次のチームが赤の場合xを減らす
                m_RemainBalls.x--;
                break;

            case Team.Blue:
                //次のチームが青の場合yを減らす
                m_RemainBalls.y--;
                break;

            default:
                break;
        }
    }

    /// <summary>
    /// ボールが動いているかを取得
    /// </summary>
    /// <returns></returns>
    public bool GetIsMoving()
    {
        return m_IsMoving;
    }

    /// <summary>
    /// 次のチームを変更
    /// </summary>
    public void ChangeNextTeam()
    {
        if(m_NextTeam == Team.Red)
        {
            m_NextTeam = Team.Blue;
        }
        else
        {
            m_NextTeam = Team.Red;
        }
    }

    public void ChangeFirstTeam()
    {
        if (m_firstTeam == Team.Red)
        {
            m_firstTeam = Team.Blue;
        }
        else
        {
            m_firstTeam = Team.Red;
        }
    }
    /// <summary>
    /// 次のチームのログ
    /// </summary>
    public void NextTeamLog()
    {
        //エンドが終わっていないとき
        if (m_GameFlowScript.GetIsEnd() == false)
        {
            //次に投げるチームによって処理を変える
            switch (m_NextTeam)
            {
                case Team.Red:
                    Debug.Log("次に赤チームが投げます");
                    Debug.Log("残り" + m_RemainBalls.x + "球です");
                    break;

                case Team.Blue:
                    Debug.Log("次に青チームが投げます");
                    Debug.Log("残り" + m_RemainBalls.y + "球です");
                    break;

                case Team.Num:
                    Debug.Log("次にボールを投げるチームが決まっていません");
                    break;
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public void ResetVar()
    {
        //初めに赤チームが投げる
        m_NextTeam = m_firstTeam;
        m_IsMoving = false;
        m_NextBallImage.GetComponent<ChangeBallSprite>().ResetVar();
        //ボールの数を初期化
        m_RemainBalls = Vector2Int.one;
        m_RemainBalls *= m_Remain;
        //一番初めなのでジャックプリーズと出す
        GameObject.Find("JackPlease").GetComponent<JackPleaseScript>().StartSlide();

        //ログを流す
        NextTeamLog();
    }
    /// <summary>
    /// 投げ始めるチームを設定する。
    /// </summary>
    public void SetFirstTeam(Team firstTeam)
    {
        m_firstTeam = firstTeam;
    }

    public void ThrowBall()
    {
        IsThrow = true;
        //ステートを投げた状態にする
        m_state = TeamFlowState.Throw;
    }

    /// <summary>
    /// ジャックボールを投げるチームを変更する
    /// </summary>
    public void ChangeJackThrowTeam()
    {
        //ジャックボールを投げるチームが変わったのでジャックプリーズと出す
        GameObject.Find("JackPlease").GetComponent<JackPleaseScript>().StartSlide();
        //ジャックボールが準備されていないとき
        ChangeNextTeam();
        ChangeFirstTeam();
        //投げ終わり
        IsThrow = false;
        //カメラ変更
        GameObject.Find("GameCamera").GetComponent<GameCameraScript>().SwitchCamera();
        m_Frame = 0;
        m_IsMoving = false;
        NextTeamLog();
    }

    /// <summary>
    /// 次に投げるチームの残りの球数
    /// </summary>
    /// <returns>残りの球数</returns>
    public int GetRemainBall()
    {
        if(m_NextTeam == Team.Red)
        {
            return m_RemainBalls.x;
        }
        else
        {
            return m_RemainBalls.y;
        }
    }

    public TeamFlowState GetState()
    {
        return m_state;
    }

    public void SetState(TeamFlowState state)
    {
        m_state = state;
    }

    /// <summary>
    /// 次に投げるチームを情報が必要なクラスにセットする
    /// </summary>
    public void SetNextTeam()
    {
        //エンドがまだ終わっていないとき
        if (m_GameFlowScript.GetIsEnd() == false)
        {
            //次に投げるチームのボールのスプライトを変更
            m_NextBallImage.GetComponent<ChangeBallSprite>().ChangeSprite(m_NextTeam);
            //パドルスクリプトを取得
            PaddleScript paddle = GameObject.Find("Paddle").GetComponent<PaddleScript>();
            //パドルのスプライトを変更
            paddle.SetTeam(m_NextTeam);
            //パドルの操作を始める
            paddle.PaddleStart();
        }

    }
}
