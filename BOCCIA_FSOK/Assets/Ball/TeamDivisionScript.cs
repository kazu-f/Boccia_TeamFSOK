using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamDivisionScript : MonoBehaviour
{
    public enum Team
    {
        Red,
        Blue,
        Jack,
        Num
    }
    public Team m_Team;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Team GetTeam()
    {
        return m_Team;
    }
}
