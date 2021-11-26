using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JackBallScript : IBallScript
{
    private GameObject m_GameFlow = null;
    private TeamFlowScript m_TeamFlow = null;
    public Vector3 m_DefaultPos = Vector3.zero;
    private Rigidbody m_rigidbody = null;
    private void Awake()
    {
        //RigidBody���擾
        m_rigidbody = GetComponent<Rigidbody>();
        if(m_rigidbody == null)
        {
            Debug.LogError("RigidBody�R���|�[�l���g�̎擾�Ɏ��s���܂���");
        }
        m_GameFlow = GameObject.Find("GameFlow");
        if (m_GameFlow == null)
        {
            //�C���X�^���X���쐬����Ă��Ȃ��Ƃ�
            Debug.LogError("�G���[�FGameFlow�̃C���X�^���X���쐬����Ă��܂���B");
        }
        else
        {
            //TeamFlowScript�R���|�[�l���g���擾
            m_TeamFlow = m_GameFlow.GetComponent<TeamFlowScript>();
            if (m_TeamFlow == null)
            {
                //TeamFlowScript�R���|�[�l���g���擾�ł��Ȃ������Ƃ�
                Debug.LogError("�G���[�FTeamFlowScript�R���|�[�l���g�̎擾�Ɏ��s���܂����B");
            }
        }
    }
    // Start is called before the first frame update
    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// �{�[�����G���A���ɓ��������̏���
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
        InArea = false;
    }

    private void ResetPos()
    {
        //���x���[���ɂ���
        m_rigidbody.velocity = Vector3.zero;
        //�N���X�ɖ߂�
        m_rigidbody.position = m_DefaultPos;
    }

    /// <summary>
    /// �{�[������~�����Ƃ��̏���
    /// </summary>
    public override void EndThrow()
    {
        IsThrowing = false;
        if (InArea == false)
        {
            if (m_GameFlow.GetComponent<BallFlowScript>().IsPreparedJack())
            {
                ResetPos();
            }
            else
            {
                this.gameObject.GetComponent<BallStateScript>().ResetState();
                this.gameObject.SetActive(false);
            }
        }
    }

    /// <summary>
    /// �W���b�N�{�[���𓊂���`�[����ύX
    /// </summary>

    public override void InKillArea()
    {
        //���x���[���ɂ���
        m_rigidbody.velocity = Vector3.zero;
        this.gameObject.SetActive(false);
        m_TeamFlow.ChangeJackThrowTeam();
    }
}
