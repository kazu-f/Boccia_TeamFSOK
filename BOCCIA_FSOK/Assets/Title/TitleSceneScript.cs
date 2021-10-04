using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleSceneScript : MonoBehaviour
{
    private ChangeSceneScript changeScene;  //シーン切り替え制御。
    public TouchManager touchManager;       //タッチ制御。
    // Start is called before the first frame update
    void Start()
    {
        changeScene = this.gameObject.GetComponent<ChangeSceneScript>();
    }

    // Update is called once per frame
    void Update()
    {
     if(touchManager.IsTouch())
        {
            changeScene.ChangeScene(true, 2.0f);
        }
    }
}
