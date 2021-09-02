using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamFlowScript : MonoBehaviour
{
    public Team m_team = Team.Num;
    private BallFlowScript m_BallFlow;
    // Start is called before the first frame update
    void Start()
    {
        m_team = Team.Jack;
        m_BallFlow = GetComponent<BallFlowScript>();
    }

    // Update is called once per frame
    void Update()
    {
        //次に投げるチームによって処理を変える
        switch(m_team)
        {
            //ジャックボールを投げるとき
            case Team.Jack:
                if(m_BallFlow.IsPreparedJack())
                {
                    //ジャックボールが準備されたら次は赤チームが投げる
                    m_team = Team.Red;
                }
                break;

            case Team.Red:
                break;

            case Team.Blue:
                break;

            default:
                Debug.Log("エラー：次に投げるチームが決まっていません");
                break;
        }
    }

    public Team GetNowTeam()
    {
        return m_team;
    }
}
