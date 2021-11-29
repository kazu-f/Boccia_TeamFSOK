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

    private void Awake()
    {
        CircleBeforeImage = CircleBefore.GetComponent<Image>();
        CircleAfterImage = CircleAfter.GetComponent<Image>();
        ServerTimer = this.gameObject.GetComponent<ServerTimerScript>();
    }
    // Start is called before the first frame update
    void Start()
    {
        ServerTimer.SetCountTimeSecond(Limit);
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
                //タイムアップ
                Debug.Log("タイムアップ");
                IsStart = false;
                IsTimeUped = true;
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
    }

    public bool IsTimerStart()
    {
        return IsStart;
    }

    public void TimerStop()
    {
        IsStart = false;
    }
    public bool IsTimeUp()
    {
        if (m_SendManager.IsOwner())
        {
            //オーナーの時
            m_SendManager.SendIsTimeUp(IsTimeUped);
            return IsTimeUped;
        }
        //オーナーじゃない時
        IsTimeUped = m_SendManager.ReceiveIsTimeUp();

        return IsTimeUped;
    }

    public void SyncStartTimer()
    {
        Debug.Log("タイマーをスタートします。");
        if(!PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            //マスタークライアント以外は呼び出さない。
            Debug.Log("マスタークライアンではないので呼び出さない。");
            return;
        }
        var photon_view = this.gameObject.GetComponent<PhotonView>();
        photon_view.RPC(nameof(TimerStart), RpcTarget.All);
    }
}
