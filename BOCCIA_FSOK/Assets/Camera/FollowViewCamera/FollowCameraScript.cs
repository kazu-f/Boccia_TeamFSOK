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
            Debug.LogError("Cameraが取得できませんでした");
        }
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //ターゲットとなるボールがあるとき
        //そのボールのポジションをカメラのターゲットとする
        m_CameraTarget.y = 0.5f;
        //カメラの位置をターゲットの位置にセット
        m_CameraPos = m_CameraTarget;
        //ターゲットの後ろ方向
        //Vector3 TargetForward = m_TargetBall.transform.forward;
        m_moveForward.y = 0.0f;
        m_moveForward *= -1.0f;
        m_moveForward.Normalize();
        //カメラの位置に加算するベクトル
        Vector3 AddCameraPos = Vector3.zero;
        AddCameraPos += m_moveForward * 2.5f;
        //カメラの位置に加算
        m_CameraPos += AddCameraPos;
        m_CameraPos.y = 1.0f;
        //カメラの前方向
        Vector3 CameraForward = m_CameraTarget;
        CameraForward -= m_CameraPos;
        CameraForward.Normalize();
        //位置と前方向をセット
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
