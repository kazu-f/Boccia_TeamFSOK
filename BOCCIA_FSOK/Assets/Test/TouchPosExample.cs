using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TouchPosExample : MonoBehaviour
{
    public Text m_Text;
    //public Canvas canvas;
    public RectTransform canvasRect;
    public TouchManager touchManager;
    Vector2 posInScreen = new Vector2(0.0f, 0.0f);

    // Start is called before the first frame update
    void Start()
    {

    }
    void Update()
    {
        Vector2 touchPos = touchManager.GetTouchPos();

        //Update the Text on the screen depending on current position of the touch each frame
        m_Text.text = "Touch Position : " + touchPos;

        Vector2 delPos = touchManager.GetDeltaPos();

        m_Text.text += "\nDelta Position : " + delPos;

        posInScreen.x = touchPos.x / Screen.width;
        posInScreen.y = touchPos.y / Screen.height;
        //posInScreen.x = (touchPos.x - Screen.width / 2) / Screen.width * canvasRect.sizeDelta.x;
        //posInScreen.y = (touchPos.y - Screen.height / 2) / Screen.height * canvasRect.sizeDelta.y;
        //RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, touchPos, canvas.worldCamera, out posInScreen);

        m_Text.text += "\nScreen Position : " + posInScreen;

        TouchPhase touchPhase = touchManager.GetPhase();
        if(touchPhase == TouchPhase.Began)
        {
            m_Text.text += "\nTouchPhase : Began";
        }
        else if(touchPhase == TouchPhase.Moved)
        {
            m_Text.text += "\nTouchPhase : Moved";
        }
        else if(touchPhase == TouchPhase.Stationary)
        {
            m_Text.text += "\nTouchPhase : Stationary";
        }
        else if(touchPhase == TouchPhase.Ended)
        {
            m_Text.text += "\nTouchPhase : Ended";
        }

    }
}
