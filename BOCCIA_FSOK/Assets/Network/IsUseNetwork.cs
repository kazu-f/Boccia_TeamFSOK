using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsUseNetwork : MonoBehaviour
{
    private bool isUseAI = true;       //AIを使用するかどうか。
    private Team playerTeamCol = Team.Red;      //プレイヤーのチームカラー。


    private void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        //AI戦でなければ追加する。
        if (!isUseAI)
        {
            this.gameObject.AddComponent<Photon.Pun.PhotonView>();
            this.gameObject.AddComponent<NetworkManagerScript>();
            this.gameObject.AddComponent<NetworkSendManagerScript>();
            Debug.Log("通信対戦を開始。");
        }
        else
        {
            Debug.Log("AIとの対戦を開始。");
        }
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
