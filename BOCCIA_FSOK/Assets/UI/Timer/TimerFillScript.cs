using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
public class TimerFillScript : MonoBehaviour
{
    [SerializeField] private float Limit = 30.0f;
    private float NowTime = 0.0f;
    private bool IsStart = false;
    private float late = 1.0f;
    [SerializeField]private GameObject CircleBefore = null;
    [SerializeField] private GameObject CircleAfter = null;
    private Image CircleBeforeImage = null;
    private Image CircleAfterImage = null;
    [SerializeField] private Text time = null;
    private bool IsTimeUped = false;
    private ServerTimerScript ServerTimer = null;
    [SerializeField] private NetworkSendManagerScript m_SendManager = null;
    private bool[] TimedUp = new bool[2];
    private void Awake()
    {
        CircleBeforeImage = CircleBefore.GetComponent<Image>();
        CircleAfterImage = CircleAfter.GetComponent<Image>();
        ServerTimer = this.gameObject.GetComponent<ServerTimerScript>();
    }
    // Start is called before the first frame update
    void Start()
    {
       //ServerTimer.SetCountTimeSecond(Limit);
    }

    // Update is called once per frame
    void Update()
    {
        if (IsStart)
        {
            if(ServerTimer.isCount == false)
            {
                return;
            }
            //NowTime -= Time.deltaTime;
            NowTime = ServerTimer.CountLeft();
            late = NowTime / Limit;
            CircleBeforeImage.fillAmount = late;
            if (late <= 0.0f)
            {
                //var photon_view = this.gameObject.GetComponent<PhotonView>();
                //if (photon_view.IsMine)
                //{
                //    photon_view.RPC(nameof(TimerUpRPC), RpcTarget.All);
                //}

                //タイムアップ
                Debug.Log("タイムアップ");
                Debug.Log("タイムアップしたのでタイマーを止める");
                IsTimeUped = true;

                //IsStart = false;

                ////このフラグもう使って無くね？
                //if (PhotonNetwork.LocalPlayer.IsMasterClient)
                //{
                //    //マスタークライアントの時
                //    m_SendManager.SendMasterIsTimeUp(true);
                //    TimedUp[0] = true;
                //}
                //else
                //{
                //    //クライアントの時
                //    m_SendManager.SendClientIsTimeUp(true);
                //    TimedUp[1] = true;
                //}
            }

            //切り上げ
            int timenum = Mathf.CeilToInt(NowTime);
            time.text = "" + timenum;
            if (timenum < Limit / 4)
            {
                time.color = Color.red;
                CircleAfterImage.color = Color.red;
                return;
            }
            else if (timenum < Limit / 2)
            {
                CircleAfterImage.color = Color.yellow;
                time.color = Color.yellow;
                return;
            }
            else
            {
                time.color = Color.green;
                CircleAfterImage.color = Color.green;
                return;
            }
        }
    }

    public void LateUpdate()
    {
        if (late <= 0.0f)
        {
            Debug.Log("タイムアップしたのでタイマーを止める");
            IsStart = false;
        }
        //if (IsTimeUped)
        //{
        //    Debug.Log("タイムアップしたのでタイマーを止める");
        //    IsStart = false;
        //}
    }

    [Photon.Pun.PunRPC]
    public void TimerUpRPC()
    {
        //タイムアップ
        Debug.Log("タイムアップ");
        Debug.Log("タイムアップしたのでタイマーを止める");
        IsTimeUped = true;
    }

    [Photon.Pun.PunRPC]
    public void TimerStart()
    {
        Debug.Log("タイマースタート！");
        ServerTimer.SetCountTimeSecond(Limit);
        NowTime = Limit;
        IsStart = true;
        late = 1.0f;
        time.color = Color.green;
        CircleAfterImage.color = Color.green;
        IsTimeUped = false;
        for(int i = 0;i<TimedUp.Length;i++)
        {
            //タイムアップフラグをリセット
            TimedUp[i] = false;
        }

    }

    public bool IsTimerStart()
    {
        return IsStart;
    }

    public void TimerStop()
    {
        //投げたので止める
        Debug.Log("ボールを投げたのでタイマーを止める");
        IsStart = false;
    }

    //AI戦用のタイムアップ下かどうかの関数
    //public bool IsTimeUpForAI()
    //{
    //    return TimedUp[0];
    //}
    public bool IsTimeUp()
    {
        //if (PhotonNetwork.LocalPlayer.IsMasterClient)
        //{
        //    //マスタークライアントの時
        //    //クライアントがタイムアップしたかどうかを取得
        //    TimedUp[1] = m_SendManager.ReceiveClientIsTimeUp();
        //}
        //else
        //{
        //    //クライアントの時
        //    //マスタークライアントがタイムアップしたかどうかを取得
        //    TimedUp[0] = m_SendManager.ReceiveMasterIsTimeUp();
        //}

        //for (int i = 0;i < TimedUp.Length; i++)
        //{
        //    if(TimedUp[i] == false)
        //    {
        //        //マスターとクライアントで終わっていないとき
        //        IsTimeUped = false;
        //        return IsTimeUped;
        //    }
        //}
        //IsTimeUped = true;
        return IsTimeUped;
    }

    public void SyncStartTimer(bool isMyTeam)
    {
        Debug.Log("タイマーをスタートします。");
        if(!isMyTeam)
        {
            //マスタークライアント以外は呼び出さない。
            Debug.Log("マスタークライアンではないので呼び出さない。");
            return;
        }
        var photon_view = this.gameObject.GetComponent<PhotonView>();
        if (!photon_view.IsMine)
        {
            photon_view.RequestOwnership();
        }
        photon_view.RPC(nameof(TimerStart), RpcTarget.All);
    }
}
