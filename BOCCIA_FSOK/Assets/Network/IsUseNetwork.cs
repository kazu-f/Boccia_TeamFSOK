using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsUseNetwork : MonoBehaviour
{
    private GameObject sendManagerObj = null;
    private NetworkSendManagerScript sendManager = null;
    private NetworkManagerScript networkManager = null;
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
            //ネットワーク用のオブジェクトを作成。
            sendManagerObj = Photon.Pun.PhotonNetwork.Instantiate("SendNetWorkObj",Vector3.zero,Quaternion.identity);
            if(sendManagerObj != null)
            {
                sendManager = sendManagerObj.GetComponent<NetworkSendManagerScript>();
                networkManager = sendManagerObj.GetComponent<NetworkManagerScript>();
            }

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

    public NetworkSendManagerScript GetSendManager()
    {
        return sendManager;
    }
}
