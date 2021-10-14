using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    /// <summary>
    /// �Q�[���̃o�[�W����
    /// </summary>
    string gameVersion = "1";
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
    }

    #endregion

    #region Public Methods
    /// <summary>
    /// �ڑ�����
    /// ���ɐڑ�����Ă����烉���_���ȕ����ɎQ��
    /// �ڑ�����Ă��Ȃ����PhotonCloudNetwork�ɐڑ�
    /// </summary>
    public void Connect()
    {
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
            PhotonNetwork.GameVersion = gameVersion;
            PhotonNetwork.ConnectUsingSettings();
            Debug.Log("Photon�T�[�o�[�ɐڑ����܂�");
        }
    }
    #endregion

    #region MonoBehaviourPunCallbacks Callbacks
    public override void OnConnectedToMaster()
    {
        Debug.Log("OnConnectedToMaster()��PUN�ɂ���ČĂ΂�܂���");
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogWarningFormat("OnDisconnected()��PUN�ɂ���ČĂ΂�܂����B����{0}",cause);
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("OnJoinRandomFailed()��PUN�ɂ���ČĂяo����܂����B���p�\�ȃ����_���ȕ������Ȃ����ߍ쐬���܂�");
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = maxPlayersPerRoom });
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("OnJoinedRoom()��PUN�ɂ���ČĂ΂�܂����B���݃��[���ɎQ���ł��܂����B");
    }
    #endregion

    private void Update()
    {
        //PhotonNetwork.JoinRandomRoom();
    }


}
