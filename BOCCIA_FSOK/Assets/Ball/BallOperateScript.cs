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
        //�R���|�[�l���g���擾
        m_rigidbody = m_StateScript.GetRigidbody();
        AddForce(new Vector3(0.0f, 0.0f, 10.0f));
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// ���W�b�h�{�f�B�ɑ��x�����Z
    /// </summary>
    /// <param name="speed">���Z���鑬�x</param>
    void AddForce(Vector3 speed)
    {
        //���x�����Z
        m_rigidbody.AddForce(speed);
        //m_rigidbody.velocity += speed;
    }
}
