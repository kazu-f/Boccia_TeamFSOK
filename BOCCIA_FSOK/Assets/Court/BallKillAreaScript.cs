using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallKillAreaScript : MonoBehaviour
{
    GameObject m_GameFlow = null;

    private void Awake()
    {
        m_GameFlow = GameObject.Find("GameFlow");
        if(m_GameFlow == null)
        {
            //ゲームフロウオブジェクトが取得できなかった時
            Debug.LogError("GameFlowオブジェクトが取得できませんでした");
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        //other.GetComponent<IBallScript>().OutsideVenue();
    }

}
