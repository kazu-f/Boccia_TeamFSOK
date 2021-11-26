using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BocciaPlayer;

public class AIFlow : IPlayerController
{
    private TouchInfo info;
    private Transform ThrowTrance;
    private ArmScript armScript;
    private GameObject JackBoll = null;
    private Vector3 JackPos = Vector3.zero;
    private GameObject CurrentTarn = null;
    private bool m_IsEnable = false;
    private float timer = 0.0f;
    private TeamFlowScript TeamFlow;
    private SwichActiveGameObjects swichActiveGameObj;
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
            timer = 0;
            swichActiveGameObj.SwitchGameObject(false);
        }
    }

    private void Awake()
    {
        InitPlayerScript();
        swichActiveGameObj = SwichActiveGameObjects.GetInstance();
        armScript = throwBallControler.GetArmScript();
    }
    // Start is called before the first frame update
    void Start()
    {
        JackBoll = GameObject.Find("GameFlow").GetComponent<BallFlowScript>().GetJackBall();
        ThrowTrance = this.gameObject.transform;
        TeamFlow = GameObject.Find("GameFlow").GetComponent<TeamFlowScript>();
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
        if (TeamFlow.GetState() == TeamFlowState.Wait) {
            if (GameObject.Find("GameFlow").GetComponent<TeamFlowScript>().GetNowTeam() == Team.Blue)
            {
                if (!GameObject.Find("GameFlow").GetComponent<BallFlowScript>().IsPreparedJack())
                {
                    Vector2 throwPow = Vector2.zero;
                    throwPow.y = Random.Range(0.4f, 0.7f);
                    throwBallControler.SetThrowPow(throwPow);
                    throwBallControler.ThrowBall();
                    m_IsEnable = false;
                }
                else
                {
                    //ジャックボールの位置
                    JackPos = JackBoll.transform.position;
                    //投げたい位置を計算
                    Vector3 TargetPos = JackPos - armScript.GetPosition();
                    TargetPos.y = 0.0f;
                    //方向を調整
                    {
                        //正規化して投げる方向だけにする
                        Vector3 TargetNorm = TargetPos.normalized;
                        //投げるプレイヤーの前方向
                        Vector3 ThrowForward = new Vector3(0.0f,0.0f,1.0f);
                        //プレイヤーをジャックボールの方向に向ける
                        ThrowTrance.rotation.SetFromToRotation(ThrowForward, TargetNorm);
                        float angle = Vector3.Angle(ThrowForward, TargetNorm);
                        throwAngleController.SetAngle(angle);
                        //Debug.LogError("プレイヤーの角度" + angle);

                    }
                    //投げる力を調整
                    {
                        Vector2 throwPow = Vector2.zero;
                        Vector3 dis = TargetPos - armScript.GetPosition();
                        //Debug.LogError("プレイヤーとジャックボールの差分" + dis.magnitude);
                        const float coatSize = 12.5f;
                        float ThrowMax = coatSize - armScript.GetPosition().z;
                        //Debug.LogError("プレイヤーからコートの最奥まで" + ThrowMax);
                        float power = dis.magnitude / ThrowMax;
                        //Debug.LogError("投げる力。" + power);
                        float scat = Random.Range(-0.1f, 0.1f);
                        throwPow.y = power + scat;
                        //Debug.LogError("補完後" + throwPow.y);
                        throwBallControler.SetThrowPow(throwPow);
                        throwBallControler.SetThrowPosition(new Vector3( throwPow.x , throwPow.y,0.0f));
                    }
                    if (!throwBallControler.IsDecision())
                    {
                        throwBallControler.ThrowBall();
                        m_IsEnable = false;
                    }
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
