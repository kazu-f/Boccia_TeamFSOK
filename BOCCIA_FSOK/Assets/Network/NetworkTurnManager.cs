using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;

public class NetworkTurnManager : MonoBehaviourPunCallbacks, IPunTurnManagerCallbacks
{
    private PunTurnManager turnManager;
    [SerializeField] private float TurnDuration = 5.0f;     //�^�[�����񂷕b��
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
            //�^�[���J�n
            this.turnManager.BeginTurn();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // �v���C���[���^�[���I�������Ƃ�
    public void OnPlayerFinished(Player photonPlayer, int turn, object move) 
    { 

    }

    // ���삵���Ƃ�
    public void OnPlayerMove(Player photonPlayer, int turn, object move)
    {
    
    }

    //�^�[���J�n
    public void OnTurnBegins(int turn)
    {

    }

    //�S�v���C���[���^�[���I��
    public void OnTurnCompleted(int obj)
    {
        if(PhotonNetwork.IsMasterClient)
        {
            this.turnManager.BeginTurn();
        }
    }

    //�^�C�}�[�I��
    public void OnTurnTimeEnds(int turn)
    { 
    
    }

    //�^�[�����I��������
    public void TurnEnd()
    {
        //�������͉��ł��悢
        this.turnManager.SendMove(1, true);
    }
}
