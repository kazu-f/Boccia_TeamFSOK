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
    /// �͈͓��ɓ��������̏���
    /// </summary>
    /// <param name="other">�I�u�W�F�N�g</param>
    public void OnTriggerEnter(Collider other)
    {
        other.gameObject.GetComponent<IBallScript>().InsideVenue();
    }


    /// <summary>
    /// �͈͊O�ɏo�����̏���
    /// </summary>
    /// <param name="other">�I�u�W�F�N�g</param>
    public void OnTriggerExit(Collider other)
    {
        other.gameObject.GetComponent<IBallScript>().OutsideVenue();
    }
}
