using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleSceneScript : MonoBehaviour
{
    private ChangeSceneScript changeScene;  //�V�[���؂�ւ�����B
    private TouchManager touchManager;       //�^�b�`����B
    private AudioSource touchSE;            //�^�b�`���B
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
