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
        //RigidBody‚ğæ“¾
        m_rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //ˆÚ“®‘¬“x‚ğæ“¾
        m_moveSpeed = m_rigidbody.velocity;

        //xz²‚ÌˆÚ“®‚Ì‚İŒ©‚é
        m_moveSpeed.y = 0.0f;

        if (m_state == BallState.Move)
        {
            if (m_moveSpeed.magnitude <= m_borderSpeed)
            {
                //‘¬“x‚ªˆê’èˆÈ‰º‚Ì
                //ˆÚ“®‚ğ’â~‚·‚é
                m_rigidbody.velocity = Vector3.zero;
                //’â~’†‚É‚·‚é
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
    
    /// <summary>
    /// ƒ{[ƒ‹‚ğ~‚ß‚éˆ—
    /// </summary>
    public void Stop()
    {
        //ˆÚ“®‚ğ’â~‚·‚é
        m_rigidbody.velocity = Vector3.zero;
        //’â~’†‚É‚·‚é
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
