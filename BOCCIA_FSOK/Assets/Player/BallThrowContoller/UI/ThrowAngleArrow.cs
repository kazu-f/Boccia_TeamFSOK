using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowAngleArrow : MonoBehaviour
{
    private static ThrowAngleArrow instance = null;        //インスタンス変数。
    public static ThrowAngleArrow GetInstance()
    {
        if (instance == null)
        {
            Debug.LogError("ThrowGaugeScriptが生成されていない。");
        }
        return instance;
    }
    //シングルトン。
    private void Awake()
    {
        // もしインスタンスが存在するなら、自らを破棄する
        if (instance != null)
        {
            Destroy(this.gameObject);
            return;
        }

        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
