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
    private bool isMoving = false;              //スライド中か。
    private bool isInitedUI = false;            //インしている状態か？

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

    }

    // スライドイン（Pauseボタンが押されたときに、これを呼ぶ）
    public void SlideIn()
    {
        StartCoroutine(StartSlidePanel(true));
    }

    // スライドアウト
    public void SlideOut()
    {
        StartCoroutine(StartSlidePanel(false));
    }

    private IEnumerator StartSlidePanel(bool isSlideIn)
    {
        float startTime = Time.time;    // 開始時間
        Vector2 startPos = rectTransform.pivot;  // 開始位置
        Vector2 moveDistance;            // 移動距離および方向

        isMoving = true;
        isInitedUI = false;

        if (isSlideIn)
        {
            moveDistance = (inPivot - startPos);

            while ((Time.time - startTime) < duration)
            {
                rectTransform.pivot = startPos + moveDistance * animCurve.Evaluate((Time.time - startTime) / duration);
                yield return 0;        // 1フレーム後、再開
            }
            //スライドインが終わった。
            isInitedUI = true;
        }
        else
        {
            moveDistance = (outPivot - startPos);

            while ((Time.time - startTime) < duration)
            {
                rectTransform.pivot = startPos + moveDistance * animCurve.Evaluate((Time.time - startTime) / duration);
                yield return 0;        // 1フレーム後、再開
            }
        }
        rectTransform.pivot = startPos + moveDistance;

        isMoving = false;
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
}
