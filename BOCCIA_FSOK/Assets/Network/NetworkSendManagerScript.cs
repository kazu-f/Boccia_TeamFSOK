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
            //データを他のプレイヤーに送る
            stream.SendNext(f);
            stream.SendNext(force);
        }
        else
        {
            //データを受け取る
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
            //オーナーじゃないとき
            if(photonView.OwnershipTransfer != OwnershipOption.Request)
            {
                Debug.LogError("OwnershipTransferをRequestに変更してください");
            }
            else
            {
                photonView.RequestOwnership();
            }
        }
    }

    //オーナー権限の設定がRequestの時に呼ばれる
    public void OnOwnershipRequest(PhotonView targetView, Player requestingPlayer)
    {
        Debug.Log("オーナー権限がリクエストされました");
        targetView.TransferOwnership(requestingPlayer);
    }

    //オーナー権限が移行したときに呼ばれる
    public void OnOwnershipTransfered(PhotonView targetView, Player previousOwner)
    {
        Debug.Log("オーナー権限が移行しました");
    }

    //オーナー権限の移行が失敗したときに呼ばれる
    public void OnOwnershipTransferFailed(PhotonView targetView, Player previousOwner)
    {
        Debug.LogError("オーナー権限の移行に失敗しました");
    }
}
