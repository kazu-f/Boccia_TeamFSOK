using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallFlowScript : MonoBehaviour
{
    private bool m_IsPreparedJack = false;
    private GameObject m_Jack = null;

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
}
