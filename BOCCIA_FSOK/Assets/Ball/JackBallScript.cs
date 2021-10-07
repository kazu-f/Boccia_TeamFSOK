using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JackBallScript : MonoBehaviour
{
    private GameObject m_GameFlow = null;
    private TeamFlowScript m_TeamFlow = null;

    private void Awake()
    {
        //m_BallStateScript = GetComponent<BallStateScript>();
        //RigidBodyを取得
        //m_rigidbody = GetComponent<Rigidbody>();
    }
    // Start is called before the first frame update
    void Start()
    {
        m_GameFlow = GameObject.Find("GameFlow");
        if(m_GameFlow == null)
        {
            //インスタンスが作成されていないとき
            Debug.LogError("エラー：GameFlowのインスタンスが作成されていません。");
        }
        else
        {
            //TeamFlowScriptコンポーネントを取得
            m_TeamFlow = m_GameFlow.GetComponent<TeamFlowScript>();
            if (m_TeamFlow == null)
            {
                //TeamFlowScriptコンポーネントが取得できなかったとき
                Debug.LogError("エラー：TeamFlowScriptコンポーネントの取得に失敗しました。");
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        //TeamFlowにジャックボールの位置を保存
        m_TeamFlow.SetJackPos(this.transform.position);
    }
}
