using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamFlowDelayScript : MonoBehaviour
{
    private GameObject Failed = null;
    [SerializeField] private float AfterEndTime = 3.0f;
    private float AfterNowTime = 0.0f;
    private bool IsStart = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (IsStart)
        {
            //遅延させる
            //遅延中の場合はフラグを立て続ける
            IsStart = !EndAfterTime();
        }
    }

    /// <summary>
    /// 遅延
    /// </summary>
    /// <returns>遅延が終了したかどうか</returns>
    private bool EndAfterTime()
    {
        if (AfterNowTime < AfterEndTime)
        {
            //遅延中
            AfterNowTime += Time.deltaTime;
            return false;
        }
        else
        {
            Failed = GameObject.Find("Failed");
            Failed.GetComponent<FailedMoveScript>().FontAlphaZero();
            //遅延終了
            AfterNowTime = 0.0f;
            return true;
        }
    }

    /// <summary>
    /// 遅延開始させる
    /// </summary>
    public void DelayStart()
    {
        IsStart = true;
    }

    /// <summary>
    /// 遅延中かどうか
    /// </summary>
    /// <returns>遅延中ならスタートフラグが立っている</returns>
    public bool IsDelay()
    {
        return IsStart;
    }
}
