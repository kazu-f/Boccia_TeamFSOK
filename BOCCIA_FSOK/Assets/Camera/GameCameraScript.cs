using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCameraScript : MonoBehaviour
{
    private bool IsFollow = false;
    public GameObject m_MainCamera = null;
    public GameObject m_FollowCamera = null;
    public Vector3 m_CameraPos = Vector3.zero;
    //public Vector3 m_CameraForward = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        m_MainCamera.SetActive(!IsFollow);
        m_FollowCamera.SetActive(IsFollow);
    }

    // Update is called once per frame
    void Update()
    {
    }

    /// <summary>
    /// ���C���J�����ƒǔ��J������؂�ւ���
    /// </summary>
    public void SwitchCamera()
    {
        IsFollow = !IsFollow;
        m_FollowCamera.SetActive(IsFollow);
        m_MainCamera.SetActive(!IsFollow);
    }

    /// <summary>
    /// �ǔ��J�����Ƀp�����[�^���Z�b�g����
    /// </summary>
    /// <param name="pos">�ǂ�������{�[���̈ʒu</param>
    /// <param name="forward">�ǂ�������{�[���̑O����</param>
    public void SetFollowCameraParam(Vector3 pos)
    {
        m_FollowCamera.GetComponent<FollowCameraScript>().SetBallPosition(pos);
    }
}
