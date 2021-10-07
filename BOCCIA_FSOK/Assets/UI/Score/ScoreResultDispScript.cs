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
        enFinish,
        enStateNum
    }


    [SerializeField] private GameObject canvas;         //�L�����o�X�B
    [SerializeField] private TouchManager touchManager;         //�L�����o�X�B
    public GameScore.GameScoreScript scoreScript;      //�X�R�A���L�^���Ă���X�N���v�g�B
    public GameObject resultPrefab;                     //���U���g�̃v���t�@�u�B
    public GameObject resultSumPrefab;                     //���U���g�̃v���t�@�u�B
    public GameObject tapGoTitle;                           //�^�C�g���֖߂�B
    private GameObject[] resultTextsObj;                   //�G���h���̃e�L�X�g�B
    private GameObject resultSumText;                       //���v�X�R�A�̃e�L�X�g�B
    private ChangeSceneScript changeScene;                  //�V�[���؂�ւ�����B

    [SerializeField] private Vector3 direction;             //���U���g����ׂ�����B

    private EnScoreDispState state = EnScoreDispState.enUISlideIn;
    private int EndNum = 0;                             //�G���h���B
    private int currentNo = 0;                          //���݃X���C�h�C�����Ă���X�R�A�B
    bool isFinish = false;                              //�I���B

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

        GameScore.EndResult sumScore = new GameScore.EndResult();

        Vector3 lastPos;

        for(int i = 0; i < EndNum; i++)
        {
            //�I�u�W�F�N�g�쐬�B
            var obj = Instantiate(resultPrefab,canvas.transform);

            var rect = obj.GetComponent<RectTransform>();

            //�I�u�W�F�N�g�̈ʒu�����炷�B
            Vector3 posDist = direction.normalized;
            posDist.x *= rect.sizeDelta.x * i * 2.0f;
            posDist.y *= rect.sizeDelta.y * i * 2.0f;

            obj.transform.localPosition = this.transform.localPosition + canvas.transform.TransformVector(posDist);

            //���U���g�擾�B
            GameScore.EndResult result = scoreScript.GetEndResult(i);
            sumScore = sumScore + result;

            //�X�R�A�\���Ƀ��U���g���Z�b�g�B
            var setScore = obj.GetComponent<SetScoreTextScript>();
            setScore.SetEndResult(result);

            resultTextsObj[i] = obj;
        }
        //��ԉ��̍��W�B
        lastPos = resultTextsObj[EndNum - 1].transform.localPosition;
        //�X�R�A���v
        {
            resultSumText = Instantiate(resultSumPrefab, canvas.transform);

            var rect = resultSumText.GetComponent<RectTransform>();

            //�I�u�W�F�N�g�̈ʒu�����炷�B
            Vector3 posDist = direction.normalized;
            posDist.x *= rect.sizeDelta.x * 2.0f;
            posDist.y *= rect.sizeDelta.y * 2.0f;
            //�e�L�X�g�̈ʒu��ݒ�B
            resultSumText.transform.localPosition = lastPos + canvas.transform.TransformVector(posDist);

            //���U���g�X�R�A���Z�b�g�B
            resultSumText.GetComponent<SetScoreTextScript>().SetEndResult(sumScore);

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
                Invoke("DispResultSum", 1.0f);
                state = EnScoreDispState.enFinish;

                break;

            case EnScoreDispState.enFinish:
                if(isFinish)
                {
                    tapGoTitle.SetActive(true);
                    if(touchManager.IsTouch())
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
    private void DispResultSum()
    {
        isFinish = true;
        resultSumText.SetActive(true);
    }

    //�X���C�h����ׂ����������
    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position, direction.normalized * 10f);
        Gizmos.DrawCube(transform.position + direction.normalized * 10.0f, Vector3.one * 3.0f);
    }

}