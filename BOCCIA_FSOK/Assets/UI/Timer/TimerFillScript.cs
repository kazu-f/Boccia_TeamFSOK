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

                //�^�C���A�b�v
                Debug.Log("�^�C���A�b�v");
                Debug.Log("�^�C���A�b�v�����̂Ń^�C�}�[���~�߂�");
                IsTimeUped = true;

                //IsStart = false;

                ////���̃t���O�����g���Ė����ˁH
                //if (PhotonNetwork.LocalPlayer.IsMasterClient)
                //{
                //    //�}�X�^�[�N���C�A���g�̎�
                //    m_SendManager.SendMasterIsTimeUp(true);
                //    TimedUp[0] = true;
                //}
                //else
                //{
                //    //�N���C�A���g�̎�
                //    m_SendManager.SendClientIsTimeUp(true);
                //    TimedUp[1] = true;
                //}
            }

            //�؂�グ
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
            Debug.Log("�^�C���A�b�v�����̂Ń^�C�}�[���~�߂�");
            IsStart = false;
        }
        //if (IsTimeUped)
        //{
        //    Debug.Log("�^�C���A�b�v�����̂Ń^�C�}�[���~�߂�");
        //    IsStart = false;
        //}
    }

    [Photon.Pun.PunRPC]
    public void TimerUpRPC()
    {
        //�^�C���A�b�v
        Debug.Log("�^�C���A�b�v");
        Debug.Log("�^�C���A�b�v�����̂Ń^�C�}�[���~�߂�");
        IsTimeUped = true;
    }

    [Photon.Pun.PunRPC]
    public void TimerStart()
    {
        Debug.Log("�^�C�}�[�X�^�[�g�I");
        ServerTimer.SetCountTimeSecond(Limit);
        NowTime = Limit;
        IsStart = true;
        late = 1.0f;
        time.color = Color.green;
        CircleAfterImage.color = Color.green;
        IsTimeUped = false;
        for(int i = 0;i<TimedUp.Length;i++)
        {
            //�^�C���A�b�v�t���O�����Z�b�g
            TimedUp[i] = false;
        }

    }

    public bool IsTimerStart()
    {
        return IsStart;
    }

    public void TimerStop()
    {
        //�������̂Ŏ~�߂�
        Debug.Log("�{�[���𓊂����̂Ń^�C�}�[���~�߂�");
        IsStart = false;
    }

    //AI��p�̃^�C���A�b�v�����ǂ����̊֐�
    //public bool IsTimeUpForAI()
    //{
    //    return TimedUp[0];
    //}
    public bool IsTimeUp()
    {
        //if (PhotonNetwork.LocalPlayer.IsMasterClient)
        //{
        //    //�}�X�^�[�N���C�A���g�̎�
        //    //�N���C�A���g���^�C���A�b�v�������ǂ������擾
        //    TimedUp[1] = m_SendManager.ReceiveClientIsTimeUp();
        //}
        //else
        //{
        //    //�N���C�A���g�̎�
        //    //�}�X�^�[�N���C�A���g���^�C���A�b�v�������ǂ������擾
        //    TimedUp[0] = m_SendManager.ReceiveMasterIsTimeUp();
        //}

        //for (int i = 0;i < TimedUp.Length; i++)
        //{
        //    if(TimedUp[i] == false)
        //    {
        //        //�}�X�^�[�ƃN���C�A���g�ŏI����Ă��Ȃ��Ƃ�
        //        IsTimeUped = false;
        //        return IsTimeUped;
        //    }
        //}
        //IsTimeUped = true;
        return IsTimeUped;
    }

    public void SyncStartTimer(bool isMyTeam)
    {
        Debug.Log("�^�C�}�[���X�^�[�g���܂��B");
        if(!isMyTeam)
        {
            //�}�X�^�[�N���C�A���g�ȊO�͌Ăяo���Ȃ��B
            Debug.Log("�}�X�^�[�N���C�A���ł͂Ȃ��̂ŌĂяo���Ȃ��B");
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
