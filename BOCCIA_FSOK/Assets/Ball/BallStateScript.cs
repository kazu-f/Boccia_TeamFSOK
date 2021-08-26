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
    public float m_BorderSpeed = 0.005f;        //~‚Ü‚Á‚Ä‚¢‚é‚©‚ÌŠî€‚Æ‚È‚é‘¬“x
    private Vector3 m_moveSpeed = Vector3.zero;     //ˆÚ“®‘¬“x

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

        if(m_moveSpeed.magnitude > m_BorderSpeed)
        {
            //ˆÚ“®’†‚É‚·‚é
            m_state = BallState.Move;
        }
        else
        {
            //‘¬“x‚ªˆê’èˆÈ‰º‚Ì
            //ˆÚ“®‚ğ’â~‚·‚é
            m_rigidbody.velocity = Vector3.zero;
            //’â~’†‚É‚·‚é
            m_state = BallState.Stop;
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

}
