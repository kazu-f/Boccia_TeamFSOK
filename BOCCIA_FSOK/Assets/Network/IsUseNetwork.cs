using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IsUseNetwork : MonoBehaviour
{
    [SerializeField]private GameObject sendManagerObj = null;
    private NetworkSendManagerScript sendManager = null;
    private NetworkManagerScript networkManager = null;
    private bool isUseAI = true;       //AIを使用するかどうか。
    private Team playerTeamCol = Team.Red;      //プレイヤーのチームカラー。
    private int playerNo = -1;                  //プレイヤー番号。

    private void Awake()
    {
        playerNo = Photon.Pun.PhotonNetwork.LocalPlayer.ActorNumber;
        if (playerNo == 1)
        {
            playerTeamCol = Team.Red;
        }
        else if (playerNo == 2)
        {
            playerTeamCol = Team.Blue;
        }
        else
        {
            Debug.LogError("プレイヤー番号の値が不正です。");
        }

        //オフラインモードだったらAIを使用する。
        isUseAI = Photon.Pun.PhotonNetwork.OfflineMode;
    }

    // Start is called before the first frame update
    void Start()
    {
        //AI戦でなければ追加する。
        if (!isUseAI)
        {
            //ネットワーク用のスクリプト取得。
            if (sendManagerObj != null)
            {
                sendManager = sendManagerObj.GetComponent<NetworkSendManagerScript>();
                networkManager = sendManagerObj.GetComponent<NetworkManagerScript>();
            }
            Debug.Log("通信対戦を開始。");
        }
        else
        {
            ////ネット関係削除。
            //Destroy(sendManagerObj);
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

    public Team GetPlayerCol()
    {
        return playerTeamCol;
    }

    public NetworkSendManagerScript GetSendManager()
    {
        return sendManager;
    }
}
