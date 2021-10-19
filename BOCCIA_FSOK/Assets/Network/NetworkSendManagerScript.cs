using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class NetworkSendManagerScript : MonoBehaviourPunCallbacks,IPunObservable
{
    public float f = 0.0f;
    public Vector3 force = Vector3.one;

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
    }
    public void Change2()
    {
        force *= 2.0f;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
