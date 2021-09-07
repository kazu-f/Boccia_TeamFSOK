using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraMove2D : MonoBehaviour
{
    public Collider collider;

    public TouchManager touchManager;   //タッチマネージャー。
    public Slider slider;           //カメラの拡大率のスライダー。

    public float moveSpeed = 2.0f;    //カメラのスピード。
    Camera camera;                  //カメラ。
    Vector3 position;               //座標。
    bool isMove = false;            //動いたかどうか。

    // Start is called before the first frame update
    void Start()
    {
        camera = GetComponent<Camera>();
        position = camera.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        CameraMove();
    }

    void CameraMove()
    {
        isMove = false;
        if (touchManager.IsTouch())
        {
            if(touchManager.GetPhase() == TouchPhase.Moved)
            {
                position.x += touchManager.GetDeltaPosInScreen().x * -moveSpeed;
                position.z += touchManager.GetDeltaPosInScreen().y * -moveSpeed;

                camera.transform.position = position;


                isMove = true;
            }
        }

    }
}
