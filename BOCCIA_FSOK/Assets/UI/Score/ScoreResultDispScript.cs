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


    [SerializeField] private GameObject canvas;         //キャンバス。
    [SerializeField] private TouchManager touchManager;         //キャンバス。
    public GameScore.GameScoreScript scoreScript;      //スコアを記録しているスクリプト。
    public GameObject resultPrefab;                     //リザルトのプレファブ。
    public GameObject resultSumPrefab;                     //リザルトのプレファブ。
    public GameObject tapGoTitle;                           //タイトルへ戻る。
    private GameObject[] resultTextsObj;                   //エンド毎のテキスト。
    private GameObject resultSumText;                       //合計スコアのテキスト。
    private ChangeSceneScript changeScene;                  //シーン切り替え制御。

    [SerializeField] private Vector3 direction;             //リザルトを並べる方向。

    private EnScoreDispState state = EnScoreDispState.enUISlideIn;
    private int EndNum = 0;                             //エンド数。
    private int currentNo = 0;                          //現在スライドインしているスコア。
    bool isFinish = false;                              //終了。

    // Start is called before the first frame update
    void Start()
    {
        //非表示。
        tapGoTitle.SetActive(false);
        //シーン切り替え制御。
        changeScene = this.gameObject.GetComponent<ChangeSceneScript>();

        var canvasRect = canvas.GetComponent<RectTransform>();

        EndNum = scoreScript.GetFinalEndNum();      //エンド数を取得。

        if (EndNum <= 0) return;
        resultTextsObj = new GameObject[EndNum];

        GameScore.EndResult sumScore = new GameScore.EndResult();

        Vector3 lastPos;

        for(int i = 0; i < EndNum; i++)
        {
            //オブジェクト作成。
            var obj = Instantiate(resultPrefab,canvas.transform);

            var rect = obj.GetComponent<RectTransform>();

            //オブジェクトの位置をずらす。
            Vector3 posDist = direction.normalized;
            posDist.x *= rect.sizeDelta.x * i;
            posDist.y *= rect.sizeDelta.y * i;

            obj.transform.position = this.transform.position + canvas.transform.TransformVector(posDist);

            //リザルト取得。
            GameScore.EndResult result = scoreScript.GetEndResult(i);
            sumScore = sumScore + result;

            //スコア表示にリザルトをセット。
            var setScore = obj.GetComponent<SetScoreTextScript>();
            setScore.SetEndResult(result);

            resultTextsObj[i] = obj;
        }
        //一番下の座標。
        lastPos = resultTextsObj[EndNum - 1].transform.position;
        //スコア合計
        {
            resultSumText = Instantiate(resultSumPrefab, canvas.transform);

            var rect = resultSumText.GetComponent<RectTransform>();

            //オブジェクトの位置をずらす。
            Vector3 posDist = direction.normalized;
            posDist.x *= rect.sizeDelta.x;
            posDist.y *= rect.sizeDelta.y;
            //テキストの位置を設定。
            resultSumText.transform.position = lastPos + canvas.transform.TransformVector(posDist);

            //リザルトスコアをセット。
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
                        //UIを順番にスライドイン。
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
                    if(touchManager.GetPhase() == TouchPhase.Ended)
                    {
                        //シーン切り替え。
                        changeScene.ChangeScene(false);
                    }
                }

                break;
            default:
                break;
        }        
    }

    /// <summary>
    /// スコア合計表示。
    /// </summary>
    private void DispResultSum()
    {
        isFinish = true;
        if(resultSumText != null) resultSumText.SetActive(true);
    }

    //スライドを並べる方向を可視化
    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position, direction.normalized * 10f);
        Gizmos.DrawCube(transform.position + direction.normalized * 10.0f, Vector3.one * 3.0f);
    }

}
