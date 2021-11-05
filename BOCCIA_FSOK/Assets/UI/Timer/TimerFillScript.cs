using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TimerFillScript : MonoBehaviour
{
    [SerializeField] private int Limit = 30;
    private float NowTime = 0.0f;
    private bool IsStart = false;
    private float late = 1.0f;
    [SerializeField]private GameObject CircleBefore = null;
    [SerializeField] private GameObject CircleAfter = null;
    private Image CircleBeforeImage = null;
    private Image CircleAfterImage = null;
    [SerializeField] private Text time = null;
    private bool IsTimeUped = false;
    private void Awake()
    {
        CircleBeforeImage = CircleBefore.GetComponent<Image>();
        CircleAfterImage = CircleAfter.GetComponent<Image>();
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
            //àÍâûç≈í·ílåàÇﬂÇ∆Ç≠
            NowTime = Mathf.Max(NowTime, -0.01f);
            late = NowTime / Limit;
            CircleBeforeImage.fillAmount = late;
            if(late < 0.0f)
            {
                IsStart = false;
                IsTimeUped = true;
            }
            //êÿÇËè„Ç∞
            int timenum = Mathf.CeilToInt(NowTime);
            time.text = "" + timenum;
            if(timenum < Limit/4)
            {
                time.color = Color.red;
                CircleAfterImage.color = Color.red;
                return;
            }
            else if(timenum < Limit /2)
            {
                CircleAfterImage.color = Color.yellow;
                time.color = Color.yellow;
                return;
            }
        }
    }

    public void TimerStart()
    {
        NowTime = Limit;
        IsStart = true;
        late = 1.0f;
        time.color = Color.green;
        CircleAfterImage.color = Color.green;
        IsTimeUped = false;
    }

    public bool IsTimeUp()
    {
        return IsTimeUped;
    }
}
