using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallOperateScript : MonoBehaviour
{
    private Rigidbody m_rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        //�R���|�[�l���g���擾
        m_rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// ���W�b�h�{�f�B�ɑ��x�����Z
    /// </summary>
    /// <param name="speed">���Z���鑬�x</param>
    void AddSpeed(Vector3 speed)
    {
        //���x�����Z
        m_rigidbody.AddForce(speed);
        //m_rigidbody.velocity += speed;
    }
}
