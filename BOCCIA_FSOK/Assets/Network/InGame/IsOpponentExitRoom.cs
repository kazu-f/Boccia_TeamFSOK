using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon;
using Photon.Pun;
using Photon.Realtime;

//�ΐ푊�肪���[�����甲�������ǂ������Ď�����X�N���v�g�B
public class IsOpponentExitRoom : MonoBehaviourPunCallbacks
{
    #region SerializeField
    [Tooltip("�ؒf���ꂽ���Ƃ�ʒm����I�u�W�F�N�g�B")]
    [SerializeField] private GameObject opponentExit = null;
    #endregion

    #region Private Properties
    private GameFlowScript gameFlow = null;     //�Q�[���i�s�Ǘ��I�u�W�F�N�g�B
    #endregion

    #region Callbacks

    // �N�����������B
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        // ���������͂���
        base.OnPlayerLeftRoom(otherPlayer);
        //�ؒf���ꂽ���Ƃ�ʒm����B
        if(opponentExit && !gameFlow.isFinishGame)
        {
            opponentExit.SetActive(true);
        }
        
        if (PhotonNetwork.IsConnected)
        {
            //�������甲����B
            PhotonNetwork.LeaveRoom();
            PhotonNetwork.Disconnect();
        }
    }
    // �ؒf���ꂽ�B
    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);
        if(opponentExit && !gameFlow.isFinishGame)
        {
            opponentExit.SetActive(true);
        }
    }

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        //�Q�[���Ǘ��I�u�W�F�N�g�擾�B
        var Obj = GameObject.FindGameObjectWithTag("GameFlow");
        if (Obj != null)
        {
            gameFlow = Obj.GetComponent<GameFlowScript>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnApplicationPause(bool pause)
    {
        if(pause)
        {
            //�ؒf���ꂽ���Ƃ�ʒm����B
            if (opponentExit && !gameFlow.isFinishGame)
            {
                opponentExit.SetActive(true);
            }

            if (PhotonNetwork.IsConnected)
            {
                //�������甲����B
                PhotonNetwork.LeaveRoom();
                PhotonNetwork.Disconnect();
            }
        }
    }

    //�Q�[�����I��鎞�ɂ̓l�b�g����ؒf���Ă����B
    private void OnDestroy()
    {
        if (PhotonNetwork.IsConnected)
        {
            //�������甲����B
            PhotonNetwork.LeaveRoom();
            PhotonNetwork.Disconnect();
        }
    }
}
