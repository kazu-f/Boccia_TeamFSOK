using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCameraScript : MonoBehaviour
{
    private Camera m_Camera = null;
    private Vector3 m_CameraPos = Vector3.zero;
    private Vector3 m_CameraTarget = Vector3.zero;
    private Vector3 m_moveForward = Vector3.zero;
    private void Awake()
    {
        m_Camera = this.GetComponent<Camera>();
        if (m_Camera == null)
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
        //�^�[�Q�b�g�ƂȂ�{�[��������Ƃ�
        //���̃{�[���̃|�W�V�������J�����̃^�[�Q�b�g�Ƃ���
        m_CameraTarget.y = 0.5f;
        //�J�����̈ʒu���^�[�Q�b�g�̈ʒu�ɃZ�b�g
        m_CameraPos = m_CameraTarget;
        //�^�[�Q�b�g�̌�����
        //Vector3 TargetForward = m_TargetBall.transform.forward;
        m_moveForward.y = 0.0f;
        m_moveForward *= -1.0f;
        m_moveForward.Normalize();
        //�J�����̈ʒu�ɉ��Z����x�N�g��
        Vector3 AddCameraPos = Vector3.zero;
        AddCameraPos += m_moveForward * 2.5f;
        //�J�����̈ʒu�ɉ��Z
        m_CameraPos += AddCameraPos;
        m_CameraPos.y = 1.0f;
        //�J�����̑O����
        Vector3 CameraForward = m_CameraTarget;
        CameraForward -= m_CameraPos;
        CameraForward.Normalize();
        //�ʒu�ƑO�������Z�b�g
        this.transform.position = m_CameraPos;
        this.transform.forward = CameraForward;
    }

    public void SetBallPosition(Vector3 pos)
    {
        m_CameraTarget = pos;
    }

    public void SetBallForward(Vector3 forward)
    {
        m_moveForward = forward;
    }

}
