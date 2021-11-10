using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsUseNetwork : MonoBehaviour
{
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
            this.gameObject.AddComponent<Photon.Pun.PhotonView>();
            this.gameObject.AddComponent<NetworkManagerScript>();
            this.gameObject.AddComponent<NetworkSendManagerScript>();
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
}
