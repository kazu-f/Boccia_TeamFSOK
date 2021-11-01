using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwitchSlideButton : MonoBehaviour
{
    [SerializeField]private PlayerMovePanelSlide panelSlide;
    [SerializeField] private Sprite spriteLeftArrow;
    [SerializeField] private Sprite spriteRightArrow;
    private Image image;
    private bool isSlideIn = true;     //次がスライドインかどうか。
    private bool isChange = false;      //変更があったか。

    private void Awake()
    {
        image = this.gameObject.GetComponent<Image>();
    }

    private void Update()
    {
        //変化が無ければ処理しない。
        if (!isChange) return;
        
        //スライドが止まったらスプライトを切り替える。
        if(!panelSlide.IsMoving())
        {
            //フラグ切り替え。
            isChange = false;
            if(isSlideIn)
            {
                image.sprite = spriteRightArrow;
            }
            else
            {
                image.sprite = spriteLeftArrow;
            }
        }

    }

    public void PanelSlide()
    {
        //スライド中は反応しない。
        if(panelSlide.IsMoving())
        {
            return;
        }
        isChange = true;
        if (isSlideIn)
        {
            panelSlide.SlideIn();
            isSlideIn = false;
        }
        else{
            panelSlide.SlideOut();
            isSlideIn = true;
        }
    }

}
