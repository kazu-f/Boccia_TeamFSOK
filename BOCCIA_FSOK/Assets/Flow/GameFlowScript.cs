using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameFlowScript : MonoBehaviour
{
    private bool m_IsEnd = false;
    // Start is called before the first frame update
    void Start()
    {

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
        m_Jack = GameObject.Find("JackBall");
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

        int NearestTeamNum = 0;
        if (NearDist[0] < NearDist[1])
        {
            NearestTeamNum = 0;
        }
        else
        {
            NearestTeamNum = 1;
        }
        float SecondNearDist = Mathf.Max(NearDist[0], NearDist[1]);
        int score = 0;
        for (int ballnum = 0; ballnum < m_balls.Length; ballnum++)
        {
            if (m_BallsDist[ballnum] < SecondNearDist)
            {
                //もう片方の一番近いボールよりも近い位置にあるのでスコアを上げる
                score++;
            }
        }

        if (NearestTeamNum == 0)
        {
            Debug.Log("赤チーム" + score);
        }
        else if(NearestTeamNum == 1)
        {
            Debug.Log("青チーム" + score);
        }
    }
}
