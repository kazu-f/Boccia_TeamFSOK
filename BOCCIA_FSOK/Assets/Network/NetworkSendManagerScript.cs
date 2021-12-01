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
        SendSyncDataToClient,           // �N���C�A���g�ɓ����f�[�^�𑗂�B
        SendRecievedSyncDataToMaster,   // �}�X�^�[�ɓ����f�[�^���󂯎�������Ƃ�ʒm����B
    }
    /// <summary>
    /// �N���C�A���g����̃f�[�^��M�ʒm���b�Z�[�W
    /// </summary>
    struct NotifyRecievedSyndDataFromClient
    {
        public int m_turnNo;
    };
    bool IsSended = false;
    int sendDataType = (int)DataType.None;      //���M����f�[�^�̎�ނ𔻕ʂ���ϐ��B
    int receiveDataType = (int)DataType.None;      //��M����f�[�^�̎�ނ𔻕ʂ���ϐ��B

    #region ThrowData
    private Vector2 m_throwPower = Vector2.zero;
    private Vector2 m_throwGaugePosition = Vector2.zero;
    private int m_state = 0;                         //���肩�ǂ����B
    #endregion

    #region GameData
    private bool[] m_IsTimeUp = new bool[2];
    public bool IsRecieved_SyncDataFromMaster { get;set; }
    public bool IsRecieved_NotifyRecievedSyncDataFromClient( int turnNo )
    {
        return m_IsRecieved_NotifyRecievedSyncDataFromClient.Exists(
            n => n == turnNo
        );
    }

    private int[] m_RemainBalls = new int[2];      //�c��̃{�[����
    private int m_NextTeam = -1;     //���ɓ�����`�[��
    private int m_FirstTeam = -1;
    #endregion

    List<NotifyRecievedSyndDataFromClient> m_notifyRecievedSyndDataFromClientList = new List<NotifyRecievedSyndDataFromClient>();
    public List<int> m_IsRecieved_NotifyRecievedSyncDataFromClient { get; set; }

    private PhotonView photonView = null;
    
    public void Awake()
    {
        photonView = this.gameObject.GetComponent<PhotonView>();
        for(int i = 0;i < m_IsTimeUp.Length;i++)
        {
            m_IsTimeUp[i] = false;
        }
        m_IsRecieved_NotifyRecievedSyncDataFromClient = new List<int>();
    }

    #region IPunObservable implementation
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            
            foreach( var d in m_notifyRecievedSyndDataFromClientList)
            {
                stream.SendNext((int)DataType.SendRecievedSyncDataToMaster);
                stream.SendNext(d.m_turnNo);
            }
            m_notifyRecievedSyndDataFromClientList.Clear();

            //�܂��f�[�^�𑗂��Ă��Ȃ��Ƃ�
            //�f�[�^�𑼂̃v���C���[�ɑ���
            if (!IsSended)
            {
                //�f�[�^�^�C�v�𑗐M�B
                stream.SendNext(sendDataType);
                //��ނ��Ƃɕ������B
                switch (sendDataType)
                {
                    case (int)DataType.None:

                        Debug.Log("SendData:None");
                        break;
                    case (int)DataType.ThrowData:
                        //�{�[���𓊂��鎞�Ɏg���f�[�^�B
                        stream.SendNext(m_throwPower);
                        stream.SendNext(m_throwGaugePosition);
                        stream.SendNext(m_state);

                        Debug.Log("SendData:ThrowData");
                        break;
                    case (int)DataType.GameData:
                        //�Q�[���i�s�Ɏg���f�[�^�B
                        stream.SendNext(m_IsTimeUp);
                        stream.SendNext(m_RemainBalls);
                        stream.SendNext(m_NextTeam);
                        stream.SendNext(m_FirstTeam);

                        Debug.Log("SendData:GameData");
                        break;
                    case (int)DataType.SendSyncDataToClient:
                        stream.SendNext(m_IsTimeUp);
                        stream.SendNext(m_RemainBalls);
                        stream.SendNext(m_NextTeam);
                        stream.SendNext(m_FirstTeam);
                        Debug.Log("SendData:SendSyncDataToClient");
                        break;

                }

                IsSended = true;

                //�K�v���ǂ��������H
                sendDataType = (int)DataType.None;
            }
        }
        else
        {
            //�f�[�^���󂯎��
            receiveDataType = (int)stream.ReceiveNext();
            //��ނ��Ƃɕ������B
            switch (receiveDataType)
            {
                case (int)DataType.None:

                    Debug.Log("ReceiveData:None");
                    break;
                case (int)DataType.ThrowData:
                    //�{�[���𓊂��鎞�Ɏg���f�[�^�B
                    m_throwPower = (Vector2)stream.ReceiveNext();
                    m_throwGaugePosition = (Vector2)stream.ReceiveNext();
                    m_state = (int)stream.ReceiveNext();

                    Debug.Log("ReceiveData:ThrowData");
                    break;
                case (int)DataType.GameData:
                    //�Q�[���i�s�Ɏg���f�[�^�B
                    m_IsTimeUp = (bool[])stream.ReceiveNext();
                    m_RemainBalls = (int[])stream.ReceiveNext();
                    m_NextTeam = (int)stream.ReceiveNext();
                    m_FirstTeam = (int)stream.ReceiveNext();
                    
                    break;
                case (int)DataType.SendSyncDataToClient:
                    // �����f�[�^����M�����B
                    m_IsTimeUp = (bool[])stream.ReceiveNext();
                    m_RemainBalls = (int[])stream.ReceiveNext();
                    m_NextTeam = (int)stream.ReceiveNext();
                    m_FirstTeam = (int)stream.ReceiveNext();
                    IsRecieved_SyncDataFromMaster = true;
                    Debug.Log("ReceiveData:SendSyncDataToClient");
                    break;
                case (int)DataType.SendRecievedSyncDataToMaster:
                    // �N���C�A���g���瓯���f�[�^���󂯎�������Ƃ��ʒm���ꂽ�B
                    if(m_IsRecieved_NotifyRecievedSyncDataFromClient.Count == 100)
                    {
                        // �Â��f�[�^���폜���Ă����B
                        m_IsRecieved_NotifyRecievedSyncDataFromClient.RemoveAt(0);
                    }
                    m_IsRecieved_NotifyRecievedSyncDataFromClient.Add((int)stream.ReceiveNext());
                    Debug.Log("ReceiveData:RecievedSyncDataToMaster");
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
    public void SendRecievedSyncDataToMaster(int turnNo)
    {
        var newData = new NotifyRecievedSyndDataFromClient();
        newData.m_turnNo = turnNo;
        m_notifyRecievedSyndDataFromClientList.Add(newData);   
        RequestOwner();
    }
    public void SendThrowPow(Vector2 vec2)
    {
        m_throwPower = vec2;
        sendDataType = (int)DataType.ThrowData;     //�{�[���𓊂��鎞�Ɏg���f�[�^�B
        IsSended = false;
        RequestOwner();
    }

    public void SendThrowGaugePosition(Vector2 vec2)
    {
        m_throwGaugePosition = vec2;
        sendDataType = (int)DataType.ThrowData;     //�{�[���𓊂��鎞�Ɏg���f�[�^�B
        IsSended = false;
        RequestOwner();
    }

    public void SendState(int state)
    {
        m_state = state;
        sendDataType = (int)DataType.ThrowData;     //�{�[���𓊂��鎞�Ɏg���f�[�^�B
        IsSended = false;
        RequestOwner();
    }

    public void SendMasterIsTimeUp(bool flag)
    {
        m_IsTimeUp[0] = flag;
        sendDataType = (int)DataType.GameData;     //�Q�[���i�s�Ɏg���f�[�^�B
        IsSended = false;
        RequestOwner();
    }

    public void SendClientIsTimeUp(bool flag)
    {
        m_IsTimeUp[1] = flag;
        sendDataType = (int)DataType.GameData;     //�Q�[���i�s�Ɏg���f�[�^�B
        IsSended = false;
        RequestOwner();
    }
    /// <summary>
    /// �����f�[�^���N���C�A���g�ɑ���B
    /// </summary>
    /// <param name="teamFlowScript"></param>
    public void SendSyncDataToClient(TeamFlowScript teamFlowScript)
    {
        SendRemainBalls(teamFlowScript.GetRemainBalls());
        m_NextTeam = (int)teamFlowScript.GetNowTeam();
        m_FirstTeam = (int)teamFlowScript.GetFirstTeam();
        sendDataType = (int)DataType.SendSyncDataToClient;     //�Q�[���i�s�Ɏg���f�[�^�B
        IsSended = false;
        RequestOwner();
    }
    /// <summary>
    /// �}�X�^�[����̓����f�[�^���󂯎��B
    /// </summary>
    /// <param name=""></param>
    public bool RecieveSyncDataFromMaster(TeamFlowScript teamFlowScript)
    {
        if (IsRecieved_SyncDataFromMaster == false)
        {
            return false;
        }
        teamFlowScript.SetRemainBalls(ReceiveRemainBalls());
        teamFlowScript.SetNextTeam(ReceiveNextTeam());
        teamFlowScript.SetFirstTeam(ReceiveFirstTeam());
        // ��M�����̂ŁA��M�t���O���I�t�ɂ���B
        IsRecieved_SyncDataFromMaster = false;
        return true;
    }
    void SendRemainBalls(int[] balls)
    {
        m_RemainBalls = balls;
        for(int i = 0;i < balls.Length;i++)
        {
            m_RemainBalls[i] = balls[i];
        }
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