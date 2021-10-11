using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BocciaPlayer
{
    public class ThrowBallControler : MonoBehaviour
    {
        TouchManager touchManager;
        public BallHolderController ballHolder;
        public GameObject bocciaPlayer;
        public GameObject throwDummyObj;
        private ThrowDummyScript throwDummy;
        public GameObject throwGaugePrefab;
        private AudioSource throwSE;
        //private。
        private GameObject throwGauge;
        private RectTransform m_gaugeTransform;
        private Material m_gaugeImageMat;
        private Vector2 m_gaugeSize;
        private Vector2 m_touchStartPos = new Vector2(0.0f, 0.0f);     //触り始めた座標。
        private Vector2 m_touchEndPos = new Vector2(0.0f, 0.0f);       //引き切った座標。
        private Vector2 m_endToStart = new Vector2(0.0f, 0.0f);       //開始座標から引き切った座標までのベクトル。
        private Vector2 m_touchPosInScreen = new Vector2(0.0f, 0.0f); //現在のタッチしている座標(スクリーン座標系？)
        private float m_throwPow = 0.0f;                //投げる力

        private Vector3 m_force = new Vector3(0.0f, 0.0f, 0.0f);      //初速。
        private Vector3 m_throwPos = new Vector3(0.0f, 0.0f, 0.0f);   //ボールの始点。
        //定数。
        const float FLICK_POWER = 0.005f;       //フリック判定用の定数。
        const float MAX_THROW_POW = 200.0f;
        //インスタンス生成時に呼ばれる。
        private void Awake()
        {
            touchManager = TouchManager.GetInstance();
            //ゲージの親をキャンバスにする。
            var canvas = touchManager.GetCanvas();
            //投げゲージ作成。
            throwGauge = Instantiate(throwGaugePrefab, canvas.GetComponent<RectTransform>(),false);

            //マテリアルの取得。。
            var image = throwGauge.GetComponent<Image>();
            m_gaugeImageMat = image.material;

            var canvasRect = touchManager.GetCavasRect();
            m_gaugeTransform = throwGauge.GetComponent<RectTransform>();
            m_gaugeSize.x = m_gaugeTransform.rect.width * m_gaugeTransform.localScale.x / canvasRect.sizeDelta.x;
            m_gaugeSize.y = m_gaugeTransform.rect.height * m_gaugeTransform.localScale.y / canvasRect.sizeDelta.y;

            throwGauge.SetActive(false);

            //予測線の処理。
            throwDummy = throwDummyObj.GetComponent<ThrowDummyScript>();

            //SEを取得。
            throwSE = GetComponent<AudioSource>();

            throwDummyObj.SetActive(false);
        }
        // Start is called before the first frame update
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
            if (touchManager.IsTouch())
            {
                //前フレームから動きがある。
                if (touchManager.GetPhase() != TouchPhase.Stationary)
                {
                    m_touchPosInScreen = touchManager.GetTouchPosInScreen();
                }

                if (touchManager.GetPhase() == TouchPhase.Moved)
                {
                    var deltaMoveVec = touchManager.GetDeltaPosInScreen();
                    //上方向にフリックしていなければ、投げる力を弱めようとしていると判断する。
                    if (deltaMoveVec.y < FLICK_POWER
                        && m_touchPosInScreen.y < m_touchStartPos.y)
                    {
                        //移動した後の座標。
                        m_touchEndPos = m_touchPosInScreen;
                        m_endToStart = m_touchStartPos - m_touchEndPos;
                        m_throwPow = m_endToStart.y / m_gaugeSize.y;
                        m_throwPow = Mathf.Min(1.0f, Mathf.Max(m_throwPow, 0.0f));

                        CalcThrowForce();
                        throwDummy.SetForce(m_force);
                    }
                }
                else if (touchManager.GetPhase() == TouchPhase.Ended)
                {
                    //指を離すタイミングで投げるかどうか決定。
                    if (m_touchStartPos.y < m_touchPosInScreen.y
                        && m_endToStart.y > 0.01f)
                    {
                        //ボールを投げる。
                        ThrowBall();
                    }
                    throwGauge.SetActive(false);
                    throwDummyObj.SetActive(false);
                }
                m_gaugeImageMat.SetFloat("_ThrowPow", m_throwPow);
            }
        }
        /// <summary>
        /// 始点を計算。
        /// </summary>
        void CalcPosition()
        {
            m_throwPos = this.transform.position;
            m_throwPos.y *= m_touchStartPos.y;
        }
        /// <summary>
        /// 初速を計算。
        /// </summary>
        void CalcThrowForce()
        {
            m_force = bocciaPlayer.transform.forward;       //プレイヤーの前方向を取る。
            m_force.x *= MAX_THROW_POW * m_throwPow;
            m_force.z *= MAX_THROW_POW * m_throwPow;
            //m_force.y = 10.0f;
        }

        //ボールを投げる処理。
        void ThrowBall()
        {
            //現在投げるボールを取得する。
            var obj = ballHolder.GetCurrentBall();
            if(obj == null)
            {
                return;
            }

            //ボールの位置を合わせる。
            obj.transform.position = m_throwPos;
            obj.transform.rotation = this.transform.rotation;

            //ボールに投げる力を加える。
            var ballOperate = obj.GetComponent<BallOperateScript>();

            ballOperate.Throw(m_force);

            throwSE.Play();

            //ボールを次に進める。
            var playerCon = bocciaPlayer.GetComponent<PlayerController>();
            playerCon.isThrowBallNone = ballHolder.UpdateCurrentBallNo();
        }

        //ボールを投げる制御を開始。
        public void ThrowBallEnable()
        {
            this.enabled = true;
            throwGauge.SetActive(true);
            throwDummyObj.SetActive(true);
            m_touchStartPos = touchManager.GetTouchPosInScreen();
            m_touchEndPos = touchManager.GetTouchPosInScreen();
            CalcPosition();
            throwDummy.SetPosition(m_throwPos);
            m_gaugeTransform.anchoredPosition = touchManager.GetTouchPos();
            m_throwPow = 0.0f;
        }

        public void ThrowBallDisable()
        {
            this.enabled = false;
            throwGauge.SetActive(false);
        }
    }
}
