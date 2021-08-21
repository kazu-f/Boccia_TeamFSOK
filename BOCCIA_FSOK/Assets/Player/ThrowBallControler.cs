using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BocciaPlayer
{
    public class ThrowBallControler : MonoBehaviour
    {
        public TouchManager touchManager;
        public GameObject bocciaPlayer;
        public GameObject ball;
        Vector2 touchStartPos = new Vector2(0.0f, 0.0f);     //触り始めた座標。
        Vector2 touchEndPos = new Vector2(0.0f, 0.0f);       //引き切った座標。
        Vector2 endToStart = new Vector2(0.0f, 0.0f);       //開始座標から引き切った座標までのベクトル。
        Vector2 touchPosInScreen = new Vector2(0.0f, 0.0f); //現在のタッチしている座標(スクリーン座標系？)

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (touchManager.IsTouch())
            {
                if (touchManager.GetPhase() != TouchPhase.Stationary)
                {
                    touchPosInScreen = touchManager.GetTouchPosInScreen();
                }

                if (touchManager.GetPhase() == TouchPhase.Began)
                {
                    touchStartPos = touchPosInScreen;

                }
                else if (touchManager.GetPhase() == TouchPhase.Moved)
                {
                    var deltaMoveVec = touchManager.GetDeltaPosInScreen();
                    //上方向にフリックしていなければ。
                    if (deltaMoveVec.y < 0.05f
                        && touchPosInScreen.y < touchStartPos.y)
                    {
                        //移動した後の座標。
                        touchEndPos = touchPosInScreen;
                    }
                }
                else if (touchManager.GetPhase() == TouchPhase.Ended)
                {
                    if (touchStartPos.y < touchPosInScreen.y)
                    {
                        endToStart = touchStartPos - touchEndPos;
                        var ballPos = this.transform.position;
                        ballPos.y = 2.0f * touchStartPos.y;
                        var obj = Instantiate(ball, ballPos, this.transform.rotation);
                        Rigidbody ballRB = obj.GetComponent<Rigidbody>();

                        Vector3 vec = bocciaPlayer.transform.forward;       //プレイヤーの前方向を取る。
                        vec.x *= 500.0f * Mathf.Min(1.0f,Mathf.Max(endToStart.y, 0.1f));
                        vec.z *= 500.0f * Mathf.Min(1.0f,Mathf.Max(endToStart.y, 0.1f));
                        vec.y = 10.0f;

                        ballRB.AddForce(vec);
                    }
                }
            }
        }
    }
}
