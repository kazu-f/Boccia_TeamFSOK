using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamFlowDelayScript : MonoBehaviour
{
    private GameObject Failed = null;
    [SerializeField] private float AfterEndTime = 3.0f;
    private float AfterNowTime = 0.0f;
    private bool IsStart = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (IsStart)
        {
            //�x��������
            //�x�����̏ꍇ�̓t���O�𗧂đ�����
            IsStart = !EndAfterTime();
        }
    }

    /// <summary>
    /// �x��
    /// </summary>
    /// <returns>�x�����I���������ǂ���</returns>
    private bool EndAfterTime()
    {
        if (AfterNowTime < AfterEndTime)
        {
            //�x����
            AfterNowTime += Time.deltaTime;
            return false;
        }
        else
        {
            Failed = GameObject.Find("Failed");
            Failed.GetComponent<FailedMoveScript>().FontAlphaZero();
            //�x���I��
            AfterNowTime = 0.0f;
            return true;
        }
    }

    /// <summary>
    /// �x���J�n������
    /// </summary>
    public void DelayStart()
    {
        IsStart = true;
    }

    /// <summary>
    /// �x�������ǂ���
    /// </summary>
    /// <returns>�x�����Ȃ�X�^�[�g�t���O�������Ă���</returns>
    public bool IsDelay()
    {
        return IsStart;
    }
}
