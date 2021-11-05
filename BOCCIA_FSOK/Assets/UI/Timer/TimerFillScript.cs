using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TimerFillScript : MonoBehaviour
{
    [SerializeField] private float Limit = 30.0f;
    private float NowTime = 0.0f;
    private bool IsStart = false;
    private float late = 1.0f;
    [SerializeField]private GameObject CircleBefore = null;
    private Image CircleImage = null;
    [SerializeField] private Text time = null;
    private void Awake()
    {
        CircleImage = CircleBefore.GetComponent<Image>();
    }
    // Start is called before the first frame update
    void Start()
    {
        TimerStart();
    }

    // Update is called once per frame
    void Update()
    {
        if(IsStart)
        {
            NowTime -= Time.deltaTime;
            //0äÑÇËÇ™î≠ê∂ÇµÇ»Ç¢ÇΩÇﬂÇ…ç≈í·ílÇåàÇﬂÇ∆Ç≠
            NowTime = Mathf.Max(NowTime, -0.01f);
            late = NowTime / Limit;
            CircleImage.fillAmount = late;
            if(late < 0.0f)
            {
                IsStart = false;
            }
            //êÿÇËè„Ç∞
            int timenum = Mathf.CeilToInt(NowTime);
            time.text = "" + timenum;
        }
    }

    public void TimerStart()
    {
        NowTime = Limit;
        IsStart = true;
        late = 1.0f;
    }
}
