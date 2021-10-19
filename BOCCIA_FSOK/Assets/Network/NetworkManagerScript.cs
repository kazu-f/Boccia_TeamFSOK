using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class NetworkManagerScript : MonoBehaviourPunCallbacks
{
    #region Private Methods
    /// <summary>
    /// ゲームシーンをロードする
    /// </summary>
    void LoadGameScene()
    {
        if(!PhotonNetwork.IsMasterClient)
        {
            //マスタークライアントではないとき
            Debug.LogError("レベルを読み込もうとしましたが、あなたはルームマスターではありません");
        }
        Debug.LogFormat("PhotonNetwork : Loading Level : {0}", PhotonNetwork.CurrentRoom.PlayerCount);
        //ゲームシーンを読み込む
        PhotonNetwork.LoadLevel("BocciaGameScene");
    }

    #endregion

    #region Photon Callbacks

    /// <summary>
    /// プレイヤーがルームに参加したときに呼ばれる
    /// </summary>
    /// <param name="other">参加したプレイヤー</param>
    public override void OnPlayerEnteredRoom(Player other)
    {
        Debug.LogFormat("OnPlayerEnteredRoom() {0}", other.NickName);
        if(PhotonNetwork.IsMasterClient)
        {
            //OnPlayerLeftRoomの前に呼び出されます。
            Debug.LogFormat("OnPlayerEnteredRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient);
        }
    }

    /// <summary>
    /// プレイヤーがルームから切断したときに呼ばれる
    /// </summary>
    /// <param name="other">切断したプレイヤー</param>
    public override void OnPlayerLeftRoom(Player other)
    {
        Debug.LogFormat("OnPlayerLeftRoom() {0}", other.NickName);
        if(PhotonNetwork.IsMasterClient)
        {
            Debug.LogFormat("OnPlayerLeftRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient);
        }
    }
    /// <summary>
    /// プレイヤーが部屋を出た時に呼び出される
    /// </summary>
    public override void OnLeftRoom()
    {

    }

    #endregion

    #region Public Methods

    /// <summary>
    /// 部屋から退出する処理
    /// </summary>
    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
