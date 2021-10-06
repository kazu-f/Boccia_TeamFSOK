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
        enResultSlideIn,
        enStateNum
    }


    [SerializeField] private GameObject canvas;         //�L�����o�X�B
    public GameScore.GameScoreScript scoreScript;      //�X�R�A���L�^���Ă���X�N���v�g�B
    public GameObject resultPrefab;                     //���U���g�̃v���t�@�u�B
    private GameObject[] resultTextsObj;                   //�G���h���̃e�L�X�g�B
    private GameObject resultSumText;                       //���v�X�R�A�̃e�L�X�g�B

    [SerializeField] private Vector3 direction;             //���U���g����ׂ�����B

    private EnScoreDispState state = EnScoreDispState.enUISlideIn;
    private int EndNum = 0;                             //�G���h���B
    private int currentNo = 0;                          //���݃X���C�h�C�����Ă���X�R�A�B

    // Start is called before the first frame update
    void Start()
    {
        //EndNum = scoreScript.GetFinalEndNum();      //�G���h�����擾�B
        EndNum = 3;      //�G���h�����擾�B
        if (EndNum <= 0) return;
        resultTextsObj = new GameObject[EndNum];

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

            //�X�R�A�\���Ƀ��U���g���Z�b�g�B
            var setScore = obj.GetComponent<SetScoreTextScript>();
            setScore.SetEndResult(result, i + 1);

            resultTextsObj[i] = obj;
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
                    //UI�����ԂɃX���C�h�C���B
                    resultTextsObj[currentNo].GetComponent<UISlideIn>().SlideIn();
                    state = EnScoreDispState.enUIWaitSlide;
                }
                else
                {
                    state = EnScoreDispState.enStateNum;
                }
                break;
            case EnScoreDispState.enUIWaitSlide:
                if (!resultTextsObj[currentNo].GetComponent<UISlideIn>().IsMoving())
                {
                    currentNo++;
                    state = EnScoreDispState.enUISlideIn;
                }
                break;
            default:
                break;
        }        
    }

    //�X���C�h����ׂ����������
    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position, direction.normalized * 10f);
        Gizmos.DrawCube(transform.position + direction.normalized * 10.0f, Vector3.one * 3.0f);
    }

}
