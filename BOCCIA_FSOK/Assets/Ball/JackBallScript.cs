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
    //境界線
    public float m_CourtHeight;
    public float m_CourtWidth;

    private Rigidbody m_rigidbody;
    private const float m_BorderSpeed = 0.05f;
    // Start is called before the first frame update
    void Start()
    {
        m_state = JackBallState.Start;
        //RigidBodyを取得
        m_rigidbody = GetComponent<Rigidbody>();
        //範囲を半分にする
        m_CourtHeight /= 2.0f;
        m_CourtWidth /= 2.0f;
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
                //オブジェクトが境界線を越えていたら位置をクロスに戻す
                if (m_pos.x > m_CourtWidth || m_pos.x < m_CourtWidth * -1.0f)
                {
                    //範囲外のため急激に止める
                    m_rigidbody.velocity += m_moveSpeed * -0.5f * Time.deltaTime;

                }
                else if (m_pos.z > m_CourtHeight || m_pos.z < m_CourtHeight * -1.0f)
                {
                    //範囲外のため急激に止める
                    m_rigidbody.velocity += m_moveSpeed * -0.5f * Time.deltaTime;
                }

                break;

            case JackBallState.End:
                //オブジェクトが境界線を越えていたら位置をクロスに戻す
                if (m_pos.x > m_CourtWidth || m_pos.x < m_CourtWidth * -1.0f)
                {
                    //クロスに戻す
                    m_pos = m_OriginPos;

                }
                else if (m_pos.z > m_CourtHeight || m_pos.z < m_CourtHeight * -1.0f)
                {
                    //クロスに戻す
                    m_pos = m_OriginPos;
                }

                break;
        }

        //座標を設定
        myTrans.position = m_pos;

    }
}
