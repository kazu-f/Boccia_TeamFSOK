using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IBallScript : MonoBehaviour
{
    protected bool InArea = false;
    private bool IsThrowing = true;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

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
    /// �{�[�����~�܂������̏���
    /// </summary>
    public abstract void EndThrow();

    public bool GetInArea()
    {
        return InArea;
    }

    public void ResetVar()
    {
        IsThrowing = true;
        InArea = false;
    }

    public bool GetIsThrow()
    {
        return IsThrowing;
    }

}