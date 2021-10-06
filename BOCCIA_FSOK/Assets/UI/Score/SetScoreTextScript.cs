using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetScoreTextScript : MonoBehaviour
{
    [SerializeField] private Text redTeamScore;     //赤チームのスコア。
    [SerializeField] private Text blueTeamScore;    //青チームのスコア。

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    /// <summary>
    /// エンドリザルトをセットする。
    /// </summary>
    /// <param name="result">リザルト。</param>
    /// <param name="no">エンド番号。</param>
    public void SetEndResult(GameScore.EndResult result,int no)
    {
        redTeamScore.text = result.redTeamScore.ToString();
        blueTeamScore.text = result.blueTeamScore.ToString();
    }
}
