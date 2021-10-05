using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneScript : MonoBehaviour
{
    public string GameSceneString;
    private bool isSceneChange = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void LoadSceneAsync()
    {
        SceneManager.LoadSceneAsync(GameSceneString);
    }

    void LoadScene()
    {
        SceneManager.LoadScene(GameSceneString);
    }

    //シーンを変更。
    public void ChangeSceneInvoke(bool isAsync,float invokeTime = 0.0f)
    {
        if(isSceneChange)
        {
            return;
        }
        if(isAsync)
        {
            Invoke("LoadSceneAsync", invokeTime);
        }
        else
        {
            Invoke("LoadScene", invokeTime);
        }
        isSceneChange = true;
    }
    /// <summary>
    /// シーン変更。
    /// </summary>
    public void ChangeScene(bool isAsync)
    {
        if(isSceneChange)
        {
            return;
        }
        if(isAsync)
        {
            LoadSceneAsync();
        }
        else
        {
            LoadScene();
        }
        isSceneChange = true;
    }
}
