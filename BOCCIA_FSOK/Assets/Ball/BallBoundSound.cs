using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallBoundSound : MonoBehaviour
{
    public AudioSource boundSE;
    private IBallScript ballScript;
    // Start is called before the first frame update
    void Start()
    {
        ballScript = this.gameObject.GetComponent<IBallScript>();
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
        if (!ballScript.GetIsThrow()) return;
        if(collision.gameObject.tag == "Court")
        {
            boundSE.Play();
        }
    }
}
