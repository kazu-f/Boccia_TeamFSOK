using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreResultDispScript : MonoBehaviour
{
    [SerializeField] private GameObject canvas;         //キャンバス。
    private GameScore.GameScoreScript scoreScript;      //スコアを記録しているスクリプト。
    public GameObject resultPrefab;                     //リザルトのプレファブ。
    private GameObject[] resultTextsObj;                   //エンド毎のテキスト。

    private int EndNum = 0;                             //エンド数。

    // Start is called before the first frame update
    void Start()
    {
        EndNum = scoreScript.GetFinalEndNum();      //エンド数を取得。
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
