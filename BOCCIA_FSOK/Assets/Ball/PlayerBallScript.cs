using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
public class PlayerBallScript : IBallScript
{
    public GameObject FailedFont = null;

    public AudioSource boundSE;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// �{�[�����G���A��ɓ��������̏���
    /// </summary>
    public override void InsideVenue()
    {
        InArea = true;
    }

    /// <summary>
    /// �{�[�����G���A�O�ɏo�����̏���
    /// </summary>
    public override void OutsideVenue()
    {
        GetOutRange = true;
        InArea = false;
        //this.gameObject.SetActive(false);
    }

    /// <summary>
    /// �{�[������~�����Ƃ��̏���
    /// </summary>
    public override void EndThrow()
    {
        IsThrowing = false;
        if (InArea == false)
        {
            FailedFont = GameObject.Find("Image");
            FailedFont.GetComponent<FailedMoveScript>().SetDirect();
            Debug.LogError("場外");
            boundSE.Play();

            this.gameObject.GetComponent<BallStateScript>().ResetState();
            this.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// �L���G���A�ɓ��������̏���
    /// </summary>
    public override void InKillArea()
    {
        InArea = false;
        this.gameObject.SetActive(false);
    }
}
