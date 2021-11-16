using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallHitScript : MonoBehaviour
{
    public AudioSource hitSound;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }
    /// <summary>
    /// É{Å[ÉãÇ∆è’ìÀÇµÇΩÅB
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ball" ||
            collision.gameObject.tag == "Jack")
        {
            hitSound.Play();
        }
    }
}
