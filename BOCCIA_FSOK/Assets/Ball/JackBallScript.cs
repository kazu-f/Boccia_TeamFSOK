using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum JackBallState
{
    Start,
    Move,
    End,
    Num,
}

public class JackBallScript : MonoBehaviour
{
    private JackBallState m_state = JackBallState.Num;
    //ジャックボールの戻るべき位置
    private Vector3 m_OriginPos = new Vector3(0.0f, 0.0f, 0.0f);
    //ジャックボールの位置
    private Vector3 m_pos = Vector3.zero;

    private Rigidbody m_rigidbody;
    public float m_BorderSpeed = 0.005f;
    //エリア内かどうか判定するためのオブジェクト
    private GameObject m_CourtArea;
    //エリア内にいるかどうかのフラグ
    private bool m_InArea = true;
    // Start is called before the first frame update
    void Start()
    {
        m_state = JackBallState.Start;
        //RigidBodyを取得
        m_rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //移動速度を取得
        Vector3 m_moveSpeed = m_rigidbody.velocity;
        //xz軸の移動のみ見る
        m_moveSpeed.y = 0.0f;
        //速度が一定以上の時
        if (m_moveSpeed.magnitude > m_BorderSpeed)
        {
            //移動中にする
            m_state = JackBallState.Move;
        }
        else
        {
            //速度が一定以下の時
            //移動を停止する
            m_rigidbody.velocity = Vector3.zero;
            //停止中にする
            m_state = JackBallState.End;
        }

        //オブジェクトのtransformを取得
        Transform myTrans = this.transform;

        //transformから座標を取得
        m_pos = myTrans.position;

        switch (m_state)
        {
            case JackBallState.Move:
                if(m_InArea == false)
                {
                    //範囲外のため急激に止める
                    m_rigidbody.velocity += m_moveSpeed * -0.5f * Time.deltaTime;
                }

                break;

            case JackBallState.End:
                //エリア外にいるとき
                if (m_InArea == false)
                {
                    //速度を0にセット
                    m_rigidbody.velocity = Vector3.zero;
                    //クロスに戻す
                    m_pos = m_OriginPos;

                    m_InArea = true;
                }

                break;
        }

        //座標を設定
        myTrans.position = m_pos;

    }


    private void OnTriggerExit(Collider other)
    {
        m_InArea = false;
    }
}
