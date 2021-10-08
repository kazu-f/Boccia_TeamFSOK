using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCameraScript : MonoBehaviour
{
    private Camera m_Camera = null;
    private Vector3 m_CameraPos = Vector3.zero;
    private Vector3 m_CameraTarget = Vector3.zero;
    private Vector3 m_AddPos = new Vector3(0.0f, 0.0f, -2.5f);
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
        //カメラの位置に加算
        m_CameraPos += m_AddPos;
        m_CameraPos.y = 1.0f;
        //カメラの前方向
        Vector3 CameraForward = m_CameraTarget;
        CameraForward -= m_CameraPos;
        CameraForward.Normalize();
        //位置と前方向をセット
        m_Camera.transform.position = m_CameraPos;
        m_Camera.transform.forward = CameraForward;
    }

    public void SetBallPosition(Vector3 pos)
    {
        m_CameraTarget = pos;
    }
}
