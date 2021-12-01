using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class NetworkSendManagerScript : MonoBehaviourPunCallbacks,IPunObservable,IPunOwnershipCallbacks
{
    enum DataType { 
        None,
        ThrowData,
        GameData,    
    }

    bool IsSended = false;
    int sendDataType = (int)DataType.None;      //送信するデータの種類を判別する変数。
    int receiveDataType = (int)DataType.None;      //受信するデータの種類を判別する変数。

    #region ThrowData
    private Vector2 m_throwPower = Vector2.zero;
    private Vector2 m_throwGaugePosition = Vector2.zero;
    private int m_state = 0;                         //決定かどうか。
    #endregion

    #region GameData
    private bool[] m_IsTimeUp = new bool[2];
    private bool[] m_SyncFlag = new bool[2];        //同期をとれたかどうか。
    private int[] m_RemainBalls = new int[2];      //残りのボール数
    private int m_NextTeam = -1;     //次に投げるチーム
    private int m_FirstTeam = -1;
    #endregion

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
            //データタイプを送信。
            stream.SendNext(sendDataType);
            //まだデータを送っていないとき
            //データを他のプレイヤーに送る
            if (!IsSended)
            {
                //種類ごとに分かれる。
                switch (sendDataType)
                {
                    case (int)DataType.None:

                        Debug.Log("SendData:None");
                        break;
                    case (int)DataType.ThrowData:
                        //ボールを投げる時に使うデータ。
                        stream.SendNext(m_throwPower);
                        stream.SendNext(m_throwGaugePosition);
                        stream.SendNext(m_state);

                        Debug.Log("SendData:ThrowData");
                        break;
                    case (int)DataType.GameData:
                        //ゲーム進行に使うデータ。
                        stream.SendNext(m_IsTimeUp);
                        stream.SendNext(m_SyncFlag);
                        stream.SendNext(m_RemainBalls);
                        stream.SendNext(m_NextTeam);
                        stream.SendNext(m_FirstTeam);

                        Debug.Log("SendData:GameData");
                        break;                
                }

                IsSended = true;

                //必要かどうか微妙？
                sendDataType = (int)DataType.None;
            }
        }
        else
        {
            //データを受け取る
            receiveDataType = (int)stream.ReceiveNext();
            //種類ごとに分かれる。
            switch (receiveDataType)
            {
                case (int)DataType.None:

                    Debug.Log("ReceiveData:None");
                    break;
                case (int)DataType.ThrowData:
                    //ボールを投げる時に使うデータ。
                    m_throwPower = (Vector2)stream.ReceiveNext();
                    m_throwGaugePosition = (Vector2)stream.ReceiveNext();
                    m_state = (int)stream.ReceiveNext();

                    Debug.Log("ReceiveData:ThrowData");
                    break;
                case (int)DataType.GameData:
                    //ゲーム進行に使うデータ。
                    m_IsTimeUp = (bool[])stream.ReceiveNext();
                    m_SyncFlag = (bool[])stream.ReceiveNext();
                    m_RemainBalls = (int[])stream.ReceiveNext();
                    m_NextTeam = (int)stream.ReceiveNext();
                    m_FirstTeam = (int)stream.ReceiveNext();
                    Debug.Log("ReceiveData:GameData");
                    break;
            }
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
        sendDataType = (int)DataType.ThrowData;     //ボールを投げる時に使うデータ。
        IsSended = false;
        RequestOwner();
    }

    public void SendThrowGaugePosition(Vector2 vec2)
    {
        m_throwGaugePosition = vec2;
        sendDataType = (int)DataType.ThrowData;     //ボールを投げる時に使うデータ。
        IsSended = false;
        RequestOwner();
    }

    public void SendState(int state)
    {
        m_state = state;
        sendDataType = (int)DataType.ThrowData;     //ボールを投げる時に使うデータ。
        IsSended = false;
        RequestOwner();
    }

    public void SendMasterIsTimeUp(bool flag)
    {
        m_IsTimeUp[0] = flag;
        sendDataType = (int)DataType.GameData;     //ゲーム進行に使うデータ。
        IsSended = false;
        RequestOwner();
    }

    public void SendClientIsTimeUp(bool flag)
    {
        m_IsTimeUp[1] = flag;
        sendDataType = (int)DataType.GameData;     //ゲーム進行に使うデータ。
        IsSended = false;
        RequestOwner();
    }

    public void SendSyncFlag(bool[] flag)
    {
        m_SyncFlag = flag;
        sendDataType = (int)DataType.GameData;     //ゲーム進行に使うデータ。
        IsSended = false;
        RequestOwner();
    }

    public void SendMasterSyncFlag(bool flag)
    {
        m_SyncFlag[0] = flag;
        sendDataType = (int)DataType.GameData;     //ゲーム進行に使うデータ。
        IsSended = false;
        RequestOwner();
    }
    public void SendClientSyncFlag(bool flag)
    {
        m_SyncFlag[1] = flag;
        sendDataType = (int)DataType.GameData;     //ゲーム進行に使うデータ。
        IsSended = false;
        RequestOwner();
    }

    public bool ResetSyncFlag()
    {
        if (IsSended)
        {
            m_SyncFlag[0] = false;
            m_SyncFlag[1] = false;
        }
        return IsSended;
    }

    public void SendRemainBalls(int[] balls)
    {
        m_RemainBalls = balls;
        for(int i = 0;i < balls.Length;i++)
        {
            m_RemainBalls[i] = balls[i];
        }
        sendDataType = (int)DataType.GameData;     //ゲーム進行に使うデータ。
        IsSended = false;
        RequestOwner();
    }

    public void SendNextTeam(int team)
    {
        m_NextTeam = team;
        sendDataType = (int)DataType.GameData;     //ゲーム進行に使うデータ。
        IsSended = false;
        RequestOwner();
    }

    public void SendFirstTeam(int team)
    {
        m_FirstTeam = team;
        sendDataType = (int)DataType.GameData;     //ゲーム進行に使うデータ。
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
    public bool ReceiveClientSyncFlag()
    {
        return m_SyncFlag[1];
    }


    public int[] ReceiveRemainBalls()
    {
        return m_RemainBalls;
    }

    public int ReceiveNextTeam()
    {
        return m_NextTeam;
    }

    public int ReceiveFirstTeam()
    {
        return m_FirstTeam;
    }
    public bool IsOwner()
    {
        return photonView.IsMine;
    }
}