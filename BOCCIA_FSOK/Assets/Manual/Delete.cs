using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Delete : MonoBehaviour
{
    public Sprite[] HideSprite;       //非表示にするスプライト
    private Image image;
    private int currentSpriteNo = 0;
    private int maxSpriteNo = 0;
    private const int minSpriteNo = 0;
    private ChangeSceneScript changeScene;  //シーン切り替え制御。

    // Start is called before the first frame update
    void Start()
    {
        image = this.gameObject.GetComponent<Image>();
        maxSpriteNo = HideSprite.Length;
        changeScene = this.gameObject.GetComponent<ChangeSceneScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            currentSpriteNo++;
            if (currentSpriteNo>=maxSpriteNo)
            {
                changeScene.ChangeSceneInvoke(true);
            }
            Debug.Log("消えました");
        }

        if (Input.GetMouseButtonDown(1))
        {
            currentSpriteNo--;
            currentSpriteNo=Mathf.Max(currentSpriteNo,minSpriteNo);
            Debug.Log("表示しました");
        }
        image.sprite = HideSprite[Mathf.Min(currentSpriteNo, maxSpriteNo - 1)];
    }
}
