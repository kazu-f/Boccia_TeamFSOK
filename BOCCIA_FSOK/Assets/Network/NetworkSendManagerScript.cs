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
    private bool[] m_IsTimeUp = new bool[2];
    private int m_state = 0;                         //決定かどうか。
    private bool[] m_SyncFlag = new bool[2];        //同期をとれたかどうか。
    private Vector2Int m_RemainBalls = Vector2Int.one;      //残りのボール数
    private Team m_NextTeam = Team.Num;     //次に投げるチーム
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
                //まだデータを送っていないとき
                //データを他のプレイヤーに送る
                stream.SendNext(m_throwPower);
                stream.SendNext(m_throwGaugePosition);
                stream.SendNext(m_state);
                stream.SendNext(m_IsTimeUp);
                stream.SendNext(m_SyncFlag);
                stream.SendNext(m_RemainBalls);
                stream.SendNext(m_NextTeam);
                IsSended = true;
            }
        }
        else
        {
            //データを受け取る
            m_throwPower = (Vector2)stream.ReceiveNext();
            m_throwGaugePosition = (Vector2)stream.ReceiveNext();
            m_state = (int)stream.ReceiveNext();
            m_IsTimeUp = (bool[])stream.ReceiveNext();
            m_SyncFlag = (bool[])stream.ReceiveNext();
            m_RemainBalls = (Vector2Int)stream.ReceiveNext();
            m_NextTeam = (Team)stream.ReceiveNext();
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

    public void SendState(int state)
    {
        m_state = state;
        IsSended = false;
        RequestOwner();
    }

    public void SendMasterIsTimeUp(bool flag)
    {
        m_IsTimeUp[0] = flag;
        IsSended = false;
        RequestOwner();
    }

    public void SendClientIsTimeUp(bool flag)
    {
        m_IsTimeUp[1] = flag;
        IsSended = false;
        RequestOwner();
    }

    public void SendSyncFlag(bool[] flag)
    {
        m_SyncFlag = flag;
        IsSended = false;
        RequestOwner();
    }

    public void SendMasterSyncFlag(bool flag)
    {
        m_SyncFlag[0] = flag;
        IsSended = false;
        RequestOwner();
    }
    public void SendClientSyncFlag(bool flag)
    {
        m_SyncFlag[1] = flag;
        IsSended = false;
        RequestOwner();
    }

    public void SendRemainBalls(Vector2Int balls)
    {
        m_RemainBalls = balls;
        IsSended = false;
        RequestOwner();
    }

    public void SendNextTeam(Team team)
    {
        m_NextTeam = team;
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
    
    public bool[] ReceiveSyncFlag()
    {
        return m_SyncFlag;
    }

    public bool ReceiveMasterSyncFlag()
    {
        return m_SyncFlag[0];
    }

    public Vector2Int ReceiveRemainBalls()
    {
        return m_RemainBalls;
    }

    public Team ReceiveNextTeam()
    {
        return m_NextTeam;
    }


    public bool IsOwner()
    {
        return photonView.IsMine;
    }
}