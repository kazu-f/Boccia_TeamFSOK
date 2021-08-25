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

    private Rigidbody m_rigidbody;
    public float m_BorderSpeed = 0.005f;
    //�G���A�����ǂ������肷�邽�߂̃I�u�W�F�N�g
    private GameObject m_CourtArea;
    //�G���A���ɂ��邩�ǂ����̃t���O
    private bool m_InArea = true;
    // Start is called before the first frame update
    void Start()
    {
        m_state = JackBallState.Start;
        //RigidBody���擾
        m_rigidbody = GetComponent<Rigidbody>();
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
                if(m_InArea == false)
                {
                    //�͈͊O�̂��ߋ}���Ɏ~�߂�
                    m_rigidbody.velocity += m_moveSpeed * -0.5f * Time.deltaTime;
                }

                break;

            case JackBallState.End:
                //�G���A�O�ɂ���Ƃ�
                if (m_InArea == false)
                {
                    //���x��0�ɃZ�b�g
                    m_rigidbody.velocity = Vector3.zero;
                    //�N���X�ɖ߂�
                    m_pos = m_OriginPos;

                    m_InArea = true;
                }

                break;
        }

        //���W��ݒ�
        myTrans.position = m_pos;

    }


    private void OnTriggerExit(Collider other)
    {
        m_InArea = false;
    }
}
