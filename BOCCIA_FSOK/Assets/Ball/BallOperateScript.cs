using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallOperateScript : MonoBehaviour
{
    private BallStateScript m_StateScript;
    private Rigidbody m_rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        m_StateScript = GetComponent<BallStateScript>();
        //コンポーネントを取得
        m_rigidbody = m_StateScript.GetRigidbody();
        AddForce(new Vector3(0.0f, 0.0f, 10.0f));
    }

    // Update is called once per frame
    void Update()
    {

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
