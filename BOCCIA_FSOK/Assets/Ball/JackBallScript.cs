using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum JackBallState
{
    Start,
    Move,
    End,
    Num,
}

public class JackBallScript : MonoBehaviour
{
    private JackBallState m_state = JackBallState.Num;
    //�W���b�N�{�[���̖߂�ׂ��ʒu
    private Vector3 m_OriginPos = new Vector3(0.0f, 0.0f, 0.0f);
    //�W���b�N�{�[���̈ʒu
    private Vector3 m_pos = Vector3.zero;
    //���E��
    public float m_CourtHeight;
    public float m_CourtWidth;

    private Rigidbody m_rigidbody;
    private const float m_BorderSpeed = 0.05f;
    // Start is called before the first frame update
    void Start()
    {
        m_state = JackBallState.Start;
        //RigidBody���擾
        m_rigidbody = GetComponent<Rigidbody>();
        //�͈͂𔼕��ɂ���
        m_CourtHeight /= 2.0f;
        m_CourtWidth /= 2.0f;
    }

    // Update is called once per frame
    void Update()
    {
        //�ړ����x���擾
        Vector3 m_moveSpeed = m_rigidbody.velocity;
        //xz���̈ړ��̂݌���
        m_moveSpeed.y = 0.0f;
        //���x�����ȏ�̎�
        if (m_moveSpeed.magnitude > m_BorderSpeed)
        {
            //�ړ����ɂ���
            m_state = JackBallState.Move;
        }
        else
        {
            //���x�����ȉ��̎�
            //�ړ����~����
            m_rigidbody.velocity = Vector3.zero;
            //��~���ɂ���
            m_state = JackBallState.End;
        }

        //�I�u�W�F�N�g��transform���擾
        Transform myTrans = this.transform;

        //transform������W���擾
        m_pos = myTrans.position;

        switch (m_state)
        {
            case JackBallState.Move:
                //�I�u�W�F�N�g�����E�����z���Ă�����ʒu���N���X�ɖ߂�
                if (m_pos.x > m_CourtWidth || m_pos.x < m_CourtWidth * -1.0f)
                {
                    //�͈͊O�̂��ߋ}���Ɏ~�߂�
                    m_rigidbody.velocity += m_moveSpeed * -0.5f * Time.deltaTime;

                }
                else if (m_pos.z > m_CourtHeight || m_pos.z < m_CourtHeight * -1.0f)
                {
                    //�͈͊O�̂��ߋ}���Ɏ~�߂�
                    m_rigidbody.velocity += m_moveSpeed * -0.5f * Time.deltaTime;
                }

                break;

            case JackBallState.End:
                //�I�u�W�F�N�g�����E�����z���Ă�����ʒu���N���X�ɖ߂�
                if (m_pos.x > m_CourtWidth || m_pos.x < m_CourtWidth * -1.0f)
                {
                    //�N���X�ɖ߂�
                    m_pos = m_OriginPos;

                }
                else if (m_pos.z > m_CourtHeight || m_pos.z < m_CourtHeight * -1.0f)
                {
                    //�N���X�ɖ߂�
                    m_pos = m_OriginPos;
                }

                break;
        }

        //���W��ݒ�
        myTrans.position = m_pos;

    }
}
