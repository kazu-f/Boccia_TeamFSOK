using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamFlowScript : MonoBehaviour
{
    public Team m_NextTeam = Team.Num;
    private BallFlowScript m_BallFlow = null;
    private GameObject m_Jack = null;
    private BallState m_JackState = BallState.Num;
    private Vector3 m_JackPos = Vector3.zero;
    private Vector2Int m_RemainBalls = Vector2Int.one;
    private int m_Remain = 6;
    private GameFlowScript m_GameFlowScript = null;
    // Start is called before the first frame update
    void Start()
    {
        m_NextTeam = Team.Jack;
        m_BallFlow = GetComponent<BallFlowScript>();
        m_RemainBalls *= m_Remain;
        m_GameFlowScript = GetComponent<GameFlowScript>();
    }

    // Update is called once per frame
    void Update()
    {
        //次に投げるチームによって処理を変える
        switch(m_NextTeam)
        {
            //ジャックボールを投げるとき
            case Team.Jack:
                Debug.Log("初めにジャックボールを投げます");
                if (m_BallFlow.IsPreparedJack())
                {
                    //ジャックボールを取得
                    m_Jack = GameObject.Find("JackBall");
                    if (m_Jack == null)
                    {
                        //インスタンスの取得に失敗したとき
                        Debug.Log("<color=red>エラー：ジャックボールの取得に失敗しました</color>");
                    }
                    else
                    {
                        //ジャックボールが準備されたら次は赤チームが投げる
                        m_NextTeam = Team.Red;
                    }
                }
                break;

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
        if(m_JackState == BallState.Move)
        {
            //ジャックボールがまだ動いているので計算は失敗
            m_NextTeam = Team.Num;
            return false;
        }
        //ballというタグのついたゲームオブジェクトを配列に入れる
        GameObject[] m_balls;
        m_balls = GameObject.FindGameObjectsWithTag("Ball");
        for(int ballnum = 0; ballnum < m_balls.Length; ballnum++)
        {
            if(m_balls[ballnum].GetComponent<BallStateScript>().GetState() != BallState.Stop)
            {
                //止まり切っていない球があるため失敗
                m_NextTeam = Team.Num;
                return false;
            }
        }
        //全ての球が止まっていたので計算を続ける
        int NearestBallNum = 0;
        float NearestDist = 10000;
        for(int ballnum = 0; ballnum < m_balls.Length;ballnum++)
        {
            //ボールからジャックボールへの距離
            Vector3 Dir = m_balls[ballnum].GetComponent<Rigidbody>().position - m_JackPos;
            float dist = Dir.magnitude;
            if(NearestDist > dist)
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

        return true;
    }

    /// <summary>
    /// ジャックボールの状態を保存
    /// </summary>
    /// <param name="state">状態</param>
    public void SetJackState(BallState state)
    {
        m_JackState = state;
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
}
