using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
public class PlayerBallScript : IBallScript
{
    private GameObject Failed = null;
    private Photon.Pun.PhotonView photonView;
    // Start is called before the first frame update
    void Start()
    {
        photonView = this.GetComponent<Photon.Pun.PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// �{�[�����G���A��ɓ��������̏���
    /// </summary>
    public override void InsideVenue()
    {
        InArea = true;
    }

    /// <summary>
    /// �{�[�����G���A�O�ɏo�����̏���
    /// </summary>
    public override void OutsideVenue()
    {
        GetOutRange = true;
        InArea = false;
        if (IsThrowing)
        {
            photonView.RPC(nameof(DispFailed), Photon.Pun.RpcTarget.All);
        }
        //this.gameObject.SetActive(false);
    }

    /// <summary>
    /// 失敗したことを表示する。
    /// </summary>
    [Photon.Pun.PunRPC]
    public void DispFailed()
    {
        //Failed = GameObject.Find("Failed");
        //Failed.GetComponent<FailedMoveScript>().SetDirect();
    }

    /// <summary>
    /// �{�[������~�����Ƃ��̏���
    /// </summary>
    public override void EndThrow()
    {
        IsThrowing = false;
        if (InArea == false)
        {

            this.gameObject.GetComponent<BallStateScript>().ResetState();
            this.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// �L���G���A�ɓ��������̏���
    /// </summary>
    public override void InKillArea()
    {
        InArea = false;
        this.gameObject.SetActive(false);
    }
}
