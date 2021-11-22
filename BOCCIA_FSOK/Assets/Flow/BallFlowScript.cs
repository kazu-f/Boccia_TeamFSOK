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
        return m_Jack;
    }

    public void ResetVar()
    {
        var ballOperate = m_Jack.GetComponent<BallOperateScript>();
        ballOperate.ResetVar();

        m_IsPreparedJack = false;
        m_Jack.SetActive(false);
    }

    /// <summary>
    /// ボールの権限リクエスト等を行う。
    /// </summary>
    /// <param name="isRequest">リクエストする側かどうか。</param>
    public void RequestOwnerShip(bool isRequest)
    {
        if(isRequest)
        {
            var photonV = m_Jack.GetComponent<Photon.Pun.PhotonView>();
            if (photonV != null)
            {
                photonV.RequestOwnership();
            }
        }
        //リジッドボディ取得。
        var RB = m_Jack.GetComponent<Rigidbody>();
        if(RB != null)
        {
            //オーナーかどうかで物理演算させるかを切り替える。
            RB.isKinematic = !isRequest;
            if (isRequest)
            {
                Debug.Log("ボールの物理挙動開始。");
            }
            else
            {
                Debug.Log("ボールの物理挙動停止。");
            }
        }
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
        var photonRigidBodyView = m_Jack.gameObject.GetComponent<Photon.Pun.PhotonRigidbodyView>();
        photonTransformView.m_SynchronizePosition = true;
        photonTransformView.m_SynchronizeRotation = true;
        photonRigidBodyView.m_SynchronizeVelocity = true;

        photonV.ObservedComponents.Add(photonRigidBodyView);
        photonV.ObservedComponents.Add(photonTransformView);

        photonV.OwnershipTransfer = Photon.Pun.OwnershipOption.Request;

        if (m_Jack != null)
        {
            Debug.Log("JackBallを作成。");
        }
        else
        {
            Debug.Log("JackBallの作成が失敗。");
        }

        //var RB = m_Jack.GetComponent<Rigidbody>();
        //if (photonV.IsMine)
        //{
        //    RB.isKinematic = false;
        //}
        //else
        //{
        //    RB.isKinematic = true;
        //}
    }
}
