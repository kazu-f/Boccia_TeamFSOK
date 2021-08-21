using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowAngleController : MonoBehaviour
{
    public TouchManager touchManager;
    public Camera camera;
    public GameObject bocciaPlayer;
    public float angleSpeed = 20.0f;
    private Vector3 newCamAngle = new Vector3(0, 0, 0);
    private Vector3 newPlayerAngle = new Vector3(0, 0, 0);
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(touchManager.IsTouch())
        {
            if(touchManager.GetPhase() == TouchPhase.Began)
            {
                newCamAngle = camera.transform.localEulerAngles;
                newPlayerAngle = bocciaPlayer.transform.localEulerAngles;
            }
            else if(touchManager.GetPhase() == TouchPhase.Moved)
            {
                var rotVec = touchManager.GetDeltaPosInScreen();
                newCamAngle.y += angleSpeed * rotVec.x;
                newPlayerAngle.y += angleSpeed * rotVec.x;

                camera.transform.localEulerAngles = newCamAngle;
                bocciaPlayer.transform.localEulerAngles = newPlayerAngle;
            }
        }
    }
}
