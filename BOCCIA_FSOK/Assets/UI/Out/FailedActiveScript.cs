using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FailedActiveScript : MonoBehaviour
{
    FailedMoveScript moveFailed = null;
    // Start is called before the first frame update
    void Start()
    {
        moveFailed = GameObject.Find("Failed").GetComponent<FailedMoveScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameObject.Find("GameFlow").GetComponent<TeamFlowScript>().GetState() == TeamFlowState.Delay)
        {
            GameObject jack = GameObject.FindGameObjectWithTag("Jack");
            if (jack == null)
            {
                return;
            }
            if (jack.GetComponent<IBallScript>().GetIsThrow())
            {
                if (!jack.GetComponent<IBallScript>().GetInArea())
                {
                    moveFailed.SetDirect();
                }
            }
            GameObject[] BallList = GameObject.FindGameObjectsWithTag("Ball");
            if (BallList.Length == 0)
            {
                return;
            }

            for (int i = 0; i < BallList.Length; i++)
            {
                if (BallList[i] == null)
                {
                    return;
                }
                if (!BallList[i].GetComponent<IBallScript>().GetInArea())
                {
                    if (BallList[i].GetComponent<IBallScript>().GetIsThrow())
                    {
                        moveFailed.SetDirect();
                    }
                }
            }
        }

    }
}
