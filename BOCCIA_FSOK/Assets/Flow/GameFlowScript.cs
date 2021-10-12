using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
/// <summary>
/// �Q�[���i�s����
/// �P�D�G���h���̊Ǘ�
/// �Q�D�G���h�I�����̃��Z�b�g
/// �R�D�S�G���h�I�����V�[���J��
/// </summary>

public class GameFlowScript : MonoBehaviour
{
    private SwichActiveGameObjects switchActiveObjs;
    private EndFlowScript endFlow;   //�G���h�i�s����X�N���v�g�B
    private TeamFlowScript teamFlow;   //������`�[�������肷��X�N���v�g�B
    public ActiveTeamController activePlayerController;    //�v���C���[����B
    private ChangeSceneScript changeScene;              //�V�[���؂�ւ�����X�N���v�g�B
    private GameScore.GameScoreScript gameScore;        //�X�R�A�L�^�p�X�N���v�g�B

    [Range(2,6)]public int GAME_FINISH_END = 2;    //1�Q�[���ӂ�̃G���h���B
    private int currentEndNo = 0;               //���݂̃G���h���B

    private bool waitFlag = false;  //������ҋ@����B
    private const float WAIT_TIME = 2.0f;   //�ҋ@���ԁB

    // Start is called before the first frame update
    void Start()
    {
        switchActiveObjs = SwichActiveGameObjects.GetInstance();
        endFlow = this.gameObject.GetComponent<EndFlowScript>();                //�G���h����X�N���v�g�擾�B
        teamFlow = this.gameObject.GetComponent<TeamFlowScript>();              //������`�[�������肷��X�N���v�g�擾�B
        changeScene = this.gameObject.GetComponent<ChangeSceneScript>();        //�V�[���؂�ւ�����X�N���v�g�擾�B
        gameScore = this.gameObject.GetComponent<GameScore.GameScoreScript>();  //�X�R�A�L�^�X�N���v�g�擾�B

        //�ŏI�G���h����ݒ�B
        gameScore.SetFinalEndNum(GAME_FINISH_END);
    }

    // Update is called once per frame
    void Update()
    {
        if(waitFlag)
        {
            return;
        }

        //���̃G���h���I�������B
        if(endFlow.GetCalced() && !waitFlag)
        {
            //���̃G���h�̃��U���g�L�^�B
            RecordEndResult();
            //�G���h����i�߂�B
            currentEndNo++;
            waitFlag = true;
            if (currentEndNo < GAME_FINISH_END)
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
        //else
        //{
        //    //�{�[���������Ă���B
        //    if (teamFlow.GetIsMoving())
        //    {
        //        //����̃I�u�W�F�N�g���~�߂�B
        //        switchActiveObjs.SwitchGameObject(false);
        //    }
        //    else
        //    {
        //        //�{�[���������ĂȂ���Ζ߂��B
        //        switchActiveObjs.SwitchGameObject(true);
        //    }
        //}
        
    }

    /// <summary>
    /// �G���h���U���g���L�^����B
    /// </summary>
    private void RecordEndResult()
    {
        //�����`�[�����擾�B
        var vicTeam = endFlow.GetVictoryTeam();
        GameScore.EndResult endResult = new GameScore.EndResult();      //�L�^����ϐ��B
        //�����`�[������B
        if(vicTeam.GetNearestTeam() == Team.Red)
        {
            endResult.redTeamScore = vicTeam.GetScore();
        }
        else if(vicTeam.GetNearestTeam() == Team.Blue)
        {
            endResult.blueTeamScore = vicTeam.GetScore();
        }
        //�ϐ����L�^�B
        gameScore.RecordResult(endResult, currentEndNo);
    }

    /// <summary>
    /// �Q�[�����I�������B
    /// </summary>
    private void FinishGame()
    {
        //�V�[����؂�ւ���B
        changeScene.ChangeSceneInvoke(false, WAIT_TIME);
        //�V�[���؂�ւ����̏�����ǉ��B
        SceneManager.sceneLoaded += gameScore.SendScoreNextScene;
    }

    /// <summary>
    /// �G���h�����X�^�[�g�B
    /// </summary>
    private void RestartEnd()
    {
        if (currentEndNo % 2 == 0)
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
