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
    /// �v���C���[���Z�b�g
    /// PlayerController�ƈꏏ
    /// </summary>
    override public void ResetPlayer()
    {
        ballHolderController.ResetBall();
    }
    public override void SwitchPlayer(bool isEnable)
    {
        m_IsEnable = isEnable;
        //�J�n���_�̃g�����X�t�H�[���ֈړ��B
        if (isEnable == true)
        {
            //�v���C���[���؂�ւ�鎞�ɃJ�����̈ʒu�����킹��B
            throwAngleController.ChangeCamPos();
            timer = 0.0f;
            angleTimer = 0.0f;
            powerTimer = 0.0f;
            swichActiveGameObj.SwitchGameObject(false);
            //�����鎞�̃p�����[�^���v�Z���Ă����B
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

        //�X�e�[�g�Ő؂�ւ���B
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
        //������l�̓e�L�g�[
        return 1.0f;
    }

    private void CalcAngle()
    {
        if(!ballFlow.IsPreparedJack())
        {
            //�W���b�N�{�[���𓊂��������K���ɁB
            throwAngle = Random.Range(-3.0f, 3.0f);
        }
        else
        {
            //���������ʒu���v�Z
            Vector3 TargetPos = JackBall.transform.position - armScript.GetPosition();
            TargetPos.y = 0.0f;
            //�����𒲐�
            //���K�����ē�������������ɂ���
            Vector3 TargetNorm = TargetPos.normalized;
            //������v���C���[�̑O����
            Vector3 ThrowForward = new Vector3(0.0f, 0.0f, 1.0f);
            //�v���C���[���W���b�N�{�[���̕����Ɍ�����
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
            //���������ʒu���v�Z
            Vector3 TargetPos = JackBall.transform.position - armScript.GetPosition();
            TargetPos.y = 0.0f;
            throwPower = Vector2.zero;
            Vector3 dis = TargetPos - armScript.GetPosition();
            //Debug.LogError("�v���C���[�ƃW���b�N�{�[���̍���" + dis.magnitude);
            const float coatSize = 12.5f;
            float ThrowMax = coatSize - armScript.GetPosition().z;
            //Debug.LogError("�v���C���[����R�[�g�̍ŉ��܂�" + ThrowMax);
            float power = dis.magnitude / ThrowMax;
            //Debug.LogError("������́B" + power);
            float scat = Random.Range(-0.2f, 0.0f);
            throwPower.y = power + scat;
            throwPower.x = Random.Range(-0.1f, 0.1f);
        }
    }

    //������ς��Ă����B
    private void SetThrowAngle()
    {
        //�^�C�}�[��i�߂�B
        angleTimer += Time.deltaTime * DicisionTime;

        float angle = Mathf.Lerp(0.0f, throwAngle, Mathf.Min(1.0f,angleTimer / DicisionTime));

        throwAngleController.SetAngle(angle);

        if(angleTimer > DicisionTime)
        {
            state = AIState.AI_SetThrowPow;
        }
    }

    //������͂�ς��Ă����B
    private void SetThrowPower()
    {
        if(!throwBallControler.enabled)
        {
            throwBallControler.enabled = true;
            Vector2 startPos = throwPower;
            startPos.x += 0.5f;
            throwBallControler.StartThrowBall(startPos);
        }

        //�^�C�}�[��i�߂�B
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
