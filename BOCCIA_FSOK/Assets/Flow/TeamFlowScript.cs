using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamFlowScript : MonoBehaviour
{
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
    private void Awake()
    {
        m_BallFlow = GetComponent<BallFlowScript>();
        m_GameFlowScript = GetComponent<EndFlowScript>();
    }

    // Start is called before the first frame update
    void Start()
    {
        //初めに投げるチームを決定
        m_NextTeam = m_firstTeam;
        //ボールの所持数を増やす
        m_RemainBalls *= m_Remain;
        //ログを流す
        NextTeamLog();
    }

    // Update is called once per frame
    void Update()
    {
        if (IsThrow)
        {
            //1フレーム目だとうまくいかないので少し遅延させる
            if (m_Frame < 10)
            {
                m_Frame++;
                return;
            }
            //ボールを投げた時
            IsStopAllBalls();
            if (m_IsMoving == false)
            {
                //全てのボールが止まっているとき
                IsThrow = !CalucNextTeam();
            }
            m_Frame = 0;
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

        //ここからは一つ以上のボールがあるときの処理
        int NearestBallNum = 0;
        float NearestDist = 10000;
        for(int ballnum = 0; ballnum < m_balls.Length;ballnum++)
        {
            //ボールからジャックボールへの距離
            Vector3 Dir = m_balls[ballnum].GetComponent<Rigidbody>().position - m_JackPos;
            float dist = Dir.magnitude;
            if(dist < NearestDist)
            {
                //今回のボールの方がジャックボールに近いので距離と番号を代入
                NearestDist = dist;
                NearestBallNum = ballnum;
            }
        }

        //次に投げるチームを変更
        if (m_balls[NearestBallNum].GetComponent<TeamDivisionScript>().GetTeam() == Team.Red)
        {
            m_NextTeam = Team.Blue;
        }
        else
        {
            m_NextTeam = Team.Red;
        }

        if(m_RemainBalls.x == 0 && m_RemainBalls.y == 0)
        {
            //両方投げ終わっているのでゲームエンド
            m_GameFlowScript.GameEnd();
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
    private void IsStopAllBalls()
    {
        //ジャックボールを取得
        m_Jack = m_BallFlow.GetJackBall();
        //ジャックボールのステートを取得
        m_JackState = m_Jack.GetComponent<BallStateScript>().GetState();
        if (m_JackState == BallState.Move)
        {
            //ジャックボールが停止していない
            m_IsMoving = true;
            return;
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
                if (m_balls[num].GetComponent<BallStateScript>().GetState() != BallState.Stop)
                {
                    //止まっていないボールがある
                    m_IsMoving = true;
                    return;
                }
            }
        }

        //全てのボールが止まっている
        m_IsMoving = false;
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
        IsStopAllBalls();
        return m_IsMoving;
    }

    ///// <summary>
    ///// ボールが動いているフラグを立てる
    ///// </summary>
    //public void SetMove(bool flag)
    //{
    //    m_IsMoving = flag;
    //}

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
                    break;

                case Team.Blue:
                    Debug.Log("次に青チームが投げます");
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
        //ボールの数を初期化
        m_RemainBalls = Vector2Int.one;
        m_RemainBalls *= m_Remain;
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
    }

    /// <summary>
    /// 場外にボールが行った時の処理
    /// </summary>
    public void OutsideVenue()
    {

    }
}
