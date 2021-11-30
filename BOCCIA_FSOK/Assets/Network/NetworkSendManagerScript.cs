using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class NetworkSendManagerScript : MonoBehaviourPunCallbacks,IPunObservable,IPunOwnershipCallbacks
{
    bool IsSended = false;
    private Vector2 m_throwPower = Vector2.zero;
    private Vector2 m_throwGaugePosition = Vector2.zero;
    private Vector3 m_playerPos = Vector3.zero;
    private Vector3 m_throwPos = Vector3.zero;
    private Quaternion m_rot = Quaternion.identity;
    private bool[] m_IsTimeUp = new bool[2];
    private int m_state = 0;                         //���肩�ǂ����B
    private PhotonView photonView = null;

    public void Awake()
    {
        photonView = this.gameObject.GetComponent<PhotonView>();
        for(int i = 0;i < m_IsTimeUp.Length;i++)
        {
            m_IsTimeUp[i] = false;
        }
    }

    #region IPunObservable implementation
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            if (!IsSended)
            {
                //�܂��f�[�^�𑗂��Ă��Ȃ��Ƃ�
                //�f�[�^�𑼂̃v���C���[�ɑ���
                stream.SendNext(m_throwPower);
                stream.SendNext(m_throwGaugePosition);
                stream.SendNext(m_playerPos);
                stream.SendNext(m_throwPos);
                stream.SendNext(m_rot);
                stream.SendNext(m_state);
                stream.SendNext(m_IsTimeUp);
                IsSended = true;
            }
        }
        else
        {
            //�f�[�^���󂯎��
            m_throwPower = (Vector2)stream.ReceiveNext();
            m_throwGaugePosition = (Vector2)stream.ReceiveNext();
            m_playerPos = (Vector3)stream.ReceiveNext();
            m_throwPos = (Vector3)stream.ReceiveNext();
            m_rot = (Quaternion)stream.ReceiveNext();
            m_state = (int)stream.ReceiveNext();
            m_IsTimeUp = (bool[])stream.ReceiveNext();
        }
    }

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void RequestOwner()
    {
        if(photonView.IsMine == false)
        {
            //�I�[�i�[����Ȃ��Ƃ�
            if(photonView.OwnershipTransfer != OwnershipOption.Request)
            {
                Debug.LogError("OwnershipTransfer��Request�ɕύX���Ă�������");
            }
            else
            {
                photonView.RequestOwnership();
            }
        }
    }

    //�I�[�i�[�����̐ݒ肪Request�̎��ɌĂ΂��
    public void OnOwnershipRequest(PhotonView targetView, Player requestingPlayer)
    {
        Debug.Log("�I�[�i�[���������N�G�X�g����܂���");
        targetView.TransferOwnership(requestingPlayer);
    }

    //�I�[�i�[�������ڍs�����Ƃ��ɌĂ΂��
    public void OnOwnershipTransfered(PhotonView targetView, Player previousOwner)
    {
        Debug.Log("�I�[�i�[�������ڍs���܂���");
    }

    //�I�[�i�[�����̈ڍs�����s�����Ƃ��ɌĂ΂��
    public void OnOwnershipTransferFailed(PhotonView targetView, Player previousOwner)
    {
        Debug.LogError("�I�[�i�[�����̈ڍs�Ɏ��s���܂���");
    }

    public void SendThrowPow(Vector2 vec2)
    {
        m_throwPower = vec2;
        IsSended = false;
        RequestOwner();
    }

    public void SendThrowGaugePosition(Vector2 vec2)
    {
        m_throwGaugePosition = vec2;
        IsSended = false;
        RequestOwner();
    }

    public void SendPlayerPos(Vector3 vec)
    {
        m_playerPos = vec;
        IsSended = false;
        RequestOwner();
    }

    public void SendThrowPos(Vector3 vec)
    {
        m_throwPos = vec;
        IsSended = false;
        RequestOwner();
    }

    public void SendQuaternion(Quaternion rot)
    {
        m_rot = rot;
        IsSended = false;
        RequestOwner();
    }

    public void SendState(int state)
    {
        m_state = state;
        IsSended = false;
        RequestOwner();
    }

    public void SendMasterIsTimeUp(bool flag)
    {
        m_IsTimeUp[0] = flag;
    }

    public void SendClientIsTimeUp(bool flag)
    {
        m_IsTimeUp[1] = flag;
        IsSended = false;
        RequestOwner();
    }

    public Vector2 ReceiveThrowPower()
    {
        return m_throwPower;
    }

    public Vector2 ReceiveThrowGaugePos()
    {
        return m_throwGaugePosition;
    }

    public Vector3 ReceivePlayerPos()
    {
        return m_playerPos;
    }

    public Vector3 ReceiveThrowPos()
    {
        return m_throwPos;
    }

    public Quaternion ReveiveQuaternion()
    {
        return m_rot;
    }

    public int ReceiveState()
    {
        return m_state;
    }

    public bool ReceiveMasterIsTimeUp()
    {
        return m_IsTimeUp[0];
    }

    public bool ReceiveClientIsTimeUp()
    {
        return m_IsTimeUp[1];
    }
    public bool IsOwner()
    {
        return photonView.IsMine;
    }
}
