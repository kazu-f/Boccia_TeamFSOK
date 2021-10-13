using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwichActiveGameObjects : MonoBehaviour
{
    public GameObject[] gameObjects;    //ボールが動いている間止めるオブジェクトたち。

    private static SwichActiveGameObjects instance = null;        //インスタンス変数。
    public static SwichActiveGameObjects GetInstance()
    {
        if (instance == null)
        {
            Debug.LogError("SwichActiveGameObjectsが生成されていない。");
        }
        return instance;
    }

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

    private void OnDestroy()
    {
        // 破棄時に、登録した実体の解除を行う
        if (this == instance) instance = null;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    /// <summary>
    /// ゲームオブジェクトのアクティブを変更。
    /// </summary>
    public void SwitchGameObject(bool isActive)
    {
        if (!(gameObjects.Length > 0))
        {
            Debug.Log("ゲームオブジェクトが登録されていない。");
            return;
        }

        //if(isActive)
        //{
        //    Debug.Log("アクティブにする。");
        //}
        //else
        //{
        //    Debug.Log("アクティブ解除。");
        //}


        foreach (var obj in gameObjects)
        {
            obj.gameObject.SetActive(isActive);
        }
    }
}
