using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JackBallScript : IBallScript
{
    private GameObject m_GameFlow = null;
    private TeamFlowScript m_TeamFlow = null;
    public Vector3 m_DefaultPos = Vector3.zero;
    private Rigidbody m_rigidbody = null;
    private Photon.Pun.PhotonView photonView;
    private void Awake()
    {
        //RigidBodyを取得
        m_rigidbody = GetComponent<Rigidbody>();
        if(m_rigidbody == null)
        {
            Debug.LogError("RigidBodyコンポーネントの取得に失敗しました");
        }
        m_GameFlow = GameObject.Find("GameFlow");
        if (m_GameFlow == null)
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
        //PhotonViewを取得する。
        photonView = this.gameObject.GetComponent<Photon.Pun.PhotonView>();
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
    /// ボールがエリア内に入った時の処理
    /// </summary>
    public override void InsideVenue()
    {
        InArea = true;
    }

    /// <summary>
    /// ボールがエリア外に出た時の処理
    /// </summary>
    public override void OutsideVenue()
    {
        InArea = false;
    }

    private void ResetPos()
    {
        //ボールの権限が無ければ無視。
        if (!photonView.IsMine || m_rigidbody.isKinematic) return;
        //速度をゼロにする
        m_rigidbody.velocity = Vector3.zero;
        //クロスに戻す
        m_rigidbody.position = m_DefaultPos;
    }

    /// <summary>
    /// ボールが停止したときの処理
    /// </summary>
    public override void EndThrow()
    {
        IsThrowing = false;
        if (InArea == false)
        {
            if (m_GameFlow.GetComponent<BallFlowScript>().IsPreparedJack())
            {
                ResetPos();
            }
            else
            {
                this.gameObject.GetComponent<BallStateScript>().ResetState();
                this.gameObject.SetActive(false);
            }
        }
    }
}
