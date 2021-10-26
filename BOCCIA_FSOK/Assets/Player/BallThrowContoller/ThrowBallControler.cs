using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BocciaPlayer
{
    public class ThrowBallControler : MonoBehaviour
    {
        public BallHolderController ballHolder;     //ボール所有者。
        public GameObject armObj;                 //手を表示させるスクリプト。
        public GameObject bocciaPlayer;             //プレイヤーオブジェクト。
        private ArmScript armScript;                 //手を表示させるスクリプト。
        private GameObject throwDummyObj;            //予測線表示オブジェクト。
        private ThrowDummyScript throwDummy;        //予測線表示スクリプト。
        private AudioSource throwSE;                //投げるときのSE。
        private ThrowGaugeScript throwGauge;
        private Vector2 m_throwPow = new Vector2(0.0f,0.0f);                //投げる力
        private Vector3 m_force = new Vector3(0.0f, 0.0f, 0.0f);      //初速。
        private float m_throwPosHeight = 0.0f;                //ボールを投げる高さの割合。
        private Vector3 m_throwPos = new Vector3(0.0f, 0.0f, 0.0f);   //ボールの始点。

        private const float THROW_DELAY = 0.2f;           //投げるまでのディレイの時間。
        private bool isDecision = false;           //投げる力を決定したか。

        //定数。
        const float MAX_THROW_POW = 200.0f;
        const float MAX_ANGLE_POW = 50.0f;
        //インスタンス生成時に呼ばれる。
        private void Awake()
        {
            armScript = armObj.GetComponent<ArmScript>();

            throwGauge = ThrowGaugeScript.GetInstance();
            throwGauge.gameObject.SetActive(false);

            //予測線の処理。
            throwDummy = ThrowDummyScript.Instance();
            throwDummyObj = throwDummy.gameObject;

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
            
        }
        
        /// <summary>
        /// ボールを投げるための処理を開始。
        /// </summary>
        /// <param name="screenPos">ゲージのスクリーン空間上の座標。xy = 0.0～1.0</param>
        public void StartThrowBall(Vector2 screenPos)
        {
            throwGauge.SetPosition(screenPos);
            SetThrowHeight(screenPos.y);
            //ボールを投げる座標を計算。
            CalcPosition();
            throwDummy.SetPosition(m_throwPos);
            //腕の構え方を切り替える。
            if(m_throwPosHeight > 0.6f)
            {
                armScript.HoldUp();
            }
            else
            {
                armScript.HoldUp();
            }
        }
        /// <summary>
        /// 投げる力をセットする。
        /// </summary>
        /// <param name="throwPow"></param>
        public void SetThrowPow(Vector2 throwPow)
        {
            m_throwPow = throwPow;
            throwGauge.SetThrowPow(m_throwPow.y);
            CalcThrowForce();
            throwDummy.SetForce(m_force);
        }
        /// <summary>
        /// 高さを設定。
        /// </summary>
        /// <param name="height">0.0～1.0の範囲で与える。</param>
        void SetThrowHeight(float height)
        {
            m_throwPosHeight = height;
        }
        /// <summary>
        /// 始点を計算。
        /// </summary>
        void CalcPosition()
        {
            m_throwPos = this.transform.position;
            m_throwPos.y *= m_throwPosHeight;
        }
        /// <summary>
        /// 初速を計算。
        /// </summary>
        void CalcThrowForce()
        {
            Vector3 forward = bocciaPlayer.transform.forward;       //プレイヤーの前方向を取る。
            forward.y = 0.0f;
            forward.Normalize();
            m_force = forward * MAX_THROW_POW* m_throwPow.y;
            Vector3 right;
            right = bocciaPlayer.transform.right;
            right.y = 0.0f;
            right.Normalize();
            m_force += right * m_throwPow.x * m_throwPow.y * MAX_ANGLE_POW;
            m_force.y = 10.0f;
        }

        //ボールを投げる。
        public void ThrowBall()
        {
            StartCoroutine(ThrowCoroutine());
        }

        //ボールを投げる処理。
        IEnumerator ThrowCoroutine()
        {
            isDecision = true;
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

            //ボールに投げる力を加える。
            var ballOperate = obj.GetComponent<BallOperateScript>();

            ballOperate.Throw(m_force);

            throwSE.Play();

            throwGauge.gameObject.SetActive(false);
            throwDummyObj.SetActive(false);

            //ボールを次に進める。
            ballHolder.UpdateCurrentBallNo();
            isDecision = false;
        }

        public bool IsDecision()
        {
            return isDecision;
        }

        private void OnEnable()
        {
            throwGauge.gameObject.SetActive(true);
            throwDummyObj.SetActive(true);
            //投げる力初期化。
            m_force = Vector3.zero;
            throwDummy.SetForce(m_force);
            m_throwPow.x = 0.0f;
            m_throwPow.y = 0.0f;
            throwGauge.SetThrowPow(0.0f);
        }

        private void OnDisable()
        {
            throwGauge.gameObject.SetActive(false);
            throwDummyObj.SetActive(false);
            armScript.HoldDown();
        }
    }
}
