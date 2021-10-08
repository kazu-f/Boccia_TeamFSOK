using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCameraScript : MonoBehaviour
{
    private bool IsFollow = false;
    public GameObject m_MainCamera = null;
    public GameObject m_FollowCamera = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SwitchCamera()
    {
        IsFollow = !IsFollow;
        m_FollowCamera.SetActive(IsFollow);
        m_MainCamera.SetActive(!IsFollow);
    }
}
