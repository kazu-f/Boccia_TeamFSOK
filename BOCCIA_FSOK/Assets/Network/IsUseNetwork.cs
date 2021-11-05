using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsUseNetwork : MonoBehaviour
{
    private bool isUseAI = true;       //AIを使用するかどうか。
    private Team playerTeamCol = Team.Red;      //プレイヤーのチームカラー。


    private void Awake()
    {
        //AI戦でなければ追加する。
        if(!isUseAI)
        {
            this.gameObject.AddComponent<NetworkManagerScript>();
            this.gameObject.AddComponent<NetworkSendManagerScript>();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool IsUseAI()
    {
        return isUseAI;
    }

    public void SetUseAI(bool flag)
    {
        isUseAI = flag;
    }

    public Team GetPlayerCol()
    {
        return playerTeamCol;
    }

    public void SetTeamCol(Team col)
    {
        playerTeamCol = col;
    }
}
