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
    private GameObject m_GameCamera = null;
    private GameCameraScript m_GameCameraScript = null;
    private bool IsThrowing = true;

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
        m_GameCamera = GameObject.Find("GameCamera");
        if (m_GameCamera == null)
        {
            Debug.LogError("GameCamera�̎擾�Ɏ��s���܂���");
        }
        else
        {
            m_GameCameraScript = m_GameCamera.GetComponent<GameCameraScript>();
            if(m_GameCameraScript == null)
            {
                Debug.LogError("GameCameraScript�̎擾�Ɏ��s���܂���");
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
        if (IsThrowing)
        {
            Vector3 position = this.gameObject.transform.position;
            m_GameCameraScript.SetFollowCameraParam(position);
        }
    }

    /// <summary>
    /// ���W�b�h�{�f�B�ɑ��x�����Z
    /// </summary>
    /// <param name="speed">���Z���鑬�x</param>
    public void Throw(Vector3 speed)
    {
        if (m_Team.GetTeam() != Team.Jack)
        {
            m_TeamFlow.DecreaseBalls();
        }
        //���x�����Z
        m_rigidbody.AddForce(speed);
        //�{�[���𓊂��������TeamFlow�ɑ���
        m_TeamFlow.ThrowBall();
        //�J������؂�ւ���
        m_GameCameraScript.SwitchCamera();
        Debug.Log("�{�[���������Ă��܂�");
    }

    /// <summary>
    /// �ϐ��̃��Z�b�g�B
    /// </summary>
    public void ResetVar()
    {
        IsThrowing = true;
        m_StateScript.ResetState();
    }

    /// <summary>
    /// �����I�����
    /// </summary>
    public void EndThrowing()
    {
        IsThrowing = false;
    }
}
