using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Delete : MonoBehaviour
{
    public Sprite[] HideSprite;       //��\���ɂ���X�v���C�g
    private Image image;
    private int currentSpriteNo = 0;
    private int maxSpriteNo = 0;
    private const int minSpriteNo = 0;
    private ChangeSceneScript changeScene;  //�V�[���؂�ւ�����B

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
            Debug.Log("�����܂���");
        }

        if (Input.GetMouseButtonDown(1))
        {
            currentSpriteNo--;
            currentSpriteNo=Mathf.Max(currentSpriteNo,minSpriteNo);
            Debug.Log("�\�����܂���");
        }
        image.sprite = HideSprite[Mathf.Min(currentSpriteNo, maxSpriteNo - 1)];
    }
}
