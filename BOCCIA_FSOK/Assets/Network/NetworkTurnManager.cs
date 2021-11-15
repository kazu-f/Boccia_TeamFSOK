using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;

public class NetworkTurnManager : MonoBehaviourPunCallbacks, IPunTurnManagerCallbacks
{
    private PunTurnManager turnManager;
    [SerializeField] private float TurnDuration = 5.0f;     //ターンを回す秒数
    private void Awake()
    {
        this.turnManager = this.gameObject.AddComponent<PunTurnManager>();
        this.turnManager.TurnManagerListener = this;
        this.turnManager.TurnDuration = TurnDuration;
    }
    // Start is called before the first frame update
    void Start()
    {
        if(PhotonNetwork.IsMasterClient)
        {
            //ターン開始
            this.turnManager.BeginTurn();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // プレイヤーがターン終了したとき
    public void OnPlayerFinished(Player photonPlayer, int turn, object move) 
    { 

    }

    // 動作したとき
    public void OnPlayerMove(Player photonPlayer, int turn, object move)
    {
    
    }

    //ターン開始
    public void OnTurnBegins(int turn)
    {

    }

    //全プレイヤーがターン終了
    public void OnTurnCompleted(int obj)
    {
        if(PhotonNetwork.IsMasterClient)
        {
            this.turnManager.BeginTurn();
        }
    }

    //タイマー終了
    public void OnTurnTimeEnds(int turn)
    { 
    
    }

    //ターンを終了させる
    public void TurnEnd()
    {
        //第一引数は何でもよい
        this.turnManager.SendMove(1, true);
    }
}
