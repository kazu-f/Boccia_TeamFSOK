using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleSceneScript : MonoBehaviour
{
    private ChangeSceneScript changeScene;  //シーン切り替え制御。
    public TouchManager touchManager;       //タッチ制御。
    public AudioSource touchSESource;       //タッチ音のソース。
    private AudioSource touchSE;            //タッチ音。
    // Start is called before the first frame update
    void Start()
    {
        changeScene = this.gameObject.GetComponent<ChangeSceneScript>();
        touchSE = Instantiate(touchSESource);
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
