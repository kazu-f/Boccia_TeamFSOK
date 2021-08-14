using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    TouchManager TouchManager;
    public GameObject ball;

    // Start is called before the first frame update
    void Start()
    {
        TouchManager = new TouchManager();
    }

    // Update is called once per frame
    void Update()
    {
        TouchManager.Update();

        if(TouchManager.IsTouch())
        {
            if(TouchManager.GetPhase() == TouchPhase.Began)
            {
                var obj = Instantiate(ball,this.transform.position,this.transform.rotation);
                Rigidbody ballRB = obj.GetComponent<Rigidbody>();
                Vector3 vec = new Vector3(0.0f, 10.0f, 100.0f);
                ballRB.AddForce(vec);
            }
        }

    }
}
