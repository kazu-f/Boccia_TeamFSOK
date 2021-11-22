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
    private bool m_IsEnable = false;
    private float timer = 0.0f;
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
            timer = 0;
            m_IsEnable = isEnable;
        }
    }

    private void Awake()
    {
        InitPlayerScript();
    }
    // Start is called before the first frame update
    void Start()
    {
        JackBoll = GameObject.Find("GameFlow").GetComponent<BallFlowScript>().GetJackBall();
        ThrowTrance = this.gameObject.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (!m_IsEnable)
        {
            return;
        }
        timer += Time.deltaTime;
        if (timer < 2.0f)
        {
            return;
        }
        if (GameObject.Find("GameFlow").GetComponent<TeamFlowScript>().GetNowTeam() == Team.Blue)
        {
            if (!GameObject.Find("GameFlow").GetComponent<BallFlowScript>().IsPreparedJack())
            {
                Vector2 throwPow = Vector2.zero;
                throwPow.y = Random.value;
                throwBallControler.SetThrowPow(throwPow);
                throwBallControler.ThrowBall();
                m_IsEnable = false;
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
                float angle = Vector3.Dot(-ThrowForward, TargetNorm);
                throwAngleController.SetAngle(angle);

                Vector2 throwPow = Vector2.one;
                float dis = TargetPos.magnitude - ThrowTrance.position.magnitude;
                //Debug.Log("プレイヤーとジャックボールの差分" + dis);
                float ThrowMax = 12.5f - ThrowTrance.position.magnitude;
                //Debug.Log("プレイヤーからコートの最奥まで" + ThrowMax);
                float power = dis / ThrowMax;
                //Debug.LogError("投げる力。" + throwPow.y);
                float scat = Random.Range(-0.3f,0.3f);
                throwPow.y = power+ scat;
                throwBallControler.SetThrowPow(throwPow);
                if (!throwBallControler.IsDecision())
                {
                    throwBallControler.ThrowBall();
                    m_IsEnable = false;
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
