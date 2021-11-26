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
                    //�W���b�N�{�[���̈ʒu
                    JackPos = JackBoll.transform.position;
                    //���������ʒu���v�Z
                    Vector3 TargetPos = JackPos - armScript.GetPosition();
                    TargetPos.y = 0.0f;
                    //�����𒲐�
                    {
                        //���K�����ē�������������ɂ���
                        Vector3 TargetNorm = TargetPos.normalized;
                        //������v���C���[�̑O����
                        Vector3 ThrowForward = new Vector3(0.0f,0.0f,1.0f);
                        //�v���C���[���W���b�N�{�[���̕����Ɍ�����
                        ThrowTrance.rotation.SetFromToRotation(ThrowForward, TargetNorm);
                        float angle = Vector3.Angle(ThrowForward, TargetNorm);
                        throwAngleController.SetAngle(angle);
                        //Debug.LogError("�v���C���[�̊p�x" + angle);

                    }
                    //������͂𒲐�
                    {
                        Vector2 throwPow = Vector2.zero;
                        Vector3 dis = TargetPos - armScript.GetPosition();
                        //Debug.LogError("�v���C���[�ƃW���b�N�{�[���̍���" + dis.magnitude);
                        const float coatSize = 12.5f;
                        float ThrowMax = coatSize - armScript.GetPosition().z;
                        //Debug.LogError("�v���C���[����R�[�g�̍ŉ��܂�" + ThrowMax);
                        float power = dis.magnitude / ThrowMax;
                        //Debug.LogError("������́B" + power);
                        float scat = Random.Range(-0.1f, 0.1f);
                        throwPow.y = power + scat;
                        //Debug.LogError("�⊮��" + throwPow.y);
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
        //������l�̓e�L�g�[
        return 1.0f;
    }
}
