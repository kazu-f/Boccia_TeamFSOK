using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallFlowScript : MonoBehaviour
{
    //シーン上にジャックボールがあるかどうか
    private bool m_IsPreparedJack = false;
    public GameObject JackPrefab;
    private GameObject m_Jack = null;
    private BallStateScript m_JackState = null;
    private BallOperateScript m_BallOperate = null;
    // Start is called before the first frame update
    void Start()
    {
        m_Jack = Instantiate(JackPrefab);
        m_Jack.SetActive(false);
        m_BallOperate = m_Jack.GetComponent<BallOperateScript>();

    }

    // Update is called once per frame
    void Update()
    {
        if (m_Jack != null)
        {
            //ステートを取得
            m_JackState = m_Jack.GetComponent<BallStateScript>();
            if(m_JackState.GetState() == BallState.Stop && m_BallOperate.GetIsThrow())
            {
                //ジャックボールが準備されたときにフラグを立てる
                m_IsPreparedJack = true;
            }
        }
    }

    //シーン上にジャックボールがあるかどうか
    public bool IsPreparedJack()
    {
        return m_IsPreparedJack;
    }

    public GameObject GetJackBall()
    {
        return m_Jack;
    }
}
