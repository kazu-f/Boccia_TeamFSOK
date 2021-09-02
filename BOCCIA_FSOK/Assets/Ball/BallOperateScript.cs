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
            Debug.Log("<color=red>エラー：GameFlowのインスタンスが作成されていません。</color>");
        }
        else
        {
            //次に投げるチームを決めるコンポーネントを取得
            m_Flow.GetComponent<TeamFlowScript>();
        }
        if(m_Flow == null)
        {
            //TeamFlowScriptコンポーネントが取得できなかったとき
            Debug.Log("<color=red>エラー：TeamFlowScriptコンポーネントの取得に失敗しました。</color>");
        }

        //ボールの状態を操作するスクリプトを取得
        m_StateScript = GetComponent<BallStateScript>();
        if(m_StateScript == null)
        {
            //BallStateScriptコンポーネントが取得できなかったとき
            Debug.Log("<color=red>エラー：BallStateScriptコンポーネントの取得に失敗しました。</color>");
        }
        //RigidBodyを取得
        m_rigidbody = GetComponent<Rigidbody>();
        if(m_rigidbody == null)
        {
            //Rigidbodyコンポーネントが取得できなかったとき
            Debug.Log("<color=red>エラー：Rigidbodyコンポーネントの取得に失敗しました。</color>");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(m_IsCalculated == false)
        {
            if(m_StateScript.GetState() == BallState.Stop)
            {

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
    }
}
