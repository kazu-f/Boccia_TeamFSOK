using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleSceneScript : MonoBehaviour
{
    private ChangeSceneScript changeScene;  //シーン切り替え制御。
    private TouchManager touchManager;       //タッチ制御。
    private AudioSource touchSE;            //タッチ音。
    // Start is called before the first frame update
    void Start()
    {
        touchManager = TouchManager.GetInstance();
        changeScene = this.gameObject.GetComponent<ChangeSceneScript>();
        touchSE = this.gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (touchManager.GetPhase() == TouchInfo.Ended)
        {
            changeScene.ChangeSceneInvoke(true, 2.0f);
            if(touchSE != null)
            {
                touchSE.Play();
            }
        }
    }
}
