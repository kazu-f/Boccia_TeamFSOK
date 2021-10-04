using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BallState
{
    Move,
    Stop,
    Num,
}
public class BallStateScript : MonoBehaviour
{
    private BallState m_state = BallState.Num;
    private Rigidbody m_rigidbody;
    private Vector3 m_moveSpeed;
    private float m_borderSpeed = 0.005f;
    // Start is called before the first frame update
    void Start()
    {
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

        if (m_state == BallState.Move)
        {
            if (m_moveSpeed.magnitude <= m_borderSpeed)
            {
                //���x�����ȉ��̎�
                //�ړ����~����
                m_rigidbody.velocity = Vector3.zero;
                //��~���ɂ���
                m_state = BallState.Stop;
            }
        }

        if (m_moveSpeed.magnitude > m_borderSpeed)
        {
            //�ړ����ɂ���
            m_state = BallState.Move;
        }
    }

    public BallState GetState()
    {
        return m_state;
    }
    
    /// <summary>
    /// �{�[�����~�߂鏈��
    /// </summary>
    public void Stop()
    {
        //�ړ����~����
        m_rigidbody.velocity = Vector3.zero;
        //��~���ɂ���
        m_state = BallState.Stop;
    }

    public Vector3 GetMoveSpeed()
    {
        return m_moveSpeed;
    }

    public void ResetState()
    {
        m_state = BallState.Num;
    }
}
