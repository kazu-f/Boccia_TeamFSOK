using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallOperateScript : MonoBehaviour
{
    private BallStateScript m_StateScript;
    private Rigidbody m_rigidbody;
    private bool m_IsCalculated = false;

    // Start is called before the first frame update
    void Start()
    {
        m_StateScript = GetComponent<BallStateScript>();
        //RigidBodyを取得
        m_rigidbody = GetComponent<Rigidbody>();
        AddForce(new Vector3(0.0f, 0.0f, 10.0f));
    }

    // Update is called once per frame
    void Update()
    {
        if(m_IsCalculated == false)
        {
            if(m_StateScript.GetState() == BallState.Stop)
            {

            }
        }
    }

    /// <summary>
    /// リジッドボディに速度を加算
    /// </summary>
    /// <param name="speed">加算する速度</param>
    void AddForce(Vector3 speed)
    {
        //速度を加算
        m_rigidbody.AddForce(speed);
        //m_rigidbody.velocity += speed;
    }
}
