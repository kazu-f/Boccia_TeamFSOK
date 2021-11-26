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
        //RigidBody���擾
        m_rigidbody = this.gameObject.GetComponent<Rigidbody>();
        if (m_rigidbody == null)
        {
            Debug.LogError("���W�b�h�{�f�B�擾�ł��Ă܂���!!!");
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
        // �I�[�i�[�̏ꍇ
        if (stream.IsWriting)
        {
            stream.SendNext(this.m_state);
        }
        // �I�[�i�[�ȊO�̏ꍇ
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
                Debug.LogError("OwnershipTransfer��Request�ɕύX���Ă��������B");
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
        //�ړ����x���擾
        m_moveSpeed = m_rigidbody.velocity;
        if (m_moveSpeed.magnitude <= m_borderSpeed)
        {
            //���x�����ȉ��̎�
            //�ړ����~����
            m_rigidbody.velocity = Vector3.zero;
            //��~���ɂ���
            m_state = BallState.Stop;
        }
        else
        {
            //�ړ����ɂ���
            m_state = BallState.Move;
        }
    }

    public BallState GetState()
    {
        return m_state;
    }
    
    /// <summary>
    /// �{�[�����~�߂鏈��
    /// </summary>
    public void Stop()
    {
        //�ړ����~����
        m_rigidbody.velocity = Vector3.zero;
        //��~���ɂ���
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