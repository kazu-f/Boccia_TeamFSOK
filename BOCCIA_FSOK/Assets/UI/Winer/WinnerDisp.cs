using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinnerDisp : MonoBehaviour
{
    public GameObject winnerImageObj;        //画像。
    public RectTransform redTeam;           //赤チームの位置。
    public RectTransform blueTeam;          //青チームの位置。

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetWinnerTeam(Team team)
    {
        winnerImageObj.SetActive(true);
        var rect = winnerImageObj.GetComponent<RectTransform>();
        if (team == Team.Red)
        {
            rect.position = redTeam.position;
        }
        else if (team == Team.Blue)
        {
            rect.position = blueTeam.position;
        }
        else
        {
            //勝利者なし。
            winnerImageObj.SetActive(false);
        }

    }
}
