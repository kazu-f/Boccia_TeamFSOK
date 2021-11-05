using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsUseNetwork : MonoBehaviour
{
    private bool isUseAI = true;       //AI���g�p���邩�ǂ����B
    private Team playerTeamCol = Team.Red;      //�v���C���[�̃`�[���J���[�B


    private void Awake()
    {
        //AI��łȂ���Βǉ�����B
        if(!isUseAI)
        {
            this.gameObject.AddComponent<NetworkManagerScript>();
            this.gameObject.AddComponent<NetworkSendManagerScript>();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
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
