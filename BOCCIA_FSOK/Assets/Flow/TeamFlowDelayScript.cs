using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamFlowDelayScript : MonoBehaviour
{
    [SerializeField] private float AfterEndTime = 3.0f;
    private float AfterNowTime = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (EndAfterTime() == false)
        {
            return;
        }
    }

    private bool EndAfterTime()
    {
        if (AfterNowTime < AfterEndTime)
        {
            AfterNowTime += Time.deltaTime;
            return false;
        }
        else
        {
            AfterNowTime = 0.0f;
            return true;
        }
    }

}
