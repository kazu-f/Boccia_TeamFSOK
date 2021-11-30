using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class NetworkLauncherScript : MonoBehaviourPunCallbacks
{
    #region Private Serializable Fields
    [Tooltip("ルームの最大人数。ルームの人数が最大だと新たにルームを作成します")]
    [SerializeField]
    private byte maxPlayersPerRoom = 2;
    [SerializeField]
    private int SendRate = 15;      //1秒間に何回パケットを送信するか
    #endregion

    #region Private Fields
    string gameVersion = "1.6";       // ゲームのバージョン
    bool isConnecting;
    bool IsJoinedRoom = false;
    bool IsGameSceneLoaded = false;
    bool IsUseAI = false;
    bool IsDisconected = false;
    bool IsDisconecting = false;
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
        connectServer.SetActive(false);
        waitMatching.SetActive(true);
        endMatching.SetActive(false);
    }

    #endregion

    #region Public Methods
    [Tooltip("マッチング完了を知らせる。")]
    [SerializeField]
    private GameObject endMatching;
    [Tooltip("マッチングが進行中であることをユーザーに知らせるためのUI")]
    [SerializeField]
    private GameObject waitMatching;
    [Tooltip("サーバー接続が進行中であることをユーザーに知らせるためのUI")]
    [SerializeField]
    private GameObject connectServer;

    /// <summary>
    /// 接続する
    /// 既に接続されていたらランダムな部屋に参加
    /// 接続されていなければPhotonCloudNetworkに接続
    /// </summary>
    public void Connect()
    {
        connectServer.SetActive(true);
        waitMatching.SetActive(false);
        endMatching.SetActive(false);
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
            isConnecting = PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = gameVersion;
            PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.KeepAliveInBackground = 0.0f;     //アプリポーズの許容時間？。
            //1秒間に同期する回数を変更
            PhotonNetwork.SendRate = SendRate;
            PhotonNetwork.SerializationRate = SendRate;
            Debug.Log("Photonサーバーに接続します");
        }
    }
    #endregion

    #region MonoBehaviourPunCallbacks Callbacks
    public override void OnConnectedToMaster()
    {
        if(!PhotonNetwork.OfflineMode)
        {
            connectServer.SetActive(false);
            waitMatching.SetActive(true);
            endMatching.SetActive(false);
        }
        else
        {
            connectServer.SetActive(false);
            waitMatching.SetActive(false);
            endMatching.SetActive(true);
        }

        if (isConnecting)
        {
            PhotonNetwork.JoinRandomRoom();
            isConnecting = false;
        }
        Debug.Log("OnConnectedToMaster()がPUNによって呼ばれました");
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        connectServer.SetActive(false);
        waitMatching.SetActive(false);
        endMatching.SetActive(true);
        isConnecting = false;
        Debug.LogWarningFormat("OnDisconnected()がPUNによって呼ばれました。原因{0}",cause);
        IsDisconected = true;
    }

    //プレイヤーが入室してきた。
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);

        if(newPlayer.ActorNumber == maxPlayersPerRoom)
        {
            //マッチング完了を通知。
            connectServer.SetActive(false);
            waitMatching.SetActive(false);
            endMatching.SetActive(true);
        }
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("OnJoinRandomFailed()がPUNによって呼び出されました。利用可能なランダムな部屋がないため作成します");
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = maxPlayersPerRoom });
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("OnJoinedRoom()がPUNによって呼ばれました。現在ルームに参加できました。");
        IsJoinedRoom = true;
    }

    public override void OnLeftRoom()
    {
        Debug.Log("OnLeftRoom()がPUNによって呼ばれました。現在ルームから抜けました。");
        IsJoinedRoom = false;
    }
    #endregion

    public void JoinRandomRoom()
    {
        //接続されているか確認
        if (PhotonNetwork.IsConnected)
        {
            //接続されている場合ランダムな部屋に参加する
            //失敗した場合はOnJoinRandomFailed関数で通知を受け取り、作成する
            PhotonNetwork.JoinRandomRoom();
            Debug.Log("ランダムな部屋に参加します");
        }
    }
    private void Update()
    {
        if (!IsDisconecting)
        {
            if (IsJoinedRoom)
            {
                //部屋に参加しているとき
                if (PhotonNetwork.CurrentRoom.PlayerCount == maxPlayersPerRoom)
                {
                    if (IsMasterClient())
                    {
                        //マスタークライアントなのでシーンをロードします
                        if (!IsGameSceneLoaded)
                        {
                            //部屋の人数が最大だとゲームシーンに移行
                            Debug.Log("ゲームシーンをロードします");
                            PhotonNetwork.LoadLevel("BocciaGameScene");
                            IsGameSceneLoaded = true;
                        }
                    }
                }
                if (IsUseAI)
                {
                    //AIを使用するとき
                    //部屋を抜ける
                    Debug.Log("ルームから抜けます");
                    PhotonNetwork.LeaveRoom();
                    //接続を切断する
                    Debug.Log("接続を切断します");
                    PhotonNetwork.Disconnect();
                    IsDisconecting = true;
                }
            }
        }
        if (IsDisconected)
        {
            if (IsUseAI)
            {
                //接続を切断した後
                if (!IsGameSceneLoaded)
                {
                    //オフラインモードにする
                    PhotonNetwork.OfflineMode = true;
                    //オフライン用の部屋に参加
                    PhotonNetwork.JoinRandomRoom();
                    //部屋の人数が最大だとゲームシーンに移行
                    Debug.Log("ゲームシーンをロードします");
                    SceneManager.LoadScene("BocciaGameScene");
                    IsGameSceneLoaded = true;
                }
            }
        }

    }

    /// <summary>
    /// 自分がマスタークライアントかどうか
    /// </summary>
    /// <returns>マスタークライアントかどうかのフラグ</returns>
    public bool IsMasterClient()
    {
        return PhotonNetwork.LocalPlayer.IsMasterClient;
    }

    /// <summary>
    /// 呼び出すと、ネットワークを切断してゲームシーンをロードする
    /// </summary>
    public void UseAI()
    {
        IsUseAI = true;
    }
}
