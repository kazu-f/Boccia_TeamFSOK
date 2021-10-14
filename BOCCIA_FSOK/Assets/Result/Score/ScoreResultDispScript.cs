using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreResultDispScript : MonoBehaviour
{
    enum EnScoreDispState
    {
        enUISlideIn,
        enUIWaitSlide,
        enResultSumDisp,
        enWinLoseEffect,
        enFinish,
        enStateNum
    }


    [SerializeField] private GameObject canvas;         //�L�����o�X�B
    [SerializeField] private TouchManager touchManager;         //�L�����o�X�B
    [SerializeField] private ResultSoundController soundController;         //�T�E���h�R���g���[���B
    public GameScore.GameScoreScript scoreScript;      //�X�R�A���L�^���Ă���X�N���v�g�B
    public GameObject resultPrefab;                     //���U���g�̃v���t�@�u�B
    public GameObject resultSumPrefab;                     //���U���g�̃v���t�@�u�B
    public GameObject tapGoTitle;                           //�^�C�g���֖߂�B
    public GameObject winnerParticle;                       //�p�[�e�B�N���B
    private GameObject[] resultTextsObj;                   //�G���h���̃e�L�X�g�B
    private GameObject resultSumText;                       //���v�X�R�A�̃e�L�X�g�B
    private ChangeSceneScript changeScene;                  //�V�[���؂�ւ�����B

    [SerializeField] private Vector3 direction;             //���U���g����ׂ�����B

    private GameScore.EndResult sumScore = new GameScore.EndResult();       //���U���g�X�R�A�B

    private EnScoreDispState state = EnScoreDispState.enUISlideIn;
    private int EndNum = 0;                             //�G���h���B
    private int currentNo = 0;                          //���݃X���C�h�C�����Ă���X�R�A�B
    //bool isWait = false;                                //�ҋ@�����邩�B
    bool isFinish = false;                              //�I���B
    const float WAIT_TIME = 1.0f;                       //���o���̑ҋ@���ԁB

    // Start is called before the first frame update
    void Start()
    {
        //��\���B
        tapGoTitle.SetActive(false);
        //�V�[���؂�ւ�����B
        changeScene = this.gameObject.GetComponent<ChangeSceneScript>();

        EndNum = scoreScript.GetFinalEndNum();      //�G���h�����擾�B

        if (EndNum <= 0) return;
        resultTextsObj = new GameObject[EndNum];

        Vector3 lastPos;

        for(int i = 0; i < EndNum; i++)
        {
            //�I�u�W�F�N�g�쐬�B
            var obj = Instantiate(resultPrefab,canvas.transform);

            var rect = obj.GetComponent<RectTransform>();

            //�I�u�W�F�N�g�̈ʒu�����炷�B
            Vector3 posDist = direction.normalized;
            posDist.x *= rect.sizeDelta.x * i;
            posDist.y *= rect.sizeDelta.y * i;

            obj.transform.position = this.transform.position + canvas.transform.TransformVector(posDist);

            //���U���g�擾�B
            GameScore.EndResult result = scoreScript.GetEndResult(i);
            sumScore = sumScore + result;

            //�X�R�A�\���Ƀ��U���g���Z�b�g�B
            var setScore = obj.GetComponent<SetScoreTextScript>();
            setScore.SetEndResult(result);

            resultTextsObj[i] = obj;
        }
        //��ԉ��̍��W�B
        lastPos = resultTextsObj[EndNum - 1].transform.position;
        //�X�R�A���v
        {
            resultSumText = Instantiate(resultSumPrefab, canvas.transform);

            var rect = resultSumText.GetComponent<RectTransform>();

            //�I�u�W�F�N�g�̈ʒu�����炷�B
            Vector3 posDist = direction.normalized;
            posDist.x *= rect.sizeDelta.x;
            posDist.y *= rect.sizeDelta.y;
            //�e�L�X�g�̈ʒu��ݒ�B
            resultSumText.transform.position = lastPos + canvas.transform.TransformVector(posDist);

            //���U���g�X�R�A���Z�b�g�B
            resultSumText.GetComponentInChildren<SetScoreTextScript>().SetEndResult(sumScore);

            resultSumText.SetActive(false);
        }

    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case EnScoreDispState.enUISlideIn:
                if (currentNo < EndNum)
                {
                    var uiSlideIn = resultTextsObj[currentNo].GetComponent<UISlideIn>();
                    if(uiSlideIn.IsInited())
                    {
                        //UI�����ԂɃX���C�h�C���B
                        uiSlideIn.SlideIn();
                        state = EnScoreDispState.enUIWaitSlide;
                    }
                }
                else
                {
                    state = EnScoreDispState.enResultSumDisp;
                }
                break;
            case EnScoreDispState.enUIWaitSlide:
                if (!resultTextsObj[currentNo].GetComponent<UISlideIn>().IsMoving())
                {
                    currentNo++;
                    state = EnScoreDispState.enUISlideIn;
                }
                break;

            case EnScoreDispState.enResultSumDisp:
                StartCoroutine(DispResultSum());

                break;
            case EnScoreDispState.enWinLoseEffect:
                StartCoroutine(WinOrLoseEffect());

                break;
            case EnScoreDispState.enFinish:
                if(isFinish)
                {
                    tapGoTitle.SetActive(true);
                    if(touchManager.IsTouch() && 
                        touchManager.GetPhase() == TouchInfo.Ended)
                    {
                        //�V�[���؂�ւ��B
                        changeScene.ChangeScene(false);
                    }
                }

                break;
            default:
                break;
        }        
    }

    /// <summary>
    /// �X�R�A���v�\���B
    /// </summary>
    private IEnumerator DispResultSum()
    {
        soundController.SetDrumRollLoop(false);
        while(!soundController.IsEndDrummRoll())
        {
            yield return null;
        }
        //yield return new WaitForSeconds(WAIT_TIME);
        soundController.PlayCymbal();
        if(resultSumText != null) resultSumText.SetActive(true);
        state = EnScoreDispState.enWinLoseEffect;      //�X�e�[�g�i�s�B
        ////�ҋ@��Ԃ������B
        //isWait = false;
    }

    /// <summary>
    /// ���s���o�B
    /// </summary>
    private IEnumerator WinOrLoseEffect()
    {
        yield return new WaitForSeconds(WAIT_TIME);
        var winDisp = resultSumText.GetComponentInChildren<WinnerDisp>();
        var winText = resultSumText.GetComponentInChildren<WinnerTextEffect>();
        var particleScript = winnerParticle.GetComponent<ResultParticleScript>();
        //�����҂��Z�b�g�B
        if(sumScore.redTeamScore > sumScore.blueTeamScore)
        {
            winDisp.SetWinnerTeam(Team.Red);
            winText.EnableWinnerTeam(Team.Red);
            particleScript.PlayWinnerParticle(Team.Red);
        }
        else if (sumScore.redTeamScore < sumScore.blueTeamScore)
        {
            winDisp.SetWinnerTeam(Team.Blue);
            winText.EnableWinnerTeam(Team.Blue);
            particleScript.PlayWinnerParticle(Team.Blue);
        }
        else
        {
            winDisp.SetWinnerTeam(Team.Num);
            winText.EnableWinnerTeam(Team.Num);
            particleScript.PlayWinnerParticle(Team.Num);
        }

        state = EnScoreDispState.enFinish;      //�X�e�[�g�i�s�B
        //�I���t���O�𗧂Ă�B
        isFinish = true;
        ////�ҋ@��Ԃ������B
        //isWait = false;
    }
    //�X���C�h����ׂ����������
    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position, direction.normalized * 10f);
        Gizmos.DrawCube(transform.position + direction.normalized * 10.0f, Vector3.one * 3.0f);
    }

}
