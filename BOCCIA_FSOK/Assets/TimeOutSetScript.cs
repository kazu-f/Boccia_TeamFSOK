using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeOutSetScript : MonoBehaviour
{
    [SerializeField] private int SleepTime = -1;
    // Start is called before the first frame update
    void Start()
    {
        //SleepTime��-1�̎��̓X���[�v���Ȃ��B
        Screen.sleepTimeout = SleepTime;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
