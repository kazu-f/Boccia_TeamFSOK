using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamFlowScript : MonoBehaviour
{
    public Team m_team = Team.Num;
    // Start is called before the first frame update
    void Start()
    {
        m_team = Team.Red;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Team GetNowTeam()
    {
        return m_team;
    }
}
