using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovePanelSlide : MonoBehaviour
{
    [SerializeField]private AnimationCurve animCurve;
    [SerializeField] private float duration = 1.0f;    // スライド時間（秒）
    private RectTransform rectTransform;    //UIのRect。
    private Vector2 inPivot = new Vector2(0.0f,0.5f);                //スライドイン後のピボット位置
    private Vector2 outPivot = new Vector2(1.0f, 0.5f);               //スライドアウト後のピボット位置
    private Vector2 startPos = new Vector2(0.0f, 0.0f); //スライド開始地点。
    private Vector2 moveDistance;            // 移動距離および方向
    float startTime = 0.0f;                     //開始時間。
    private bool isMoving = false;              //スライド中か。
    private bool isInitedUI = false;            //インしている状態か？
    private bool isExecuteSlideIn = false;             //スライドインをしているか？

    private void Awake()
    {
        rectTransform = this.gameObject.GetComponent<RectTransform>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ExecuteSlide();
    }

    // スライドインを開始する。
    public void SlideIn()
    {
        StartSlidePanel(true);
    }

    // スライドアウトを開始する。
    public void SlideOut()
    {
        StartSlidePanel(false);
    }

    private void StartSlidePanel(bool isSlideIn)
    {
        startTime = Time.time;    // 開始時間
        startPos = rectTransform.pivot;  // 開始位置

        isMoving = true;                //スライドし始める。
        isInitedUI = false;             //UIが動いている。
        isExecuteSlideIn = isSlideIn;   //スライドインを実行しているか。

        if (isSlideIn)
        {
            moveDistance = (inPivot - startPos);
        }
        else
        {
            moveDistance = (outPivot - startPos);
        }
    }

    private void ExecuteSlide()
    {
        //スライドしていないなら処理しない。
        if (!isMoving) return;

        //スライド時間に到達していない。。
        if ((Time.time - startTime) < duration)
        {
            rectTransform.pivot = startPos + moveDistance * animCurve.Evaluate((Time.time - startTime) / duration);
        }
        else
        {
            rectTransform.pivot = startPos + moveDistance;

            //スライドが終わった。
            isMoving = false;
            //スライドインが終わったか？
            isInitedUI = isExecuteSlideIn;
        }
    }

    /// <summary>
    /// スライド中かどうか。
    /// </summary>
    /// <returns></returns>
    public bool IsMoving()
    {
        return isMoving;
    }
    /// <summary>
    /// スライドインした後の状態か。
    /// </summary>
    /// <returns></returns>
    public bool IsInitedUI()
    {
        return isInitedUI;
    }

    private void OnEnable()
    {
        //有効になった瞬間にもスライドを実行しておく。
        ExecuteSlide();
    }
}
