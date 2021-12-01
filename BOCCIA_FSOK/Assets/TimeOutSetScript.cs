using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeOutSetScript : MonoBehaviour
{
    [SerializeField] private int SleepTime = -1;
    // Start is called before the first frame update
    void Start()
    {
        //SleepTimeが-1の時はスリープしない。
        Screen.sleepTimeout = SleepTime;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
