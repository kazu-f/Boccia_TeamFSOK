using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreResultDispScript : MonoBehaviour
{
    [SerializeField] private GameObject canvas;         //�L�����o�X�B
    private GameScore.GameScoreScript scoreScript;      //�X�R�A���L�^���Ă���X�N���v�g�B
    public GameObject resultPrefab;                     //���U���g�̃v���t�@�u�B
    private GameObject[] resultTextsObj;                   //�G���h���̃e�L�X�g�B

    private int EndNum = 0;                             //�G���h���B

    // Start is called before the first frame update
    void Start()
    {
        EndNum = scoreScript.GetFinalEndNum();      //�G���h�����擾�B
        resultTextsObj = new GameObject[EndNum];

        for(int i = 0; i < EndNum; i++)
        {
            resultTextsObj[i] = Instantiate(resultPrefab);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
