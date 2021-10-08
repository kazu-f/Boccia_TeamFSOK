using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DispScoreScript : MonoBehaviour
{
    public Text m_ScoreResult;

    private void Awake()
    {

    }
    // Start is called before the first frame update
    void Start()
    {
        m_ScoreResult.color = new Color(1.0f, 0.0f, 0.0f, 0.0f);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void DispResult(int score)
    {
        m_ScoreResult.text = score + "点!";
    }

    /// <summary>
    /// テキストをチームによって変更する
    /// </summary>
    /// <param name="team">得点したチーム</param>
    public void SetTextTeam(Team team)
    {
        //得点したチームによって色を変更
        switch(team)
        {
            case Team.Red:
                //赤に変える
                m_ScoreResult.color = Color.red;
                //m_ScoreResult.text = "赤チームが";
                break;
            case Team.Blue:
                //青に変える
                m_ScoreResult.color = Color.blue;
                //m_ScoreResult.text = "青チームが";
                break;
            default:
                return;
        }
    }

    //リセット関数
    public void ResetVar()
    {
        m_ScoreResult.color = Vector4.zero;
    }
}
