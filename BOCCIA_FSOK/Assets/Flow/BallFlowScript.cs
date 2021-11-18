using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallFlowScript : Photon.Pun.MonoBehaviourPun
{
    //シーン上にジャックボールがあるかどうか
    private bool m_IsPreparedJack = false;
    public GameObject JackPrefab;
    private GameObject m_Jack = null;
    private BallStateScript m_JackState = null;
    private BallOperateScript m_BallOperate = null;

    private const float LOG_TIME = 10.0f;
    private float m_currentTime = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        if(photonView.IsMine)
        {
            photonView.RPC(nameof(CreateJackBallRPC), Photon.Pun.RpcTarget.AllBuffered, Photon.Pun.PhotonNetwork.AllocateViewID(true));
        }
        if(m_Jack == null)
        {
            Debug.LogError("JackBallはnullです");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (m_IsPreparedJack == false)
        {
            if (m_Jack != null)
            {
                //ステートを取得
                m_JackState = m_Jack.GetComponent<BallStateScript>();
                if (m_JackState.GetState() == BallState.Stop && m_BallOperate.GetInArea())
                {
                    //ジャックボールが準備されたときにフラグを立てる
                    m_IsPreparedJack = true;
                }
            }
        }

        m_currentTime -= Time.deltaTime;

        if(m_currentTime < 0.0f)
        {
            if (m_Jack == null)
            {
                Debug.LogError("JackBallは存在していない。");
            }
            else
            {
                Debug.Log("JackBallは存在している。");
            }
            m_currentTime = LOG_TIME;
        }
    }

    //シーン上にジャックボールがあるかどうか
    public bool IsPreparedJack()
    {
        return m_IsPreparedJack;
    }

    public GameObject GetJackBall()
    {
        if (m_Jack == null)
        {
            Debug.LogError("GetJackBall() == null");
        }
        else
        {
            Debug.Log("JackBallは存在している。");
        }
        return m_Jack;
    }

    public void ResetVar()
    {
        var ballOperate = m_Jack.GetComponent<BallOperateScript>();
        ballOperate.ResetVar();

        m_IsPreparedJack = false;
        m_Jack.SetActive(false);
    }

    [Photon.Pun.PunRPC]
    public void CreateJackBallRPC(int viewID)
    {
        m_Jack = Instantiate(JackPrefab);
        m_Jack.SetActive(false);
        m_BallOperate = m_Jack.GetComponent<BallOperateScript>();
        //PhotonViewの取得。
        var photonV = m_Jack.GetComponent<Photon.Pun.PhotonView>();
        photonV.ViewID = viewID;
        photonV.ObservedComponents = new List<Component>();
        var photonTransformView = m_Jack.gameObject.GetComponent<Photon.Pun.PhotonTransformView>();
        photonTransformView.m_SynchronizePosition = true;
        photonTransformView.m_SynchronizeRotation = true;
        var photonRigidbodyView = m_Jack.gameObject.GetComponent<Photon.Pun.PhotonRigidbodyView>();
        photonV.ObservedComponents.Add(photonTransformView);
        photonV.ObservedComponents.Add(photonRigidbodyView);

        if (m_Jack != null)
        {
            Debug.Log("JackBallを作成。");
        }
        else
        {
            Debug.Log("JackBallの作成が失敗。");
        }
    }
}
