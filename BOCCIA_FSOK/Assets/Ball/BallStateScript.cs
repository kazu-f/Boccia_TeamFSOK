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
    private float m_borderSpeed = 0.0005f;
    private void Awake()
    {
        //RigidBody���擾
        m_rigidbody = this.gameObject.GetComponent<Rigidbody>();
        if (m_rigidbody == null)
        {
            Debug.LogError("���W�b�h�{�f�B�擾�ł��Ă܂���!!!");
        }
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        CalcState();
    }

    private void CalcState()
    {
        if (m_rigidbody == null)
        {
            m_state = BallState.Num;
            return;
        }
        //�ړ����x���擾
        m_moveSpeed = m_rigidbody.velocity;
        if (m_moveSpeed.magnitude <= m_borderSpeed)
        {
            //���x�����ȉ��̎�
            //�ړ����~����
            m_rigidbody.velocity = Vector3.zero;
            //��~���ɂ���
            m_state = BallState.Stop;
        }
        else
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

    public void ResetState()
    {
        m_state = BallState.Num;
    }

}