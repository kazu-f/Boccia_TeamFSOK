using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BallState
{
    Start,
    Move,
    End,
    Num,
}
public class BallStateScript : MonoBehaviour
{
    private BallState m_state = BallState.Num;
    private Rigidbody m_rigidbody;
    public float m_BorderSpeed = 0.005f;        //�~�܂��Ă��邩�̊�ƂȂ鑬�x
    private Vector3 m_moveSpeed = Vector3.zero;     //�ړ����x

    // Start is called before the first frame update
    void Start()
    {
        //�X�e�[�g���J�n�ɃZ�b�g
        m_state = BallState.Start;
        //RigidBody���擾
        m_rigidbody = GetComponent<Rigidbody>();

    }

    // Update is called once per frame
    void Update()
    {
        //�ړ����x���擾
        m_moveSpeed = m_rigidbody.velocity;
        //xz���̈ړ��̂݌���
        m_moveSpeed.y = 0.0f;

        switch(m_state)
        {
            case BallState.Start:
                //���x�����ȏ�̎�
                if (m_moveSpeed.magnitude > m_BorderSpeed)
                {
                    //�ړ����ɂ���
                    m_state = BallState.Move;
                }
                else
                {
                    //���x�����ȉ��̎�
                    //�ړ����~����
                    m_rigidbody.velocity = Vector3.zero;
                    //��~���ɂ���
                    m_state = BallState.End;
                }
                break;

            case BallState.Move:
                if (m_moveSpeed.magnitude < m_BorderSpeed)
                {
                    //���x�����ȉ��̎�
                    //�ړ����~����
                    m_rigidbody.velocity = Vector3.zero;
                    //��~���ɂ���
                    m_state = BallState.End;
                }
                break;
            
            case BallState.End:
                //���������ɏI��
                return;
        }

    }

    public BallState GetState()
    {
        return m_state;
    }
    
    public Rigidbody GetRigidbody()
    {
        return m_rigidbody;
    }

    public Vector3 GetMoveSpeed()
    {
        return m_moveSpeed;
    }

    public void ReStart()
    {
        m_state = BallState.Start;
    }
}
