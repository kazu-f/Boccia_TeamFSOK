using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//マッチングを探し始める。
public class FindMatching : MonoBehaviour
{
    private NetworkLauncherScript launcher = null;
    private float startTime = 0.0f;
    [Tooltip("マッチングのために待機する時間のリミット。")]
    [SerializeField] private float LIMIT_WAIT_TIME = 30.0f;
    // Start is called before the first frame update
    void Start()
    {
        launcher = this.gameObject.GetComponent<NetworkLauncherScript>();
        if(launcher != null)
        {
            //接続を開始する。
            launcher.Connect();
        }
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time - startTime > LIMIT_WAIT_TIME)
        {
            //マッチングに時間が掛かりすぎているためAI対戦を行う。
            launcher.UseAI();
            Debug.Log("AI対戦に切り替える。");
        }        
    }
}
