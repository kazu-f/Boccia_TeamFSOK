using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPinchInOut : MonoBehaviour
{
    Camera camera;
    float m_time = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        camera = this.GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        m_time += Time.deltaTime;
        camera.orthographicSize = Mathf.Abs(Mathf.Sin(m_time) * 3.0f);
    }
}
