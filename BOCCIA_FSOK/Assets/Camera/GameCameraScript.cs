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
    /// メインカメラと追尾カメラを切り替える
    /// </summary>
    public void SwitchCamera()
    {
        IsFollow = !IsFollow;
        m_FollowCamera.SetActive(IsFollow);
        m_MainCamera.SetActive(!IsFollow);
    }

    /// <summary>
    /// 追尾カメラにパラメータをセットする
    /// </summary>
    /// <param name="pos">追いかけるボールの位置</param>
    /// <param name="forward">追いかけるボールの前方向</param>
    public void SetFollowCameraParam(Vector3 pos)
    {
        m_FollowCamera.GetComponent<FollowCameraScript>().SetBallPosition(pos);
    }
}
