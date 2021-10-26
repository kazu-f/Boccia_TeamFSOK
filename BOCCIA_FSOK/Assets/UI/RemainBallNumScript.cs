using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RemainBallNumScript : MonoBehaviour
{
    private GameObject m_GameFlow = null;
    private Text m_Text = null;
    private void Awake()
    {
        m_GameFlow = GameObject.Find("GameFlow");
        if(m_GameFlow == null)
        {
            //ゲームフローオブジェクトの取得に失敗
            Debug.LogError("GameFlowオブジェクトの取得に失敗しました。");
        }
        m_Text = this.gameObject.GetComponent<Text>();
        if(m_Text == null)
        {
            //Textコンポーネントの取得に失敗
            Debug.LogError("Textコンポーネントの取得に失敗しました。");
        }
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(m_GameFlow.GetComponent<BallFlowScript>().IsPreparedJack())
        {
            Color color = new Color(0.0f, 0.0f, 0.0f, 1.0f);
            m_Text.color = color;
        }
        else
        {
            Color color = new Color(0.0f, 0.0f, 0.0f, 0.0f);
            m_Text.color = color;
        }
        m_Text.text = ("x" + m_GameFlow.GetComponent<TeamFlowScript>().GetRemainBall());
    }
}
