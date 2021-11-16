using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPhysicsThrow : MonoBehaviour
{
    public GameObject prefab;
    private GameObject ball;
    public float Power = 100.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButton(0))
        {
            if(ball != null)
            {
                Destroy(ball);
            }
            ball = Instantiate(prefab, transform.position, transform.rotation);
            var RB = ball.GetComponent<Rigidbody>();
            Vector3 force = Vector3.zero;
            force = transform.forward;
            force.y = 0.2f;
            force *= Power;
            RB.AddForce(force);
        }
        
    }
}
