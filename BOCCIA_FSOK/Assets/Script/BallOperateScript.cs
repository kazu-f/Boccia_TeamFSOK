using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallOperateScript : MonoBehaviour
{
    private Rigidbody m_rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        //コンポーネントを取得
        m_rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// リジッドボディに速度を加算
    /// </summary>
    /// <param name="speed">加算する速度</param>
    void AddSpeed(Vector3 speed)
    {
        //速度を加算
        m_rigidbody.AddForce(speed);
        //m_rigidbody.velocity += speed;
    }
}
