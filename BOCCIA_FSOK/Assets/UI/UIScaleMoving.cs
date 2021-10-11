using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIScaleMoving : MonoBehaviour
{
    private RectTransform rect = null;
    public float moveTime = 1.0f;       //動きを付ける感覚。
    public float minScale = 0.8f;       //一番小さい時点でのスケール。
    public float maxScale = 1.0f;       //一番大きい時点でのスケール。

    private float currentTime = 0.0f;   //経過時間。
    private Vector3 vScale = new Vector3();  //スケール。
    private float scale = 1.0f;         //スケール。

    // Start is called before the first frame update
    void Start()
    {
        rect = this.gameObject.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if(rect == null)
        {
            return;
        }
        //スケールを計算する(0.0～1.0の間の値を時間で取り、最小～最大で線形補間。)
        currentTime += Time.deltaTime;
        float t = (currentTime / moveTime);
        float weight = Mathf.Abs(t - 2 * Mathf.Floor(t / 2.0f) - 1.0f);
        scale = Mathf.Lerp(minScale, maxScale, weight);
        //スケールを設定。
        vScale.Set(scale, scale, scale);
        //テキストのスケールを変更。
        rect.localScale = vScale;
    }
}
