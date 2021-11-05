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
            //àÍâûç≈í·ílåàÇﬂÇ∆Ç≠
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
            if(timenum < Limit/4)
            {
                time.color = Color.red;
                return;
            }
            else if(timenum < Limit /2)
            {
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
    }
}
