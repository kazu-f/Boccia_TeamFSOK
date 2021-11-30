using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class NetworkLauncherScript : MonoBehaviourPunCallbacks
{
    #region Private Serializable Fields
    [Tooltip("���[���̍ő�l���B���[���̐l�����ő傾�ƐV���Ƀ��[�����쐬���܂�")]
    [SerializeField]
    private byte maxPlayersPerRoom = 2;
    [SerializeField]
    private int SendRate = 15;      //1�b�Ԃɉ���p�P�b�g�𑗐M���邩
    #endregion

    #region Private Fields
    string gameVersion = "1.6";       // �Q�[���̃o�[�W����
    bool isConnecting;
    bool IsJoinedRoom = false;
    bool IsGameSceneLoaded = false;
    bool IsUseAI = false;
    bool IsDisconected = false;
    bool IsDisconecting = false;
    #endregion

    #region MonoBehaviour CallBacks

    private void Awake()
    {
        //�ڑ����Ă���v���C���[���������x����ǂݍ���
        PhotonNetwork.AutomaticallySyncScene = true;
    }
    // Start is called before the first frame update
    void Start()
    {
        connectServer.SetActive(false);
        waitMatching.SetActive(true);
        endMatching.SetActive(false);
    }

    #endregion

    #region Public Methods
    [Tooltip("�}�b�`���O������m�点��B")]
    [SerializeField]
    private GameObject endMatching;
    [Tooltip("�}�b�`���O���i�s���ł��邱�Ƃ����[�U�[�ɒm�点�邽�߂�UI")]
    [SerializeField]
    private GameObject waitMatching;
    [Tooltip("�T�[�o�[�ڑ����i�s���ł��邱�Ƃ����[�U�[�ɒm�点�邽�߂�UI")]
    [SerializeField]
    private GameObject connectServer;

    /// <summary>
    /// �ڑ�����
    /// ���ɐڑ�����Ă����烉���_���ȕ����ɎQ��
    /// �ڑ�����Ă��Ȃ����PhotonCloudNetwork�ɐڑ�
    /// </summary>
    public void Connect()
    {
        connectServer.SetActive(true);
        waitMatching.SetActive(false);
        endMatching.SetActive(false);
        Debug.Log("Photon�ɐڑ����܂��B���ɐڑ�����Ă���΃����_���ȕ����ɎQ�����܂��B�ڑ�����Ă��Ȃ����Photon�T�[�o�[�ɐڑ����܂�");
        //�ڑ�����Ă��邩�m�F
        if(PhotonNetwork.IsConnected)
        {
            //�ڑ�����Ă���ꍇ�����_���ȕ����ɎQ������
            //���s�����ꍇ��OnJoinRandomFailed�֐��Œʒm���󂯎��A�쐬����
            PhotonNetwork.JoinRandomRoom();
            Debug.Log("�����_���ȕ����ɎQ�����܂�");
        }
        else
        {
            //�ڑ�����Ă��Ȃ��ꍇ
            //Photo�T�[�o�[�ɐڑ�����
            isConnecting = PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = gameVersion;
            PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.KeepAliveInBackground = 0.0f;     //�A�v���|�[�Y�̋��e���ԁH�B
            //1�b�Ԃɓ�������񐔂�ύX
            PhotonNetwork.SendRate = SendRate;
            PhotonNetwork.SerializationRate = SendRate;
            Debug.Log("Photon�T�[�o�[�ɐڑ����܂�");
        }
    }
    #endregion

    #region MonoBehaviourPunCallbacks Callbacks
    public override void OnConnectedToMaster()
    {
        if(!PhotonNetwork.OfflineMode)
        {
            connectServer.SetActive(false);
            waitMatching.SetActive(true);
            endMatching.SetActive(false);
        }
        else
        {
            connectServer.SetActive(false);
            waitMatching.SetActive(false);
            endMatching.SetActive(true);
        }

        if (isConnecting)
        {
            PhotonNetwork.JoinRandomRoom();
            isConnecting = false;
        }
        Debug.Log("OnConnectedToMaster()��PUN�ɂ���ČĂ΂�܂���");
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        connectServer.SetActive(false);
        waitMatching.SetActive(false);
        endMatching.SetActive(true);
        isConnecting = false;
        Debug.LogWarningFormat("OnDisconnected()��PUN�ɂ���ČĂ΂�܂����B����{0}",cause);
        IsDisconected = true;
    }

    //�v���C���[���������Ă����B
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);

        if(newPlayer.ActorNumber == maxPlayersPerRoom)
        {
            //�}�b�`���O������ʒm�B
            connectServer.SetActive(false);
            waitMatching.SetActive(false);
            endMatching.SetActive(true);
        }
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("OnJoinRandomFailed()��PUN�ɂ���ČĂяo����܂����B���p�\�ȃ����_���ȕ������Ȃ����ߍ쐬���܂�");
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = maxPlayersPerRoom });
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("OnJoinedRoom()��PUN�ɂ���ČĂ΂�܂����B���݃��[���ɎQ���ł��܂����B");
        IsJoinedRoom = true;
    }

    public override void OnLeftRoom()
    {
        Debug.Log("OnLeftRoom()��PUN�ɂ���ČĂ΂�܂����B���݃��[�����甲���܂����B");
        IsJoinedRoom = false;
    }
    #endregion

    public void JoinRandomRoom()
    {
        //�ڑ�����Ă��邩�m�F
        if (PhotonNetwork.IsConnected)
        {
            //�ڑ�����Ă���ꍇ�����_���ȕ����ɎQ������
            //���s�����ꍇ��OnJoinRandomFailed�֐��Œʒm���󂯎��A�쐬����
            PhotonNetwork.JoinRandomRoom();
            Debug.Log("�����_���ȕ����ɎQ�����܂�");
        }
    }
    private void Update()
    {
        if (!IsDisconecting)
        {
            if (IsJoinedRoom)
            {
                //�����ɎQ�����Ă���Ƃ�
                if (PhotonNetwork.CurrentRoom.PlayerCount == maxPlayersPerRoom)
                {
                    if (IsMasterClient())
                    {
                        //�}�X�^�[�N���C�A���g�Ȃ̂ŃV�[�������[�h���܂�
                        if (!IsGameSceneLoaded)
                        {
                            //�����̐l�����ő傾�ƃQ�[���V�[���Ɉڍs
                            Debug.Log("�Q�[���V�[�������[�h���܂�");
                            PhotonNetwork.LoadLevel("BocciaGameScene");
                            IsGameSceneLoaded = true;
                        }
                    }
                }
                if (IsUseAI)
                {
                    //AI���g�p����Ƃ�
                    //�����𔲂���
                    Debug.Log("���[�����甲���܂�");
                    PhotonNetwork.LeaveRoom();
                    //�ڑ���ؒf����
                    Debug.Log("�ڑ���ؒf���܂�");
                    PhotonNetwork.Disconnect();
                    IsDisconecting = true;
                }
            }
        }
        if (IsDisconected)
        {
            if (IsUseAI)
            {
                //�ڑ���ؒf������
                if (!IsGameSceneLoaded)
                {
                    //�I�t���C�����[�h�ɂ���
                    PhotonNetwork.OfflineMode = true;
                    //�I�t���C���p�̕����ɎQ��
                    PhotonNetwork.JoinRandomRoom();
                    //�����̐l�����ő傾�ƃQ�[���V�[���Ɉڍs
                    Debug.Log("�Q�[���V�[�������[�h���܂�");
                    SceneManager.LoadScene("BocciaGameScene");
                    IsGameSceneLoaded = true;
                }
            }
        }

    }

    /// <summary>
    /// �������}�X�^�[�N���C�A���g���ǂ���
    /// </summary>
    /// <returns>�}�X�^�[�N���C�A���g���ǂ����̃t���O</returns>
    public bool IsMasterClient()
    {
        return PhotonNetwork.LocalPlayer.IsMasterClient;
    }

    /// <summary>
    /// �Ăяo���ƁA�l�b�g���[�N��ؒf���ăQ�[���V�[�������[�h����
    /// </summary>
    public void UseAI()
    {
        IsUseAI = true;
    }
}
