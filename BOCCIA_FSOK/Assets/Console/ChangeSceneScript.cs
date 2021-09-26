using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneScript : MonoBehaviour
{
    public string GameSceneString;
    private TouchManager touchManager = null;
    private bool isSceneChange = false;
    // Start is called before the first frame update
    void Start()
    {
        touchManager = TouchManager.GetInstance();
    }

    // Update is called once per frame
    void Update()
    {
        if(touchManager.IsTouch() && !isSceneChange)
        {
            Invoke("ChangeScene", 2.0f);
            isSceneChange = true;
        }
    }

    void ChangeScene()
    {
        SceneManager.LoadSceneAsync(GameSceneString);
    }
}
