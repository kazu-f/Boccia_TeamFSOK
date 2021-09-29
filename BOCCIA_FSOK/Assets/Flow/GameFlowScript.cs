using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct VictoryTeam
{
    private Team NearestTeam;       //最も近いチーム
    private int Score;      //点数

    public void SetNearestTeam(Team team)
    {
        NearestTeam = team;
    }

    public Team GetNearestTeam()
    {
        return NearestTeam;
    }

    public void SetScore(int score)
    {
        Score = score;
    }

    public int GetScore()
    {
        return Score;
    }
};

public class GameFlowScript : MonoBehaviour
{
    private bool m_IsEnd = false;       //エンドが終了したかどうか
    private VictoryTeam m_VicTeam; 

    BallFlowScript ballFlow;
    // Start is called before the first frame update
    void Start()
    {
        ballFlow = this.gameObject.GetComponent<BallFlowScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (m_IsEnd)
        {
            CalcScore();
        }
    }

    /// <summary>
    /// ゲーム終了のフラグを立てる
    /// </summary>
    public void GameEnd()
    {
        m_IsEnd = true;
    }

    /// <summary>
    /// 得点を計算する
    /// </summary>
    private void CalcScore()
    {
        GameObject m_Jack = null;
        m_Jack = ballFlow.GetJackBall();
        if (m_Jack == null)
        {
            //インスタンスの取得に失敗したとき
            Debug.Log("<color=red>エラー：ジャックボールの取得に失敗しました</color>");
        }
        Vector3 JackBallPos = m_Jack.GetComponent<Rigidbody>().position;
        //ballというタグのついたゲームオブジェクトを配列に入れる
        GameObject[] m_balls;
        m_balls = GameObject.FindGameObjectsWithTag("Ball");
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

        for (int ballnum = 0; ballnum < m_balls.Length; ballnum++)
        {
            //チームを取得
            Teams[ballnum] = m_balls[ballnum].GetComponent<TeamDivisionScript>().GetTeam();
            //ボールの位置を取得
            Vector3 BallPos = m_balls[ballnum].GetComponent<Rigidbody>().position;
            //ジャックボールまでの距離を計算
            Vector3 Dist = JackBallPos - BallPos;
            m_BallsDist[ballnum] = Dist.magnitude;

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
            m_VicTeam.SetNearestTeam(Team.Red);
        }
        else
        {
            m_VicTeam.SetNearestTeam(Team.Blue);
        }

        float SecondNearDist = Mathf.Max(NearDist[0], NearDist[1]);
        //スコア
        int score = 0;
        for (int ballnum = 0; ballnum < m_balls.Length; ballnum++)
        {
            if (m_BallsDist[ballnum] < SecondNearDist)
            {
                //もう片方の一番近いボールよりも近い位置にあるのでスコアを上げる
                score++;
            }
        }

        if (m_VicTeam.GetNearestTeam() == Team.Red)
        {
            Debug.Log("赤チーム" + m_VicTeam.GetScore());
        }
        else if (m_VicTeam.GetNearestTeam() == Team.Blue)
        {
            Debug.Log("青チーム" + m_VicTeam.GetScore());
        }
    }

    public VictoryTeam GetVictoryTeam()
    {
        return m_VicTeam;
    }
}
