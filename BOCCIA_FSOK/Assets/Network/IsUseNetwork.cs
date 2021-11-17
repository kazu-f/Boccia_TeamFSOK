using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IsUseNetwork : MonoBehaviour
{
    [SerializeField]private GameObject sendManagerObj = null;
    private NetworkSendManagerScript sendManager = null;
    private NetworkManagerScript networkManager = null;
    private bool isUseAI = true;       //AI���g�p���邩�ǂ����B
    private Team playerTeamCol = Team.Red;      //�v���C���[�̃`�[���J���[�B
    private int playerNo = -1;                  //�v���C���[�ԍ��B

    private void Awake()
    {
        playerNo = Photon.Pun.PhotonNetwork.LocalPlayer.ActorNumber;
        if (playerNo == 1)
        {
            playerTeamCol = Team.Red;
        }
        else if (playerNo == 2)
        {
            playerTeamCol = Team.Blue;
        }
        else
        {
            Debug.LogError("�v���C���[�ԍ��̒l���s���ł��B");
        }

        //�I�t���C�����[�h��������AI���g�p����B
        isUseAI = Photon.Pun.PhotonNetwork.OfflineMode;
    }

    // Start is called before the first frame update
    void Start()
    {
        //AI��łȂ���Βǉ�����B
        if (!isUseAI)
        {
            //�l�b�g���[�N�p�̃X�N���v�g�擾�B
            if (sendManagerObj != null)
            {
                sendManager = sendManagerObj.GetComponent<NetworkSendManagerScript>();
                networkManager = sendManagerObj.GetComponent<NetworkManagerScript>();
            }
            Debug.Log("�ʐM�ΐ���J�n�B");
        }
        else
        {
            ////�l�b�g�֌W�폜�B
            //Destroy(sendManagerObj);
            Debug.Log("AI�Ƃ̑ΐ���J�n�B");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool IsUseAI()
    {
        return isUseAI;
    }

    public Team GetPlayerCol()
    {
        return playerTeamCol;
    }

    public NetworkSendManagerScript GetSendManager()
    {
        return sendManager;
    }
}
