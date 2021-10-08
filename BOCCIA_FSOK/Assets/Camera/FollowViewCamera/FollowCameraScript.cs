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
        if(m_TargetBall != null)
        {
            //ターゲットとなるボールがあるとき
            //そのボールのポジションをカメラのターゲットとする
            m_CameraTarget = m_TargetBall.transform.position;
            //カメラの位置をターゲットの位置にセット
            m_CameraPos = m_CameraTarget;
            //カメラの位置に加算するベクトル
            Vector3 AddCameraPos = Vector3.zero;
            //カメラの位置に加算
            m_CameraPos += AddCameraPos;
        }
    }

    public void SetBall(GameObject ball)
    {
        m_TargetBall = ball;
    }
}
