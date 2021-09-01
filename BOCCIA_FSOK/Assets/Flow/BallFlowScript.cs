using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallFlowScript : MonoBehaviour
{
    //シーン上にジャックボールがあるかどうか
    private bool m_IsPreparedJack = false;
    private GameObject m_Jack = null;
    private BallStateScript m_JackState;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (m_IsPreparedJack == false)
        {
            //ジャックボールが準備されていないときジャックボールを検索
            m_Jack = GameObject.FindGameObjectWithTag("Jack");
            if (m_Jack != null)
            {
                //ステートを取得
                m_JackState = m_Jack.GetComponent<BallStateScript>();
                //ジャックボールが準備されたときにフラグを立てる
                m_IsPreparedJack = true;
            }
        }
        //else
        //{
        //    //ジャックボールが停止しているとき
        //    if (m_JackState.GetState() == BallState.Stop)
        //    {

        //    }
        //}
    }

    //シーン上にジャックボールがあるかどうか
    public bool IsPreparedJack()
    {
        return m_IsPreparedJack;
    }
}
