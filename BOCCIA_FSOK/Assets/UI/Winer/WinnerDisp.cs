using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinnerDisp : MonoBehaviour
{
    public GameObject winnerImageObj;        //�摜�B
    public RectTransform redTeam;           //�ԃ`�[���̈ʒu�B
    public RectTransform blueTeam;          //�`�[���̈ʒu�B

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
            //�����҂Ȃ��B
            winnerImageObj.SetActive(false);
        }

    }
}
