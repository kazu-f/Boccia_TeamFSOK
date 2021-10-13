using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BocciaPlayer
{
    public class ThrowBallControler : MonoBehaviour
    {
        TouchManager touchManager;                  //タッチコンソール。
        public BallHolderController ballHolder;     //ボール所有者。
        private BallStateScript currentBallState = null;    //現在投げるボール状態。
        public GameObject bocciaPlayer;             //プレイヤーオブジェクト。
        public GameObject throwDummyObj;            //予測線表示オブジェクト。
        private ThrowDummyScript throwDummy;        //予測線表示スクリプト。
        private AudioSource throwSE;                //投げるときのSE。
        //private。
        private ThrowGaugeScript throwGauge;
        private Vector2 m_touchStartPos = new Vector2(0.0f, 0.0f);     //触り始めた座標。
        private Vector2 m_touchEndPos = new Vector2(0.0f, 0.0f);       //引き切った座標。
        private Vector2 m_endToStart = new Vector2(0.0f, 0.0f);       //開始座標から引き切った座標までのベクトル。
        private Vector2 m_touchPosInScreen = new Vector2(0.0f, 0.0f); //現在のタッチしている座標(スクリーン座標系？)
        private float m_throwPow = 0.0f;                //投げる力

        private Vector3 m_force = new Vector3(0.0f, 0.0f, 0.0f);      //初速。
        private Vector3 m_throwPos = new Vector3(0.0f, 0.0f, 0.0f);   //ボールの始点。7

        private const float THROW_DELAY = 0.2f;           //投げるまでのディレイの時間。
        private bool isDecision = false;           //投げる力を決定したか。

        //定数。
        //const float FLICK_POWER = 0.005f;       //フリック判定用の定数。
        const float MAX_THROW_POW = 200.0f;
        //インスタンス生成時に呼ばれる。
        private void Awake()
        {
            touchManager = TouchManager.GetInstance();

            throwGauge = ThrowGaugeScript.GetInstance();
            throwGauge.gameObject.SetActive(false);

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
            if (touchManager.IsTouch() && !isDecision)
            {
                //前フレームから動きがある。
                if (touchManager.GetPhase() != TouchInfo.Stationary)
                {
                    m_touchPosInScreen = touchManager.GetTouchPosInScreen();
                }

                if (touchManager.GetPhase() == TouchInfo.Moved)
                {
                    //移動した後の座標。
                    m_touchEndPos = m_touchPosInScreen;
                    m_endToStart = m_touchStartPos - m_touchEndPos;
                    m_throwPow = m_endToStart.y / throwGauge.GetGaugeSize().y;
                    m_throwPow = Mathf.Min(1.0f, Mathf.Max(m_throwPow, 0.0f));

                    throwGauge.SetThrowPow(m_throwPow);

                    CalcThrowForce();
                    throwDummy.SetForce(m_force);
                }
                else if (touchManager.GetPhase() == TouchInfo.Ended)
                {
                    if (m_throwPow > 0.0f)
                    {
                        //ボールを投げる。
                        isDecision = true;
                        StartCoroutine("ThrowBall");
                    }
                }
            }
            
            if(isDecision)
            {
                //Debug.Log("ボールが動いているか。");
                if (currentBallState != null && 
                    currentBallState.GetState() == BallState.Stop)
                {
                    currentBallState = null;
                    //ボールを次に進める。
                    var playerCon = bocciaPlayer.GetComponent<PlayerController>();
                    playerCon.isThrowBallNone = ballHolder.UpdateCurrentBallNo();
                    isDecision = false;
                }
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
            m_force.y = 10.0f;
        }

        //ボールを投げる処理。
        IEnumerator ThrowBall()
        {
            yield return new WaitForSeconds(THROW_DELAY);
            //現在投げるボールを取得する。
            var obj = ballHolder.GetCurrentBall();
            if(obj == null)
            {
                yield return null;
            }

            //ボールの位置を合わせる。
            obj.transform.position = m_throwPos;
            obj.transform.rotation = this.transform.rotation;

            currentBallState = obj.GetComponent<BallStateScript>();

            //ボールに投げる力を加える。
            var ballOperate = obj.GetComponent<BallOperateScript>();

            ballOperate.Throw(m_force);

            throwSE.Play();
        }

        public bool IsDecision()
        {
            return isDecision;
        }

        private void OnEnable()
        {
            if (isDecision) return;
            throwGauge.gameObject.SetActive(true);
            throwDummyObj.SetActive(true);
            //タッチ位置を初期化。
            m_touchStartPos = touchManager.GetTouchPosInScreen();
            m_touchEndPos = touchManager.GetTouchPosInScreen();
            //ボールを投げる座標を計算。
            CalcPosition();
            throwDummy.SetPosition(m_throwPos);
            //投げる力初期化。
            m_force = Vector3.zero;
            throwDummy.SetForce(m_force);
            m_throwPow = 0.0f;
            throwGauge.SetThrowPow(0.0f);
        }

        private void OnDisable()
        {
            throwGauge.gameObject.SetActive(false);
            throwDummyObj.SetActive(false);
        }
    }
}
