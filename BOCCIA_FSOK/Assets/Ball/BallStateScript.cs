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
        //RigidBody‚ðŽæ“¾
        m_rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //ˆÚ“®‘¬“x‚ðŽæ“¾
        m_moveSpeed = m_rigidbody.velocity;

        //xzŽ²‚ÌˆÚ“®‚Ì‚ÝŒ©‚é
        m_moveSpeed.y = 0.0f;

        if (m_state == BallState.Move)
        {
            if (m_moveSpeed.magnitude <= m_borderSpeed)
            {
                //‘¬“x‚ªˆê’èˆÈ‰º‚ÌŽž
                //ˆÚ“®‚ð’âŽ~‚·‚é
                m_rigidbody.velocity = Vector3.zero;
                //’âŽ~’†‚É‚·‚é
                m_state = BallState.Stop;
            }
        }

        if (m_moveSpeed.magnitude > m_borderSpeed)
        {
            //ˆÚ“®’†‚É‚·‚é
            m_state = BallState.Move;
        }
    }

    public BallState GetState()
    {
        return m_state;
    }
    
    public Vector3 GetMoveSpeed()
    {
        return m_moveSpeed;
    }
}
