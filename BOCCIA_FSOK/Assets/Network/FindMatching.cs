using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�}�b�`���O��T���n�߂�B
public class FindMatching : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var launchar = this.gameObject.GetComponent<NetworkLauncherScript>();
        if(launchar != null)
        {
            //�ڑ����J�n����B
            launchar.Connect();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
