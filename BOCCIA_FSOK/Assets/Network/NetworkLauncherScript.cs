using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class NetworkLauncherScript : MonoBehaviourPunCallbacks
{
    #region Private Serializable Fields
    [Tooltip("ルームの最大人数。ルームの人数が最大だと新たにルームを作成します")]
    [SerializeField]
    private byte maxPlayersPerRoom = 2;
    #endregion

    #region Private Fields
    /// <summary>
    /// ゲームのバージョン
    /// </summary>
    string gameVersion = "1";
    #endregion

    #region MonoBehaviour CallBacks

    private void Awake()
    {
        //接続しているプレイヤーが同じレベルを読み込む
        PhotonNetwork.AutomaticallySyncScene = true;
    }
    // Start is called before the first frame update
    void Start()
    {
    }

    #endregion

    #region Public Methods
    /// <summary>
    /// 接続する
    /// 既に接続されていたらランダムな部屋に参加
    /// 接続されていなければPhotonCloudNetworkに接続
    /// </summary>
    public void Connect()
    {
        Debug.Log("Photonに接続します。既に接続されていればランダムな部屋に参加します。接続されていなければPhotonサーバーに接続します");
        //接続されているか確認
        if(PhotonNetwork.IsConnected)
        {
            //接続されている場合ランダムな部屋に参加する
            //失敗した場合はOnJoinRandomFailed関数で通知を受け取り、作成する
            PhotonNetwork.JoinRandomRoom();
            Debug.Log("ランダムな部屋に参加します");
        }
        else
        {
            //接続されていない場合
            //Photoサーバーに接続する
            PhotonNetwork.GameVersion = gameVersion;
            PhotonNetwork.ConnectUsingSettings();
            Debug.Log("Photonサーバーに接続します");
        }
    }
    #endregion

    #region MonoBehaviourPunCallbacks Callbacks
    public override void OnConnectedToMaster()
    {
        Debug.Log("OnConnectedToMaster()がPUNによって呼ばれました");
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogWarningFormat("OnDisconnected()がPUNによって呼ばれました。原因{0}",cause);
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("OnJoinRandomFailed()がPUNによって呼び出されました。利用可能なランダムな部屋がないため作成します");
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = maxPlayersPerRoom });
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("OnJoinedRoom()がPUNによって呼ばれました。現在ルームに参加できました。");
    }
    #endregion

    private void Update()
    {
        //PhotonNetwork.JoinRandomRoom();
    }


}
