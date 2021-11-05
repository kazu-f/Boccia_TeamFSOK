using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraChangeSprite : MonoBehaviour
{
    [SerializeField]private Sprite[] ButtonSprite = new Sprite[2];
    private Image buttonImage;
    int currentSprite = 0;

    // Start is called before the first frame update
    void Start()
    {
        buttonImage = this.gameObject.GetComponent<Image>();
        buttonImage.sprite = ButtonSprite[currentSprite];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeSprite()
    {
        currentSprite ^= 1;

        buttonImage.sprite = ButtonSprite[currentSprite];
    }

}
