using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsUseNetwork : MonoBehaviour
{
    private GameObject sendManagerObj = null;
    private NetworkSendManagerScript sendManager = null;
    private NetworkManagerScript networkManager = null;
    private bool isUseAI = true;       //AI���g�p���邩�ǂ����B
    private Team playerTeamCol = Team.Red;      //�v���C���[�̃`�[���J���[�B


    private void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        //AI��łȂ���Βǉ�����B
        if (!isUseAI)
        {
            //�l�b�g���[�N�p�̃I�u�W�F�N�g���쐬�B
            sendManagerObj = Photon.Pun.PhotonNetwork.Instantiate("SendNetWorkObj",Vector3.zero,Quaternion.identity);
            if(sendManagerObj != null)
            {
                sendManager = sendManagerObj.GetComponent<NetworkSendManagerScript>();
                networkManager = sendManagerObj.GetComponent<NetworkManagerScript>();
            }

            Debug.Log("�ʐM�ΐ���J�n�B");
        }
        else
        {
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

    public void SetUseAI(bool flag)
    {
        isUseAI = flag;
    }

    public Team GetPlayerCol()
    {
        return playerTeamCol;
    }

    public void SetTeamCol(Team col)
    {
        playerTeamCol = col;
    }

    public NetworkSendManagerScript GetSendManager()
    {
        return sendManager;
    }
}
