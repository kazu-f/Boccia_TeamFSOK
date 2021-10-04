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
    public GameObject[] gameObjects;    //�{�[���������Ă���Ԏ~�߂�I�u�W�F�N�g�����B
    private EndFlowScript endFlow;   //�G���h�i�s����X�N���v�g�B
    private TeamFlowScript teamFlow;   //������`�[�������肷��X�N���v�g�B
    public ActiveTeamController activePlayerController;    //�v���C���[����B
    private ChangeSceneScript changeScene;              //�V�[���؂�ւ�����X�N���v�g�B

    public int GAME_FINISH_END = 2;    //1�Q�[���ӂ�̃G���h���B
    private int nowEndNo = 0;               //���݂̃G���h���B

    private bool waitFlag = false;  //������ҋ@����B
    private const float WAIT_TIME = 2.0f;   //�ҋ@���ԁB

    // Start is called before the first frame update
    void Start()
    {
        endFlow = this.gameObject.GetComponent<EndFlowScript>();
        teamFlow = this.gameObject.GetComponent<TeamFlowScript>();
        changeScene = this.gameObject.GetComponent<ChangeSceneScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if(waitFlag)
        {
            return;
        }

        if(endFlow.GetIsEnd() && !waitFlag)
        {
            nowEndNo++;
            waitFlag = true;
            if (nowEndNo < GAME_FINISH_END)
            {
                //�G���h���ăX�^�[�g�B
                Invoke("RestartEnd", WAIT_TIME);
            }
            else
            {
                //�Q�[���I���B
                FinishGame();
            }
        }
        
    }

    /// <summary>
    /// �Q�[�����I�������B
    /// </summary>
    private void FinishGame()
    {
        //�V�[����2�b��؂�ւ���B
        changeScene.ChangeScene(false, WAIT_TIME);
    }

    /// <summary>
    /// �G���h�����X�^�[�g�B
    /// </summary>
    private void RestartEnd()
    {
        if (nowEndNo % 2 == 0)
        {
            teamFlow.SetFirstTeam(Team.Red);
        }
        else
        {
            teamFlow.SetFirstTeam(Team.Blue);
        }
        endFlow.ResetVar();                         //�G���h�i�s��������Z�b�g�B
        activePlayerController.RestartGame();       //�v���C���[�����Z�b�g�B  

        waitFlag = false;
    }
}
