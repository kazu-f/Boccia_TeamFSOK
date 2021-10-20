using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class NetworkSendManagerScript : MonoBehaviourPunCallbacks,IPunObservable
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
}
