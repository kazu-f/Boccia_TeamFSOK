using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISlideIn : MonoBehaviour
{
    public AnimationCurve animCurve = AnimationCurve.Linear(0, 0, 1, 1);    //アニメーションカーブ。
    [SerializeField]private Vector3 direction;           //スライド方向。
    [SerializeField] private float duration = 1.0f;    // スライド時間（秒）
    private RectTransform rect;    // UIのRect
    private Vector3 inPosition;        // スライドイン後の位置
    private Vector3 outPosition;      // スライドアウト後の位置
    private bool isMoving = false;              //スライド中か。
    private bool isInited = false;              //座標設定済みか？
    // Start is called before the first frame update
    void Start()
    {
        rect = this.gameObject.GetComponent<RectTransform>();
        Vector3 dir = direction.normalized;
        dir.x *= Screen.width + rect.sizeDelta.x;
        dir.z *= Screen.height + rect.sizeDelta.y;

        inPosition = transform.localPosition;           //スライドイン後の位置。
        outPosition = transform.localPosition + dir;    //スライドアウト後の位置。
        transform.localPosition = transform.localPosition - dir;

        isInited = true;
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
        Vector3 startPos = transform.localPosition;  // 開始位置
        Vector3 moveDistance;            // 移動距離および方向

        isMoving = true;

        if (isSlideIn)
        {
            moveDistance = (inPosition - startPos);

            while ((Time.time - startTime) < duration)
            {
                transform.localPosition = startPos + moveDistance * animCurve.Evaluate((Time.time - startTime) / duration);
                yield return 0;        // 1フレーム後、再開
            }
        }
        else
        {
            moveDistance = (outPosition - startPos);

            while ((Time.time - startTime) < duration)
            {
                transform.localPosition = startPos + moveDistance * animCurve.Evaluate((Time.time - startTime) / duration);
                yield return 0;        // 1フレーム後、再開
            }
        }
        transform.localPosition = startPos + moveDistance;

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
    /// 座標設定済みか？
    /// </summary>
    /// <returns></returns>
    public bool IsInited()
    {
        return isInited;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, direction.normalized * 10f);
        Gizmos.DrawCube(transform.position + direction.normalized * 10.0f, Vector3.one * 3.0f);
    }
}
