using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinnerTextEffect : MonoBehaviour
{
    [SerializeField] private UIScaleMoving redTeam;
    [SerializeField] private UIScaleMoving blueTeam;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EnableWinnerTeam(Team team)
    {
        if(team == Team.Red)
        {
            redTeam.enabled = true;
        }
        else if(team == Team.Blue)
        {
            blueTeam.enabled = true;
        }
    }
}
