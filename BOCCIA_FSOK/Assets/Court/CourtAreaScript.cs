using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CourtAreaScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    /// <summary>
    /// 範囲内に入った時の処理
    /// </summary>
    /// <param name="other">オブジェクト</param>
    public void OnTriggerEnter(Collider other)
    {
        other.gameObject.GetComponent<IBallScript>().InsideVenue();
    }


    /// <summary>
    /// 範囲外に出た時の処理
    /// </summary>
    /// <param name="other">オブジェクト</param>
    public void OnTriggerExit(Collider other)
    {
        other.gameObject.GetComponent<IBallScript>().OutsideVenue();
    }
}
