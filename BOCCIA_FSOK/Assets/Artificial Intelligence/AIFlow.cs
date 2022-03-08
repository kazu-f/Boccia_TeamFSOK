using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BocciaPlayer;

public enum AIState 
{ 
    AI_SetAngle,
    AI_SetThrowPow,
    AI_Throw,
    AI_Wait
}


public class AIFlow : IPlayerController
{
    private GameObject gameFlow = null;
    private BallFlowScript ballFlow = null;
    private TouchInfo info;
    private Transform ThrowTrance;
    private ArmScript armScript;
    private GameObject JackBall = null;
    private bool m_IsEnable = false;
    private float timer = 0.0f;
    private TeamFlowScript TeamFlow;
    private SwichActiveGameObjects swichActiveGameObj;
    private const float DicisionTime = 2.0f;
    private float throwAngle = 0.0f;
    private float angleTimer = 0.0f;
    private Vector2 throwPower = Vector2.zero;
    private float powerTimer = 0.0f;
    private AIState state = AIState.AI_Wait;
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
        m_IsEnable = isEnable;
        //開始時点のトランスフォームへ移動。
        if (isEnable == true)
        {
            //プレイヤーが切り替わる時にカメラの位置を合わせる。
            throwAngleController.ChangeCamPos();
            timer = 0.0f;
            angleTimer = 0.0f;
            powerTimer = 0.0f;
            swichActiveGameObj.SwitchGameObject(false);
            //投げる時のパラメータを計算しておく。
            CalcAngle();
            CalcThrowPower();
            state = AIState.AI_SetAngle;
        }
    }

    private void Awake()
    {
        InitPlayerScript();
        swichActiveGameObj = SwichActiveGameObjects.GetInstance();
        armScript = throwBallControler.GetArmScript();
        throwBallControler.enabled = false;
        throwBallControler.isMyTeam = true;
    }
    // Start is called before the first frame update
    void Start()
    {
        gameFlow = GameObject.Find("GameFlow");
        ballFlow = gameFlow.GetComponent<BallFlowScript>();
        JackBall = ballFlow.GetJackBall();
        ThrowTrance = this.gameObject.transform;
        TeamFlow = gameFlow.GetComponent<TeamFlowScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!m_IsEnable || throwBallControler.IsDecision())
        {
            return;
        }
        else
        {
            swichActiveGameObj.SwitchGameObject(false);
        }
        timer += Time.deltaTime;
        if (timer < 2.0f)
        {
            return;
        }

        //ステートで切り替える。
        switch (state)
        {
            case AIState.AI_SetAngle:
                SetThrowAngle();
                break;
            case AIState.AI_SetThrowPow:
                SetThrowPower();
                break;
            case AIState.AI_Throw:
                throwBallControler.ThrowBall();
                throwBallControler.enabled = false;
                m_IsEnable = false;
                state = AIState.AI_Wait;
                break;
            case AIState.AI_Wait:

                break;
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

    private void CalcAngle()
    {
        if(!ballFlow.IsPreparedJack())
        {
            //ジャックボールを投げる方向を適当に。
            throwAngle = Random.Range(-3.0f, 3.0f);
        }
        else
        {
            //投げたい位置を計算
            Vector3 TargetPos = JackBall.transform.position - armScript.GetPosition();
            TargetPos.y = 0.0f;
            //方向を調整
            //正規化して投げる方向だけにする
            Vector3 TargetNorm = TargetPos.normalized;
            //投げるプレイヤーの前方向
            Vector3 ThrowForward = new Vector3(0.0f, 0.0f, 1.0f);
            //プレイヤーをジャックボールの方向に向ける
            ThrowTrance.rotation.SetFromToRotation(ThrowForward, TargetNorm);
            throwAngle = Vector3.SignedAngle(ThrowForward, TargetNorm, Vector3.up);
        }
    }

    private void CalcThrowPower()
    {
        if (!ballFlow.IsPreparedJack())
        {
            throwPower = Vector2.zero;
            throwPower.y = Random.Range(0.4f, 0.7f);
        }
        else
        {
            //投げたい位置を計算
            Vector3 TargetPos = JackBall.transform.position - armScript.GetPosition();
            TargetPos.y = 0.0f;
            throwPower = Vector2.zero;
            Vector3 dis = TargetPos - armScript.GetPosition();
            //Debug.LogError("プレイヤーとジャックボールの差分" + dis.magnitude);
            const float coatSize = 12.5f;
            float ThrowMax = coatSize - armScript.GetPosition().z;
            //Debug.LogError("プレイヤーからコートの最奥まで" + ThrowMax);
            float power = dis.magnitude / ThrowMax;
            //Debug.LogError("投げる力。" + power);
            float scat = Random.Range(-0.2f, 0.0f);
            throwPower.y = power + scat;
            throwPower.x = Random.Range(-0.1f, 0.1f);
        }
    }

    //向きを変えていく。
    private void SetThrowAngle()
    {
        //タイマーを進める。
        angleTimer += Time.deltaTime * DicisionTime;

        float angle = Mathf.Lerp(0.0f, throwAngle, Mathf.Min(1.0f,angleTimer / DicisionTime));

        throwAngleController.SetAngle(angle);

        if(angleTimer > DicisionTime)
        {
            state = AIState.AI_SetThrowPow;
        }
    }

    //投げる力を変えていく。
    private void SetThrowPower()
    {
        if(!throwBallControler.enabled)
        {
            throwBallControler.enabled = true;
            Vector2 startPos = throwPower;
            startPos.x += 0.5f;
            throwBallControler.StartThrowBall(startPos);
        }

        //タイマーを進める。
        powerTimer += Time.deltaTime;
        Vector2 throwPow = Vector2.zero;
        throwPow.x = Mathf.Lerp(0.0f, throwPower.x, Mathf.Min(1.0f, powerTimer / DicisionTime));
        throwPow.y = Mathf.Lerp(0.0f, throwPower.y, Mathf.Min(1.0f, powerTimer / DicisionTime));
        throwBallControler.SetThrowPow(throwPow);

        if (powerTimer > DicisionTime)
        {
            state = AIState.AI_Throw;
        }
    }
}
