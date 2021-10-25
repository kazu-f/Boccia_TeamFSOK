using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class NetworkSendManagerScript : MonoBehaviourPunCallbacks,IPunObservable,IPunOwnershipCallbacks
{
    bool IsSended = false;
    public float m_f = 0.0f;
    public Vector3 m_vec = Vector3.one;
    private Quaternion m_rot = Quaternion.identity;
    private PhotonView photonView = null;

    public void Awake()
    {
        photonView = this.gameObject.GetComponent<PhotonView>();
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
                stream.SendNext(m_f);
                stream.SendNext(m_vec);
                stream.SendNext(m_rot);
                IsSended = true;
            }
        }
        else
        {
            //�f�[�^���󂯎��
            m_f = (float)stream.ReceiveNext();
            m_vec = (Vector3)stream.ReceiveNext();
            m_rot = (Quaternion)stream.ReceiveNext();
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

    public void SendVector(Vector3 vec)
    {
        m_vec = vec;
        IsSended = false;
        RequestOwner();
    }

    public void SendQuaternion(Quaternion rot)
    {
        m_rot = rot;
        IsSended = false;
        RequestOwner();
    }

    public void SendFloat(float f)
    {
        m_f = f;
        IsSended = false;
        RequestOwner();
    }

    public Vector3 ReceiveVector()
    {
        return m_vec;
    }

    public Quaternion ReveiveQuaternion()
    {
        return m_rot;
    }

    public float ReceiveFloat()
    {
        return m_f;
    }
}
