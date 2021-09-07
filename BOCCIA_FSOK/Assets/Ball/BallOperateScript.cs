using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallOperateScript : MonoBehaviour
{
    private BallStateScript m_StateScript = null;
    private Rigidbody m_rigidbody = null;
    private bool m_IsCalculated = false;
    private GameObject m_Flow = null;       //�Q�[���̗���S�̂��R���g���[������I�u�W�F�N�g
    private TeamFlowScript m_TeamFlow = null;
    // Start is called before the first frame update
    void Start()
    {
        //�I�u�W�F�N�g���擾
        m_Flow = GameObject.Find("GameFlow");
        if(m_Flow == null)
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

        //�{�[���̏�Ԃ𑀍삷��X�N���v�g���擾
        m_StateScript = GetComponent<BallStateScript>();
        if(m_StateScript == null)
        {
            //BallStateScript�R���|�[�l���g���擾�ł��Ȃ������Ƃ�
            Debug.LogError("�G���[�FBallStateScript�R���|�[�l���g�̎擾�Ɏ��s���܂����B");
        }
        //RigidBody���擾
        m_rigidbody = GetComponent<Rigidbody>();
        if(m_rigidbody == null)
        {
            //Rigidbody�R���|�[�l���g���擾�ł��Ȃ������Ƃ�
            Debug.LogError("�G���[�FRigidbody�R���|�[�l���g�̎擾�Ɏ��s���܂����B");
        }

        //AddForce(new Vector3(10.0f,0.0f,0.0f));
        m_TeamFlow.DecreaseBalls();
    }

    // Update is called once per frame
    void Update()
    {
        if(m_IsCalculated == false)
        {
            if(m_StateScript.GetState() == BallState.Stop)
            {
                m_IsCalculated = m_TeamFlow.CalucNextTeam();
            }
        }
    }

    /// <summary>
    /// ���W�b�h�{�f�B�ɑ��x�����Z
    /// </summary>
    /// <param name="speed">���Z���鑬�x</param>
    void AddForce(Vector3 speed)
    {
        //���x�����Z
        m_rigidbody.AddForce(speed);
        m_TeamFlow.SetMove();
    }
}
