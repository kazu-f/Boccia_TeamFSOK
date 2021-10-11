using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IBallScript : MonoBehaviour
{
    protected bool InArea = false;
    protected bool IsThrowing = true;
    protected bool GetOutRange = false;
    public float KillTime = 1.0f;
    private float NowTime = 0.0f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (GetOutRange)
        {
            //�͈͊O�ɏo�Ă���Ƃ�
            NowTime += Time.deltaTime;
            if (NowTime > KillTime)
            {
                InKillArea();
                NowTime = 0.0f;
            }
        }
    }

    /// <summary>
    /// �{�[�����G���A���ɓ��������̏���
    /// </summary>
    public abstract void InsideVenue();

    /// <summary>
    /// �{�[�����G���A�O�ɏo�����̏���
    /// </summary>
    public abstract void OutsideVenue();

    /// <summary>
    /// �L���G���A�ɓ��������̏���
    /// </summary>
    public abstract void InKillArea();

    /// <summary>
    /// �{�[�����~�܂������̏���
    /// </summary>
    public abstract void EndThrow();

    public void ThrowBall()
    {
        IsThrowing = true;
    }
    public bool GetInArea()
    {
        return InArea;
    }

    public void ResetVar()
    {
        IsThrowing = true;
        InArea = false;
        GetOutRange = false;
        NowTime = 0.0f;
    }

    public bool GetIsThrow()
    {
        return IsThrowing;
    }

}