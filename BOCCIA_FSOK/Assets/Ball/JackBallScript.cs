using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JackBallScript : MonoBehaviour
{
    //ジャックボールの戻るべき位置
    private Vector3 m_OriginPos = new Vector3(0.0f, 0.0f, 0.0f);
    //ジャックボールの位置
    private Vector3 m_pos = Vector3.zero;

    //エリア内かどうか判定するためのオブジェクト
    private GameObject m_CourtArea;
    //エリア内にいるかどうかのフラグ
    private bool m_InArea = true;

    private BallState m_state = BallState.Num;
    private Rigidbody m_rigidbody;
    private Vector3 m_moveSpeed;
    private BallStateScript m_BallStateScript;
    private GameObject m_GameFlow = null;
    private TeamFlowScript m_TeamFlow = null;
    // Start is called before the first frame update
    void Start()
    {
        m_BallStateScript = GetComponent<BallStateScript>();
        //RigidBodyを取得
        m_rigidbody = GetComponent<Rigidbody>();
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
        //各種状態を取得
        m_state = m_BallStateScript.GetState();
        m_moveSpeed = m_BallStateScript.GetMoveSpeed();

        //オブジェクトのtransformを取得
        Transform myTrans = this.transform;

        //transformから座標を取得
        m_pos = myTrans.position;

        //ジャックボールの状態をTeamFlowに保存
        m_TeamFlow.SetJackState(m_state);

        switch (m_state)
        {
            case BallState.Move:
                if(m_InArea == false)
                {
                    //範囲外のため急激に止める
                    m_rigidbody.velocity += m_moveSpeed * -0.5f * Time.deltaTime;
                }
                break;

            case BallState.Stop:
                //エリア外にいるとき
                if (m_InArea == false)
                {
                    //クロスに戻す
                    m_pos = m_OriginPos;

                    m_InArea = true;
                }
                //速度を0にセット
                m_rigidbody.velocity = Vector3.zero;

                //TeamFlowにジャックボールの位置を保存
                m_TeamFlow.SetJackPos(m_pos);

                break;
        }

        //座標を設定
        myTrans.position = m_pos;

    }

    /// <summary>
    /// 得点計算
    /// </summary>
    private void CalcScore()
    {

    }

    private void OnTriggerExit(Collider other)
    {
        m_InArea = false;
    }
}
