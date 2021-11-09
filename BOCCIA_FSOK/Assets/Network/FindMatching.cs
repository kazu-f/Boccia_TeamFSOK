using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//マッチングを探し始める。
public class FindMatching : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var launchar = this.gameObject.GetComponent<NetworkLauncherScript>();
        if(launchar != null)
        {
            //接続を開始する。
            launchar.Connect();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
