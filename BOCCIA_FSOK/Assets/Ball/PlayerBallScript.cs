using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class PlayerBallScript : IBallScript
{
    private GameObject FailedFont = null;

    // Start is called before the first frame update
    void Start()
    {
        FailedFont=GameObject.Find("FailedSprite");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// ボールがエリア内に入った時の処理
    /// </summary>
    public override void InsideVenue()
    {
        InArea = true;
    }

    /// <summary>
    /// ボールがエリア外に出た時の処理
    /// </summary>
    public override void OutsideVenue()
    {
        GetOutRange = true;
        InArea = false;
        //this.gameObject.SetActive(false);
    }

    /// <summary>
    /// ボールが停止したときの処理
    /// </summary>
    public override void EndThrow()
    {
        IsThrowing = false;
        if (InArea == false)
        {
            this.gameObject.GetComponent<BallStateScript>().ResetState();
            this.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// キルエリアに入った時の処理
    /// </summary>
    public override void InKillArea()
    {
        FailedFont.SetActive(true);
        InArea = false;
        this.gameObject.SetActive(false);
    }
}
