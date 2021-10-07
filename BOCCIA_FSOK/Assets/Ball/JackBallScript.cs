using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JackBallScript : MonoBehaviour
{
    private GameObject m_GameFlow = null;
    private TeamFlowScript m_TeamFlow = null;
    public Vector3 m_DefaultPos = Vector3.zero;
    private Rigidbody m_rigidbody = null;
    private void Awake()
    {
        //m_BallStateScript = GetComponent<BallStateScript>();
        //RigidBody���擾
        m_rigidbody = GetComponent<Rigidbody>();
        if(m_rigidbody == null)
        {
            Debug.LogError("RigidBody�R���|�[�l���g�̎擾�Ɏ��s���܂���");
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        m_GameFlow = GameObject.Find("GameFlow");
        if(m_GameFlow == null)
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

    // Update is called once per frame
    void Update()
    {
        //TeamFlow�ɃW���b�N�{�[���̈ʒu��ۑ�
        m_TeamFlow.SetJackPos(this.transform.position);
    }

    /// <summary>
    /// �W���b�N�{�[�����͈͊O�ɏo�����̏���
    /// </summary>
    public void OutsideVenue()
    {
        //���x���[���ɂ���
        m_rigidbody.velocity = Vector3.zero;
        //�N���X�ɖ߂�
        m_rigidbody.position = m_DefaultPos;
    }
}
