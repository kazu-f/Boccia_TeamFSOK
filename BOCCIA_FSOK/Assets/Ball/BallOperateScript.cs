using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallOperateScript : MonoBehaviour
{
    private BallStateScript m_StateScript = null;
    private TeamDivisionScript m_Team = null;
    private Rigidbody m_rigidbody = null;
    private GameObject m_Flow = null;       //ゲームの流れ全体をコントロールするオブジェクト
    private TeamFlowScript m_TeamFlow = null;
    public Vector3 DefaultPos;
    private GameObject m_GameCamera = null;
    private GameCameraScript m_GameCameraScript = null;
    private bool IsThrowing = true;

    private void Awake()
    {
        //RigidBodyを取得
        m_rigidbody = GetComponent<Rigidbody>();
        if (m_rigidbody == null)
        {
            //Rigidbodyコンポーネントが取得できなかったとき
            Debug.LogError("エラー：Rigidbodyコンポーネントの取得に失敗しました。");
        }

        //オブジェクトを取得
        m_Flow = GameObject.Find("GameFlow");
        if (m_Flow == null)
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

        m_Team = GetComponent<TeamDivisionScript>();
        if (m_Team == null)
        {
            //TeamDivisionScriptコンポーネントが取得できなかったとき
            Debug.LogError("エラー：TeamDivisionScriptコンポーネントの取得に失敗しました。");
        }
        //ボールの状態を操作するスクリプトを取得
        m_StateScript = GetComponent<BallStateScript>();
        if (m_StateScript == null)
        {
            //BallStateScriptコンポーネントが取得できなかったとき
            Debug.LogError("エラー：BallStateScriptコンポーネントの取得に失敗しました。");
        }
        m_GameCamera = GameObject.Find("GameCamera");
        if (m_GameCamera == null)
        {
            Debug.LogError("GameCameraの取得に失敗しました");
        }
        else
        {
            m_GameCameraScript = m_GameCamera.GetComponent<GameCameraScript>();
            if(m_GameCameraScript == null)
            {
                Debug.LogError("GameCameraScriptの取得に失敗しました");
            }
        }
    }
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (IsThrowing)
        {
            Vector3 position = this.gameObject.transform.position;
            m_GameCameraScript.SetFollowCameraParam(position);
        }
    }

    /// <summary>
    /// リジッドボディに速度を加算
    /// </summary>
    /// <param name="speed">加算する速度</param>
    public void Throw(Vector3 speed)
    {
        if (m_Team.GetTeam() != Team.Jack)
        {
            m_TeamFlow.DecreaseBalls();
        }
        //速度を加算
        m_rigidbody.AddForce(speed);
        //ボールを投げた判定をTeamFlowに送る
        m_TeamFlow.ThrowBall();
        //カメラを切り替える
        m_GameCameraScript.SwitchCamera();
        Debug.Log("ボールが動いています");
    }

    /// <summary>
    /// 変数のリセット。
    /// </summary>
    public void ResetVar()
    {
        IsThrowing = true;
        m_StateScript.ResetState();
    }

    /// <summary>
    /// 投げ終わった
    /// </summary>
    public void EndThrowing()
    {
        IsThrowing = false;
    }
}
