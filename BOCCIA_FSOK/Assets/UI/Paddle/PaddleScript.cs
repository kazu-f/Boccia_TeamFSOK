using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PaddleScript : MonoBehaviour
{
    private Image image = null;
    private RectTransform rect = null;
    //スプライト
    [SerializeField] private Sprite RedPaddleSprite;
    [SerializeField] private Sprite BluePaddleSprite;
    private Vector3 DefaultScale = Vector3.zero;       //始めの幅と高さ
    [SerializeField] private Vector3 LastScale;        //補完後の幅と高さ
    private float late = 0.0f;      //補完率
    [SerializeField] private float LerpSpeed = 1.0f;        //補完速度
    [SerializeField] private float StopTime = 1.0f;     //停止している時間
    private float NowTime = 0.0f;       //現在の時間
    private bool IsMove = false;        //動くかどうかのフラグ
    private void Awake()
    {
        image = this.gameObject.GetComponent<Image>();
        rect = this.gameObject.GetComponent<RectTransform>();
        DefaultScale = rect.localScale;
    }
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        LerpPos();
    }

    /// <summary>
    /// 次に投げるチームをセットしてスプライトを切り替え
    /// </summary>
    /// <param name="team">次に投げるチーム</param>
    public void SetTeam(Team team)
    {
        switch(team)
        {
            case Team.Red:
                image.sprite = RedPaddleSprite;
                break;
            case Team.Blue:
                image.sprite = BluePaddleSprite;
                break;
        }
        SetDefault();
    }

    /// <summary>
    /// デフォルトにセット
    /// </summary>
    private void SetDefault()
    {
        rect.localScale = DefaultScale;
        late = 0.0f;
        IsMove = false;
    }

    private void LerpPos()
    {
        if (!IsMove)
        {
            NowTime += Time.deltaTime;
            if (NowTime > StopTime)
            {
                IsMove = true;
                NowTime = 0.0f;
            }
            return;
        }
        if (late < 1.0f)
        {
            //補完率を加算
            late += Time.deltaTime * LerpSpeed;
            late = Mathf.Min(late, 1.0f);
            rect.localScale = (late * LastScale) + ((1.0f - late) * DefaultScale);
        }
    }
}
