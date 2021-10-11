using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IBallScript : MonoBehaviour
{
    protected bool InArea = false;
    protected bool IsThrowing = true;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// ボールがエリア内に入った時の処理
    /// </summary>
    public abstract void InsideVenue();

    /// <summary>
    /// ボールがエリア外に出た時の処理
    /// </summary>
    public abstract void OutsideVenue();

    /// <summary>
    /// キルエリアに入った時の処理
    /// </summary>
    public abstract void InKillArea();

    /// <summary>
    /// ボールが止まった時の処理
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
    }

    public bool GetIsThrow()
    {
        return IsThrowing;
    }

}