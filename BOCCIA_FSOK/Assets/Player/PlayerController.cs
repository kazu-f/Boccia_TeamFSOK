using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BocciaPlayer
{
    public class PlayerController : MonoBehaviour
    {
        public TouchManager touchManager;
        public ThrowBallControler throwBallControler;
        public ThrowAngleController throwAngleController;
        Vector2 touchStartPos = new Vector2(0.0f,0.0f);     //触り始めた座標。

        // Start is called before the first frame update
        void Start()
        {
            throwBallControler.enabled = false;
            throwAngleController.enabled = false;
        }

        // Update is called once per frame
        void Update()
        {
            if(touchManager.IsTouch())
            {
                if(touchManager.GetPhase() == TouchPhase.Began)
                {
                    touchStartPos = touchManager.GetTouchPosInScreen();
                }
                //有効化フラグを切り替える。
                throwBallControler.enabled = touchStartPos.y > 0.2f;
                throwAngleController.enabled = touchStartPos.y <= 0.2f;

            }
            else
            {
                throwBallControler.enabled = false;
                throwAngleController.enabled = false;
            }
        }
    }
}
