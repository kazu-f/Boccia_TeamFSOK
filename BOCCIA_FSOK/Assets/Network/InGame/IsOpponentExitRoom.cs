using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon;
using Photon.Pun;
using Photon.Realtime;

//対戦相手がルームから抜けたかどうかを監視するスクリプト。
public class IsOpponentExitRoom : MonoBehaviourPunCallbacks
{
    #region SerializeField
    [Tooltip("切断されたことを通知するオブジェクト。")]
    [SerializeField] private GameObject opponentExit = null;
    #endregion

    #region Private Properties
    private GameFlowScript gameFlow = null;     //ゲーム進行管理オブジェクト。
    #endregion

    #region Callbacks

    // 誰かが抜けた。
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        // 抜けた時はここ
        base.OnPlayerLeftRoom(otherPlayer);
        //切断されたことを通知する。
        if(opponentExit && !gameFlow.isFinishGame)
        {
            opponentExit.SetActive(true);
        }
        
        if (PhotonNetwork.IsConnected)
        {
            //部屋から抜ける。
            PhotonNetwork.LeaveRoom();
            PhotonNetwork.Disconnect();
        }
    }
    // 切断された。
    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);
        if(opponentExit && !gameFlow.isFinishGame)
        {
            opponentExit.SetActive(true);
        }
    }

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        //ゲーム管理オブジェクト取得。
        var Obj = GameObject.FindGameObjectWithTag("GameFlow");
        if (Obj != null)
        {
            gameFlow = Obj.GetComponent<GameFlowScript>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnApplicationPause(bool pause)
    {
        if(pause)
        {
            //切断されたことを通知する。
            if (opponentExit && !gameFlow.isFinishGame)
            {
                opponentExit.SetActive(true);
            }

            if (PhotonNetwork.IsConnected)
            {
                //部屋から抜ける。
                PhotonNetwork.LeaveRoom();
                PhotonNetwork.Disconnect();
            }
        }
    }

    //ゲームが終わる時にはネットから切断しておく。
    private void OnDestroy()
    {
        if (PhotonNetwork.IsConnected)
        {
            //部屋から抜ける。
            PhotonNetwork.LeaveRoom();
            PhotonNetwork.Disconnect();
        }
    }
}
