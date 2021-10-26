using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchSlideButton : MonoBehaviour
{
    [SerializeField]private PlayerMovePanelSlide panelSlide;
    private bool isSlideIn = false;

    public void PanelSlide()
    {
        //スライド中は反応しない。
        if(panelSlide.IsMoving())
        {
            return;
        }

        if(isSlideIn)
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
