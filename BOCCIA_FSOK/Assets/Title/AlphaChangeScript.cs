using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlphaChangeScript : MonoBehaviour
{
    Text text;
    Image image;
    bool isText = false;
    bool isImage = false;

    public float ChangeTime = 2.0f;
    public float alphaMin = 0.0f;
    public float alphaMax = 1.0f;
    float time = 0.0f;
    float alpha = 1.0f;
    // Start is called before the first frame update
    void Start()
    {
        //イメージを取得。
        image = GetComponent<Image>();
        if(image != null)
        {
            //取得できた。
            isImage = true;
        }
        //テキストを取得。
        text = GetComponent<Text>();
        if (text != null)
        {
            //取得できた。
            isText = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        float t = (time / ChangeTime);
        float weight = Mathf.Abs(t - 2 * Mathf.Floor(t / 2.0f) - 1.0f);
        //alpha = 1.0f - (Mathf.Sin(time / ChangeTime) / 2.0f + 0.5f);
        alpha = Mathf.Lerp(alphaMin, alphaMax, weight);

        //スプライト。
        if (isImage)
        {
            var col = image.color;
            col.a = alpha;
            image.color = col;
        }
        //テキスト。
        if (isText)
        {
            var col = text.color;
            col.a = alpha;
            text.color = col;
        }
    }
}
