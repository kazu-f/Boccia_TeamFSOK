using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class NetworkSendManagerScript : MonoBehaviourPunCallbacks,IPunObservable,IPunOwnershipCallbacks
{
    public float f = 0.0f;
    public Vector3 force = Vector3.one;
    private PhotonView photonView = null;

    public void Awake()
    {
        photonView = this.gameObject.GetComponent<PhotonView>();
    }

    #region IPunObservable implementation
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.IsWriting)
        {
            //�f�[�^�𑼂̃v���C���[�ɑ���
            stream.SendNext(f);
            stream.SendNext(force);
        }
        else
        {
            //�f�[�^���󂯎��
            f = (float)stream.ReceiveNext();
            force = (Vector3)stream.ReceiveNext();
        }
    }

    #endregion

    public void Change1()
    {
        f++;
        RequestOwner();
    }
    public void Change2()
    {
        force *= 2.0f;
        RequestOwner();
    }

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
}
