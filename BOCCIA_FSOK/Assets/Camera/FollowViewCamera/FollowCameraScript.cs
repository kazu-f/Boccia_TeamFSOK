using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCameraScript : MonoBehaviour
{
    private Camera m_Camera = null;
    private Vector3 m_CameraPos = Vector3.zero;
    private Vector3 m_CameraTarget = Vector3.zero;
    private GameObject m_TargetBall = null;
    private Vector3 m_MoveSpeed = Vector3.zero;
    private void Awake()
    {
        m_Camera = this.GetComponent<Camera>();
        if(m_Camera == null)
        {
            Debug.LogError("Camera���擾�ł��܂���ł���");
        }
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(m_TargetBall != null)
        {
            //�^�[�Q�b�g�ƂȂ�{�[��������Ƃ�
            //���̃{�[���̃|�W�V�������J�����̃^�[�Q�b�g�Ƃ���
            m_CameraTarget = m_TargetBall.transform.position;
            //�J�����̈ʒu���^�[�Q�b�g�̈ʒu�ɃZ�b�g
            m_CameraPos = m_CameraTarget;
            //�J�����̈ʒu�ɉ��Z����x�N�g��
            Vector3 AddCameraPos = Vector3.zero;
            //�J�����̈ʒu�ɉ��Z
            m_CameraPos += AddCameraPos;
        }
    }

    public void SetBall(GameObject ball)
    {
        m_TargetBall = ball;
    }
}
