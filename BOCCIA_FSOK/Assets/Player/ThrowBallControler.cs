using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BocciaPlayer
{
    public class ThrowBallControler : MonoBehaviour
    {
        public TouchManager touchManager;
        public GameObject bocciaPlayer;
        public GameObject ball;
        public GameObject throwGauge;
        public RectTransform canvasRect;
        RectTransform gaugeTransform;
        Material gaugeImageMat;
        Vector2 gaugeSize;
        Vector2 touchStartPos = new Vector2(0.0f, 0.0f);     //触り始めた座標。
        Vector2 touchEndPos = new Vector2(0.0f, 0.0f);       //引き切った座標。
        Vector2 endToStart = new Vector2(0.0f, 0.0f);       //開始座標から引き切った座標までのベクトル。
        Vector2 touchPosInScreen = new Vector2(0.0f, 0.0f); //現在のタッチしている座標(スクリーン座標系？)
        float throwPow = 0.0f;
        const float FLICK_POWER = 0.005f;     //フリック判定用の定数。
        const float MAX_THROW_POW = 300.0f;

        // Start is called before the first frame update
        void Start()
        {
            //マテリアルの取得。。
            var image = throwGauge.GetComponent<Image>();
            gaugeImageMat = image.material;

            gaugeTransform = throwGauge.GetComponent<RectTransform>();
            gaugeSize.x = gaugeTransform.rect.width * gaugeTransform.localScale.x / canvasRect.sizeDelta.x;
            gaugeSize.y = gaugeTransform.rect.height * gaugeTransform.localScale.y / canvasRect.sizeDelta.y;

            throwGauge.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {
            if (touchManager.IsTouch())
            {
                throwGauge.SetActive(true);
                if (touchManager.GetPhase() != TouchPhase.Stationary)
                {
                    touchPosInScreen = touchManager.GetTouchPosInScreen();
                }

                if (touchManager.GetPhase() == TouchPhase.Moved)
                {
                    var deltaMoveVec = touchManager.GetDeltaPosInScreen();
                    //上方向にフリックしていなければ。
                    if (deltaMoveVec.y < FLICK_POWER
                        && touchPosInScreen.y < touchStartPos.y)
                    {
                        //移動した後の座標。
                        touchEndPos = touchPosInScreen;
                        endToStart = touchStartPos - touchEndPos;
                        throwPow = endToStart.y / gaugeSize.y;
                        throwPow = Mathf.Min(1.0f, Mathf.Max(throwPow, 0.0f));
                    }
                }
                else if (touchManager.GetPhase() == TouchPhase.Ended)
                {
                    if (touchStartPos.y < touchPosInScreen.y
                        && endToStart.y > 0.01f)
                    {
                        var ballPos = this.transform.position;
                        ballPos.y *= touchStartPos.y;
                        var obj = Instantiate(ball, ballPos, this.transform.rotation);
                        Rigidbody ballRB = obj.GetComponent<Rigidbody>();

                        Vector3 vec = bocciaPlayer.transform.forward;       //プレイヤーの前方向を取る。
                        vec.x *= MAX_THROW_POW * throwPow;
                        vec.z *= MAX_THROW_POW * throwPow;
                        vec.y = 10.0f;

                        ballRB.AddForce(vec);
                    }
                }
                gaugeImageMat.SetFloat("_ThrowPow", throwPow);
            }
            else
            {
                throwGauge.SetActive(false);
            }
        }

        public void ThrowBallEnable()
        {
            this.enabled = true;
            touchStartPos = touchManager.GetTouchPosInScreen();
            touchEndPos = touchManager.GetTouchPosInScreen();
            gaugeTransform.anchoredPosition = touchManager.GetTouchPos();
            throwPow = 0.0f;
        }

        public void ThrowBallDisable()
        {
            this.enabled = false;
        }
    }
}
