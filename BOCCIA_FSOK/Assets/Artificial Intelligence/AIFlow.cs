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
    private TeamFlowScript TeamFlow;
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
        //�J�n���_�̃g�����X�t�H�[���ֈړ��B
        if (isEnable == true)
        {
            //�v���C���[���؂�ւ�鎞�ɃJ�����̈ʒu�����킹��B
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
        TeamFlow = GameObject.Find("GameFlow").GetComponent<TeamFlowScript>();
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
        if (TeamFlow.GetState() == TeamFlowState.Wait) {
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
                    //�W���b�N�{�[���̈ʒu
                    JackPos = JackBoll.transform.position;
                    //���������ʒu���v�Z
                    Vector3 TargetPos = JackPos - ThrowTrance.position;
                    //�����𒲐�
                    {
                        //���K�����ē�������������ɂ���
                        Vector3 TargetNorm = TargetPos.normalized;
                        //������v���C���[�̑O����
                        Vector3 ThrowForward = ThrowTrance.forward;
                        //�v���C���[���W���b�N�{�[���̕����Ɍ�����
                        ThrowTrance.rotation.SetFromToRotation(ThrowForward, TargetNorm);
                        float angle = Vector3.Dot(ThrowTrance.forward, ThrowForward);
                        throwAngleController.SetAngle(0);
                        Debug.LogError("�v���C���[�̊p�x" + angle);

                    }
                    //������͂𒲐�
                    {
                        Vector2 throwPow = Vector2.one;
                        Vector3 dis = TargetPos - ThrowTrance.position;
                        //Debug.LogError("�v���C���[�ƃW���b�N�{�[���̍���" + dis.magnitude);
                        const float coatSize = 12.5f;
                        float ThrowMax = coatSize - ThrowTrance.position.z;
                        //Debug.LogError("�v���C���[����R�[�g�̍ŉ��܂�" + ThrowMax);
                        float power = dis.magnitude / ThrowMax;
                        //Debug.LogError("������́B" + power);
                        float scat = Random.Range(-0.1f, 0.1f);
                        throwPow.y = power /*+ scat*/;
                        //Debug.LogError("�⊮��" + throwPow.y);
                        throwBallControler.SetThrowPow(throwPow);
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
