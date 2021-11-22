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
            //Debug.LogError("JackBoll��������܂���ł����B");
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
                    //Debug.LogError("�W���b�N�{�[���𓊂��܂��B"+throwPow.x+ throwPow.y);
                    timer = 0;
                }
            }
            else
            {
                //�W���b�N�{�[���̈ʒu
                JackPos = JackBoll.transform.position;
                //���������ʒu���v�Z
                Vector3 TargetPos = JackPos - ThrowTrance.position;
                //���K�����ē�������������ɂ���
                Vector3 TargetNorm = TargetPos.normalized;
                //������v���C���[�̑O����
                Vector3 ThrowForward = ThrowTrance.forward;
                //�v���C���[���W���b�N�{�[���̕����Ɍ�����
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
                    Debug.LogError("�}�C�{�[���𓊂��܂��B" + throwPow.x + throwPow.y);
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
        //������l�̓e�L�g�[
        return 1.0f;
    } 
}
