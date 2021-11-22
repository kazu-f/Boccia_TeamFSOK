using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
public class TimerFillScript : MonoBehaviourPun
{
    [SerializeField] private double Limit = 30.0;
    private double NowTime = 0.0f;
    private bool IsStart = false;
    private float late = 1.0f;
    [SerializeField]private GameObject CircleBefore = null;
    [SerializeField] private GameObject CircleAfter = null;
    private Image CircleBeforeImage = null;
    private Image CircleAfterImage = null;
    [SerializeField] private Text time = null;
    private bool IsTimeUped = false;
    private double StartTime = 0.0;
    private void Awake()
    {
        CircleBeforeImage = CircleBefore.GetComponent<Image>();
        CircleAfterImage = CircleAfter.GetComponent<Image>();
        
        if(PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            //�J�E���g�_�E���̃X�^�[�g���Ԃ��Z�b�g
            var properties = new ExitGames.Client.Photon.Hashtable();
            properties.Add("StartTime", PhotonNetwork.Time);
            PhotonNetwork.CurrentRoom.SetCustomProperties(properties);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        //���݂̃��[������J�n���Ԃ��擾
        StartTime = (double)PhotonNetwork.CurrentRoom.CustomProperties["StartTime"];
    }

    // Update is called once per frame
    void Update()
    {
        if (IsStart)
        {
            //�o�ߎ��Ԃ����߂�
            double elapsedTime = PhotonNetwork.Time - StartTime;
            //���~�b�g����o�ߎ��Ԃ������Č��݂̎��Ԃ����߂�
            NowTime = Limit - elapsedTime;
            //NowTime -= Time.deltaTime;
            //�ꉞ�Œ�l���߂Ƃ�
            NowTime = Mathf.Max((float)NowTime, -0.01f);
            late = (float)NowTime / (float)Limit;
            CircleBeforeImage.fillAmount = late;
            if (late < 0.0f)
            {
                IsStart = false;
                IsTimeUped = true;
            }
            //�؂�グ
            int timenum = Mathf.CeilToInt((float)NowTime);
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
        }
    }

    public void TimerStart()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            //�J�E���g�_�E���̃X�^�[�g���Ԃ��Z�b�g
            var properties = new ExitGames.Client.Photon.Hashtable();
            properties.Add("StartTime", PhotonNetwork.Time);
            PhotonNetwork.CurrentRoom.SetCustomProperties(properties);
        }
        //���݂̃��[������J�n���Ԃ��擾
        StartTime = (double)PhotonNetwork.CurrentRoom.CustomProperties["StartTime"];
        NowTime = Limit;
        IsStart = true;
        late = 1.0f;
        time.color = Color.green;
        CircleAfterImage.color = Color.green;
        IsTimeUped = false;
    }

    public void TimerStop()
    {
        IsStart = false;
    }
    public bool IsTimeUp()
    {
        return IsTimeUped;
    }
}
