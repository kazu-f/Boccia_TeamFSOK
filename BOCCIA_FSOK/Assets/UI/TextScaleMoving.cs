using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextScaleMoving : MonoBehaviour
{
    private Text text = null;
    public float moveTime = 1.0f;       //動きを付ける感覚。
    public float minScale = 0.8f;       //一番小さい時点でのスケール。
    public float maxScale = 1.0f;       //一番大きい時点でのスケール。

    private float currentTime = 0.0f;   //経過時間。
    private Vector3 textScale = new Vector3();  //テキストのスケール。
    private float scale = 1.0f;         //スケール。

    // Start is called before the first frame update
    void Start()
    {
        text = this.gameObject.GetComponent<Text>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if(text == null)
        {
            return;
        }
        //スケールを計算する(0.0～1.0の間の値を時間で取り、最小～最大で線形補間。)
        currentTime += Time.deltaTime;
        float t = (currentTime / moveTime);
        float weight = Mathf.Abs(t - 2 * Mathf.Floor(t / 2.0f) - 1.0f);
        scale = Mathf.Lerp(minScale, maxScale, weight);
        //スケールを設定。
        textScale.Set(scale, scale, scale);
        //テキストのスケールを変更。
        text.rectTransform.localScale = textScale;
    }
}
