using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallOperateScript : MonoBehaviour
{
    private BallStateScript m_StateScript = null;
    private Rigidbody m_rigidbody = null;
    private bool m_IsCalculated = false;
    private GameObject m_Flow = null;       //ゲームの流れ全体をコントロールするオブジェクト
    private TeamFlowScript m_TeamFlow = null;
    // Start is called before the first frame update
    void Start()
    {
        //オブジェクトを取得
        m_Flow = GameObject.Find("GameFlow");
        if(m_Flow == null)
        {
            //インスタンスが作成されていないとき
            Debug.LogError("エラー：GameFlowのインスタンスが作成されていません。");
        }
        else
        {
            //次に投げるチームを決めるコンポーネントを取得
            m_TeamFlow = m_Flow.GetComponent<TeamFlowScript>();
            if (m_TeamFlow == null)
            {
                //TeamFlowScriptコンポーネントが取得できなかったとき
                Debug.LogError("エラー：TeamFlowScriptコンポーネントの取得に失敗しました。");
            }
        }

        //ボールの状態を操作するスクリプトを取得
        m_StateScript = GetComponent<BallStateScript>();
        if(m_StateScript == null)
        {
            //BallStateScriptコンポーネントが取得できなかったとき
            Debug.LogError("エラー：BallStateScriptコンポーネントの取得に失敗しました。");
        }
        //RigidBodyを取得
        m_rigidbody = GetComponent<Rigidbody>();
        if(m_rigidbody == null)
        {
            //Rigidbodyコンポーネントが取得できなかったとき
            Debug.LogError("エラー：Rigidbodyコンポーネントの取得に失敗しました。");
        }

        //AddForce(new Vector3(10.0f,0.0f,0.0f));
        m_TeamFlow.DecreaseBalls();
    }

    // Update is called once per frame
    void Update()
    {
        if(m_IsCalculated == false)
        {
            if(m_StateScript.GetState() == BallState.Stop)
            {
                m_IsCalculated = m_TeamFlow.CalucNextTeam();
            }
        }
    }

    /// <summary>
    /// リジッドボディに速度を加算
    /// </summary>
    /// <param name="speed">加算する速度</param>
    void AddForce(Vector3 speed)
    {
        //速度を加算
        m_rigidbody.AddForce(speed);
        m_TeamFlow.SetMove();
    }
}
