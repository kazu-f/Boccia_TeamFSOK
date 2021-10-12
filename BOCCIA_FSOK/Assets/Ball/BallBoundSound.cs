using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallBoundSound : MonoBehaviour
{
    private AudioSource boundSE;
    new private Rigidbody rigidbody;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        boundSE = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    /// <summary>
    /// ï®ëÃÇ∆è’ìÀÇµÇΩÅB
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Court")
        {
            boundSE.Play();
        }
    }
}
