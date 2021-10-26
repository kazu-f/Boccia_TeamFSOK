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
    private bool isSlideIn = true;     //�����X���C�h�C�����ǂ����B
    private bool isChange = false;      //�ύX�����������B

    private void Awake()
    {
        image = this.gameObject.GetComponent<Image>();
    }

    private void Update()
    {
        //�ω���������Ώ������Ȃ��B
        if (!isChange) return;
        
        //�X���C�h���~�܂�����X�v���C�g��؂�ւ���B
        if(!panelSlide.IsMoving())
        {
            //�t���O�؂�ւ��B
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
        //�X���C�h���͔������Ȃ��B
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
