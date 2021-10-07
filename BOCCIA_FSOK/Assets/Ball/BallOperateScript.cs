using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallOperateScript : MonoBehaviour
{
    private BallStateScript m_StateScript = null;
    private TeamDivisionScript m_Team = null;
    private Rigidbody m_rigidbody = null;
    private GameObject m_Flow = null;       //�Q�[���̗���S�̂��R���g���[������I�u�W�F�N�g
    private TeamFlowScript m_TeamFlow = null;
    public Vector3 DefaultPos;
    private Vector3 m_pos = Vector3.zero;
    private void Awake()
    {
        //RigidBody���擾
        m_rigidbody = GetComponent<Rigidbody>();
        if (m_rigidbody == null)
        {
            //Rigidbody�R���|�[�l���g���擾�ł��Ȃ������Ƃ�
            Debug.LogError("�G���[�FRigidbody�R���|�[�l���g�̎擾�Ɏ��s���܂����B");
        }

        //�I�u�W�F�N�g���擾
        m_Flow = GameObject.Find("GameFlow");
        if (m_Flow == null)
        {
            //�C���X�^���X���쐬����Ă��Ȃ��Ƃ�
            Debug.LogError("�G���[�FGameFlow�̃C���X�^���X���쐬����Ă��܂���B");
        }
        else
        {
            //���ɓ�����`�[�������߂�R���|�[�l���g���擾
            m_TeamFlow = m_Flow.GetComponent<TeamFlowScript>();
            if (m_TeamFlow == null)
            {
                //TeamFlowScript�R���|�[�l���g���擾�ł��Ȃ������Ƃ�
                Debug.LogError("�G���[�FTeamFlowScript�R���|�[�l���g�̎擾�Ɏ��s���܂����B");
            }
        }

        m_Team = GetComponent<TeamDivisionScript>();
        if (m_Team == null)
        {
            //TeamDivisionScript�R���|�[�l���g���擾�ł��Ȃ������Ƃ�
            Debug.LogError("�G���[�FTeamDivisionScript�R���|�[�l���g�̎擾�Ɏ��s���܂����B");
        }
        //�{�[���̏�Ԃ𑀍삷��X�N���v�g���擾
        m_StateScript = GetComponent<BallStateScript>();
        if (m_StateScript == null)
        {
            //BallStateScript�R���|�[�l���g���擾�ł��Ȃ������Ƃ�
            Debug.LogError("�G���[�FBallStateScript�R���|�[�l���g�̎擾�Ɏ��s���܂����B");
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
    /// ���W�b�h�{�f�B�ɑ��x�����Z
    /// </summary>
    /// <param name="speed">���Z���鑬�x</param>
    public void AddForce(Vector3 speed)
    {
        if (m_Team.GetTeam() != Team.Jack)
        {
            m_TeamFlow.DecreaseBalls();
        }
        //���x�����Z
        m_rigidbody.AddForce(speed);
        //�{�[���𓊂��������TeamFlow�ɑ���
        m_TeamFlow.ThrowBall();
        Debug.Log("�{�[���������Ă��܂�");
    }

    /// <summary>
    /// ��O�Ƀ{�[�����s�������̏���
    /// </summary>
    public void OutsideVenue()
    {
        //�I�u�W�F�N�g��transform���擾
        Transform myTrans = this.transform;
        //transform������W���擾
        m_pos = myTrans.position;

        //�{�[�����~����
        m_StateScript.Stop();

        if (m_Team.GetTeam() == Team.Jack)
        {
            //�W���b�N�{�[���̏ꍇ
            //�N���X�ɖ߂�
            m_pos = DefaultPos;
            //TeamFlow�ɃW���b�N�{�[���̈ʒu��ۑ�
            m_TeamFlow.SetJackPos(m_pos);

            //���W��ݒ�
            myTrans.position = m_pos;
        }
        else
        {
            //�W���b�N�{�[���ȊO�̏ꍇ
            //ball�Ƃ����^�O�̂����Q�[���I�u�W�F�N�g��z��ɓ����
            GameObject[] m_balls;
            m_balls = GameObject.FindGameObjectsWithTag("Ball");
            if (m_balls.Length == 0)
            {
                m_TeamFlow.ChangeNextTeam();
            }
            else
            {
                //���ɂ��{�[��������̂Ŏ��ɓ�����`�[�����v�Z����
                m_TeamFlow.CalucNextTeam();
            }
        }
        m_TeamFlow.NextTeamLog();
        if (m_Team.GetTeam() != Team.Jack)
        {
            this.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// �ϐ��̃��Z�b�g�B
    /// </summary>
    public void ResetVar()
    {
        m_StateScript.ResetState();
    }
}
