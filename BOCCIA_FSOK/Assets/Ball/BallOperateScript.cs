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
    private Vector3 m_pos = Vector3.zero;
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
    }
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    /// <summary>
    /// リジッドボディに速度を加算
    /// </summary>
    /// <param name="speed">加算する速度</param>
    public void AddForce(Vector3 speed)
    {
        if (m_Team.GetTeam() != Team.Jack)
        {
            m_TeamFlow.DecreaseBalls();
        }
        //速度を加算
        m_rigidbody.AddForce(speed);
        //ボールを投げた判定をTeamFlowに送る
        m_TeamFlow.ThrowBall();
        Debug.Log("ボールが動いています");
    }

    /// <summary>
    /// 場外にボールが行った時の処理
    /// </summary>
    public void OutsideVenue()
    {
        //オブジェクトのtransformを取得
        Transform myTrans = this.transform;
        //transformから座標を取得
        m_pos = myTrans.position;

        //ボールを停止する
        m_StateScript.Stop();

        if (m_Team.GetTeam() == Team.Jack)
        {
            //ジャックボールの場合
            //クロスに戻す
            m_pos = DefaultPos;
            //TeamFlowにジャックボールの位置を保存
            m_TeamFlow.SetJackPos(m_pos);

            //座標を設定
            myTrans.position = m_pos;
        }
        else
        {
            //ジャックボール以外の場合
            //ballというタグのついたゲームオブジェクトを配列に入れる
            GameObject[] m_balls;
            m_balls = GameObject.FindGameObjectsWithTag("Ball");
            if (m_balls.Length == 0)
            {
                m_TeamFlow.ChangeNextTeam();
            }
            else
            {
                //他にもボールがあるので次に投げるチームを計算する
                m_TeamFlow.CalucNextTeam();
            }
        }
        m_TeamFlow.NextTeamLog();
        if (m_Team.GetTeam() != Team.Jack)
        {
            this.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// 変数のリセット。
    /// </summary>
    public void ResetVar()
    {
        m_StateScript.ResetState();
    }
}
