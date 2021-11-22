using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BocciaPlayer;

public class AIFlow : IPlayerController
{
    private TouchInfo info;
    private Transform ThrowTrance;
    private GameObject JackBoll = null;
    private Vector3 JackPos = Vector3.zero;
    private GameObject CurrentTarn = null;

    private int timer = 0;
    /// <summary>
    /// プレイヤーリセット
    /// PlayerControllerと一緒
    /// </summary>
    override public void ResetPlayer()
    {
        ballHolderController.ResetBall();
    }
    public override void SwitchPlayer(bool isEnable)
    {
        //開始時点のトランスフォームへ移動。
        if (isEnable == true)
        {
            //プレイヤーが切り替わる時にカメラの位置を合わせる。
            throwAngleController.ChangeCamPos();
        }
    }

    private void Awake()
    {
        InitPlayerScript();
    }
    // Start is called before the first frame update
    void Start()
    {
        JackBoll =  GameObject.Find("GameFlow").GetComponent<BallFlowScript>().GetJackBall();
        if (JackBoll == null)
        {
            //Debug.LogError("JackBollが見つかりませんでした。");
        }

        ThrowTrance = this.gameObject.transform;
        ;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameObject.Find("GameFlow").GetComponent<TeamFlowScript>().GetNowTeam() == Team.Blue)
        {
            if (!GameObject.Find("GameFlow").GetComponent<BallFlowScript>().IsPreparedJack())
            {
                Vector2 throwPow = Vector2.zero;
                throwPow.y = 1.0f;
                throwBallControler.SetThrowPow(throwPow);
                timer++;
                if (timer > 100)
                {
                    //Debug.LogError("ジャックボールを投げます。"+throwPow.x+ throwPow.y);
                    timer = 0;
                }
            }
            else
            {
                //ジャックボールの位置
                JackPos = JackBoll.transform.position;
                //投げたい位置を計算
                Vector3 TargetPos = JackPos - ThrowTrance.position;
                //正規化して投げる方向だけにする
                Vector3 TargetNorm = TargetPos.normalized;
                //投げるプレイヤーの前方向
                Vector3 ThrowForward = ThrowTrance.forward;
                //プレイヤーをジャックボールの方向に向ける
                ThrowTrance.rotation.SetFromToRotation(ThrowForward, TargetNorm);
                float angle = Vector3.Dot(ThrowForward, TargetNorm);
                throwAngleController.SetAngle(angle);

                Vector2 throwPow = Vector2.zero;
                float power = TargetPos.magnitude / 12.5f;
                throwPow.y = power;
                throwBallControler.SetThrowPow(throwPow);
                if (!throwBallControler.IsDecision())
                {
                    throwBallControler.ThrowBall();
                    Debug.LogError("マイボールを投げます。" + throwPow.x + throwPow.y);
                    timer = 0;
                }
                timer++;
                if (timer > 100)
                {
                }
            }
        }
  }
    public TouchInfo GetPath()
    {
        return info;
    }

    public float FllowPower()
    {
        //投げる値はテキトー
        return 1.0f;
    } 
}
