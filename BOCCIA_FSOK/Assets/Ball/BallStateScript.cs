using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using Photon.Pun;
using Photon.Realtime;

public enum BallState
{
    Move,
    Stop,
    Num,
}
public class BallStateScript : MonoBehaviourPun, IPunObservable
{
    private BallState m_state = BallState.Num;
    private Rigidbody m_rigidbody;
    private Vector3 m_moveSpeed;
    private float m_borderSpeed = 0.0005f;
    private bool IsPhysicsUpdate = false;

    private void Awake()
    {
        //RigidBodyを取得
        m_rigidbody = this.gameObject.GetComponent<Rigidbody>();
        if (m_rigidbody == null)
        {
            Debug.LogError("リジッドボディ取得できてません!!!");
        }
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (IsPhysicsUpdate && this.photonView.IsMine)
        {
            CalcState();
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        // オーナーの場合
        if (stream.IsWriting)
        {
            stream.SendNext(this.m_state);
        }
        // オーナー以外の場合
        else
        {
            this.m_state = (BallState)stream.ReceiveNext();
        }
    }

    private void RequestOwner()
    {
        if (this.photonView.IsMine == false)
        {
            if (this.photonView.OwnershipTransfer != OwnershipOption.Request)
                Debug.LogError("OwnershipTransferをRequestに変更してください。");
            else
                this.photonView.RequestOwnership();
        }
    }

    public void LateUpdate()
    {
        IsPhysicsUpdate = false;
    }
    public void FixedUpdate()
    {
        IsPhysicsUpdate = true;
    }
    private void CalcState()
    {
        if (m_rigidbody == null)
        {
            m_state = BallState.Num;
            return;
        }
        //移動速度を取得
        m_moveSpeed = m_rigidbody.velocity;
        if (m_moveSpeed.magnitude <= m_borderSpeed)
        {
            //速度が一定以下の時
            //移動を停止する
            m_rigidbody.velocity = Vector3.zero;
            //停止中にする
            m_state = BallState.Stop;
        }
        else
        {
            //移動中にする
            m_state = BallState.Move;
        }
    }

    public BallState GetState()
    {
        return m_state;
    }
    
    /// <summary>
    /// ボールを止める処理
    /// </summary>
    public void Stop()
    {
        //移動を停止する
        m_rigidbody.velocity = Vector3.zero;
        //停止中にする
        m_state = BallState.Stop;
    }

    public void ResetState()
    {
        m_state = BallState.Num;
    }

    public bool GetIsPhysicsUpdate()
    {
        return IsPhysicsUpdate;
    }
}