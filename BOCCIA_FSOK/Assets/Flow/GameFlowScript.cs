using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// �Q�[���i�s����
/// �P�D�G���h���̊Ǘ�
/// �Q�D�G���h�I�����̃��Z�b�g
/// �R�D�S�G���h�I�����V�[���J��
/// </summary>

public class GameFlowScript : MonoBehaviour
{
    private EndFlowScript endFlow;   //�G���h�i�s����X�N���v�g�B
    private TeamFlowScript teamFlow;   //������`�[�������肷��X�N���v�g�B

    public int GAME_END_NUM = 2;    //1�Q�[���ӂ�̃G���h���B
    private int nowEndNo = 0;               //���݂̃G���h���B

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
