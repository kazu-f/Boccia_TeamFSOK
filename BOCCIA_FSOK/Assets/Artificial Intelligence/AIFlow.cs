using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BocciaPlayer;

public class AIFlow : IPlayerController
{
    private TouchInfo info;
    private Transform ThrowTrance;
    private Vector2 ThrowPower = Vector2.zero;
    private GameObject JackBoll = null;
    private Rigidbody JackBody = null;
    private Vector3 JackPos = Vector3.zero;
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
        JackBoll = GameObject.Find("JackBoll");
        if (JackBoll == null)
        {
            Debug.LogError("JackBoll��������܂���ł����B");
        }
        JackBody = JackBoll.GetComponent<Rigidbody>();
        if (JackBody == null)
        {
            Debug.LogError("JackBoll�̃��W�b�g�{�f�B��������܂���ł����B");
        }
        ThrowTrance = GameObject.Find("Players").transform;
        
    }
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        //�W���b�N�{�[���̈ʒu
        JackPos = JackBody.position;
        //���������ʒu���v�Z
        Vector3 TargetPos = JackPos - ThrowTrance.position;
        //���K�����ē�������������ɂ���
        Vector3 TargetNorm = TargetPos.normalized;
        //������v���C���[�̑O����
        Vector3 ThrowForward = ThrowTrance.forward;
        //�v���C���[���W���b�N�{�[���̕����Ɍ�����
        ThrowTrance.rotation.SetFromToRotation(ThrowForward, TargetNorm);

        Vector2 throwPow = Vector2.zero;
        throwPow.y = 1.0f;
        throwBallControler.SetThrowPow(throwPow);
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
