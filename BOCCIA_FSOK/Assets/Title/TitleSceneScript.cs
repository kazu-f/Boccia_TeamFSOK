using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleSceneScript : MonoBehaviour
{
    private ChangeSceneScript changeScene;  //�V�[���؂�ւ�����B
    public TouchManager touchManager;       //�^�b�`����B
    // Start is called before the first frame update
    void Start()
    {
        changeScene = this.gameObject.GetComponent<ChangeSceneScript>();
    }

    // Update is called once per frame
    void Update()
    {
     if(touchManager.GetPhase() == TouchPhase.Ended)
        {
            changeScene.ChangeSceneInvoke(true, 2.0f);
        }
    }
}
