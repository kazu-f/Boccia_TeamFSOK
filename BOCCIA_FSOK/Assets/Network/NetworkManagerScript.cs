using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class NetworkManagerScript : MonoBehaviourPunCallbacks
{
    #region Private Methods
    /// <summary>
    /// �Q�[���V�[�������[�h����
    /// </summary>
    void LoadGameScene()
    {
        if(!PhotonNetwork.IsMasterClient)
        {
            //�}�X�^�[�N���C�A���g�ł͂Ȃ��Ƃ�
            Debug.LogError("���x����ǂݍ������Ƃ��܂������A���Ȃ��̓��[���}�X�^�[�ł͂���܂���");
        }
        Debug.LogFormat("PhotonNetwork : Loading Level : {0}", PhotonNetwork.CurrentRoom.PlayerCount);
        //�Q�[���V�[����ǂݍ���
        PhotonNetwork.LoadLevel("BocciaGameScene");
    }

    #endregion

    #region Photon Callbacks

    /// <summary>
    /// �v���C���[�����[���ɎQ�������Ƃ��ɌĂ΂��
    /// </summary>
    /// <param name="other">�Q�������v���C���[</param>
    public override void OnPlayerEnteredRoom(Player other)
    {
        Debug.LogFormat("OnPlayerEnteredRoom() {0}", other.NickName);
        if(PhotonNetwork.IsMasterClient)
        {
            //OnPlayerLeftRoom�̑O�ɌĂяo����܂��B
            Debug.LogFormat("OnPlayerEnteredRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient);
        }
    }

    /// <summary>
    /// �v���C���[�����[������ؒf�����Ƃ��ɌĂ΂��
    /// </summary>
    /// <param name="other">�ؒf�����v���C���[</param>
    public override void OnPlayerLeftRoom(Player other)
    {
        Debug.LogFormat("OnPlayerLeftRoom() {0}", other.NickName);
        if(PhotonNetwork.IsMasterClient)
        {
            Debug.LogFormat("OnPlayerLeftRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient);
        }
    }
    /// <summary>
    /// �v���C���[���������o�����ɌĂяo�����
    /// </summary>
    public override void OnLeftRoom()
    {

    }

    #endregion

    #region Public Methods

    /// <summary>
    /// ��������ޏo���鏈��
    /// </summary>
    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
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
}
