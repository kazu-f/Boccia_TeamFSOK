using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// ゲーム進行制御
/// １．エンド数の管理
/// ２．エンド終了時のリセット
/// ３．全エンド終了時シーン遷移
/// </summary>

public class GameFlowScript : MonoBehaviour
{
    private EndFlowScript endFlow;   //エンド進行制御スクリプト。
    private TeamFlowScript teamFlow;   //投げるチームを決定するスクリプト。

    public int GAME_END_NUM = 2;    //1ゲーム辺りのエンド数。
    private int nowEndNo = 0;               //現在のエンド数。

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
