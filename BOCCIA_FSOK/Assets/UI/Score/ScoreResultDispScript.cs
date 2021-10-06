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


    [SerializeField] private GameObject canvas;         //キャンバス。
    public GameScore.GameScoreScript scoreScript;      //スコアを記録しているスクリプト。
    public GameObject resultPrefab;                     //リザルトのプレファブ。
    private GameObject[] resultTextsObj;                   //エンド毎のテキスト。
    private GameObject resultSumText;                       //合計スコアのテキスト。

    [SerializeField] private Vector3 direction;             //リザルトを並べる方向。

    private EnScoreDispState state = EnScoreDispState.enUISlideIn;
    private int EndNum = 0;                             //エンド数。
    private int currentNo = 0;                          //現在スライドインしているスコア。

    // Start is called before the first frame update
    void Start()
    {
        //EndNum = scoreScript.GetFinalEndNum();      //エンド数を取得。
        EndNum = 3;      //エンド数を取得。
        if (EndNum <= 0) return;
        resultTextsObj = new GameObject[EndNum];

        for(int i = 0; i < EndNum; i++)
        {
            //オブジェクト作成。
            var obj = Instantiate(resultPrefab,canvas.transform);

            var rect = obj.GetComponent<RectTransform>();

            //オブジェクトの位置をずらす。
            Vector3 posDist = direction.normalized;
            posDist.x *= rect.sizeDelta.x * i * 2.0f;
            posDist.y *= rect.sizeDelta.y * i * 2.0f;

            obj.transform.localPosition = this.transform.localPosition + canvas.transform.TransformVector(posDist);

            //リザルト取得。
            GameScore.EndResult result = scoreScript.GetEndResult(i);

            //スコア表示にリザルトをセット。
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
                    //UIを順番にスライドイン。
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

    //スライドを並べる方向を可視化
    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position, direction.normalized * 10f);
        Gizmos.DrawCube(transform.position + direction.normalized * 10.0f, Vector3.one * 3.0f);
    }

}
