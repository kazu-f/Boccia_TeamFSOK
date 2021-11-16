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
    #endregion

    #region Private Fields
    string gameVersion = "1.1";       // �Q�[���̃o�[�W����
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
        progressLabel.SetActive(false);
        controlPanel.SetActive(true);
    }

    #endregion

    #region Public Methods
    [Tooltip("���[�U�[�����O��ݒ�ł���UI")]
    [SerializeField]
    private GameObject controlPanel;
    [Tooltip("�ڑ����i�s���ł��邱�Ƃ����[�U�[�ɒm�点�邽�߂�UI")]
    [SerializeField]
    private GameObject progressLabel;

    /// <summary>
    /// �ڑ�����
    /// ���ɐڑ�����Ă����烉���_���ȕ����ɎQ��
    /// �ڑ�����Ă��Ȃ����PhotonCloudNetwork�ɐڑ�
    /// </summary>
    public void Connect()
    {
        progressLabel.SetActive(true);
        controlPanel.SetActive(false);
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
            Debug.Log("Photon�T�[�o�[�ɐڑ����܂�");
        }
    }
    #endregion

    #region MonoBehaviourPunCallbacks Callbacks
    public override void OnConnectedToMaster()
    {
        if(isConnecting)
        {
            PhotonNetwork.JoinRandomRoom();
            isConnecting = false;
        }
        Debug.Log("OnConnectedToMaster()��PUN�ɂ���ČĂ΂�܂���");
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        progressLabel.SetActive(false);
        controlPanel.SetActive(true);
        isConnecting = false;
        Debug.LogWarningFormat("OnDisconnected()��PUN�ɂ���ČĂ΂�܂����B����{0}",cause);
        IsDisconected = true;
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
                    if (!IsGameSceneLoaded)
                    {
                        //�����̐l�����ő傾�ƃQ�[���V�[���Ɉڍs
                        Debug.Log("�Q�[���V�[�������[�h���܂�");
                        //�V�[���؂�ւ����̏�����ǉ��B
                        SceneManager.sceneLoaded += SendGameConfig;
                        PhotonNetwork.LoadLevel("BocciaGameScene");
                        IsGameSceneLoaded = true;
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
                    //�V�[���؂�ւ����̏�����ǉ��B
                    SceneManager.sceneLoaded += SendGameConfig;
                    SceneManager.LoadScene("BocciaGameScene");
                    IsGameSceneLoaded = true;
                }
            }
        }

    }

    /// <summary>
    /// AI�ΐ킩�ǂ����A1P���ǂ����Ȃǂ��Q�[���V�[���֑��M�B
    /// </summary>
    private void SendGameConfig(Scene next, LoadSceneMode mode)
    {
        //����̃I�u�W�F�N�g��T���B
        foreach (var gameObject in next.GetRootGameObjects())
        {
            if (gameObject.tag == "Network")
            {
                var useNetworkManager = gameObject.GetComponent<IsUseNetwork>();
                if(useNetworkManager != null)
                {
                    //AI���g�p���邩�ǂ����B
                    useNetworkManager.SetUseAI(IsUseAI);
                    //�v���C���[�`�[���J���[���ǂ������B
                    if(IsUseAI)
                    {
                        useNetworkManager.SetTeamCol(Team.Red);
                    }
                    else
                    {
                        if(IsMasterClient())
                        {
                            useNetworkManager.SetTeamCol(Team.Red);
                        }
                        else
                        {
                            useNetworkManager.SetTeamCol(Team.Blue);
                        }
                    }
                }
                //���f�B
                break;
            }
        }

        SceneManager.sceneLoaded -= SendGameConfig;
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
