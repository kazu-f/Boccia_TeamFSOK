using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BocciaPlayer
{
    public class ThrowBallControler : Photon.Pun.MonoBehaviourPun
    {
        public BallHolderController ballHolder;     //ボール所有者。
        public GameObject armObj;                 //手を表示させるスクリプト。
        public GameObject bocciaPlayer;             //プレイヤーオブジェクト。
        [SerializeField]private TimerFillScript timerFill;          //タイマー。
        private ArmScript armScript;                 //手を表示させるスクリプト。
        private GameObject throwDummyObj;            //予測線表示オブジェクト。
        private ThrowDummyScript throwDummy;        //予測線表示スクリプト。
        private AudioSource throwSE;                //投げるときのSE。
        private ThrowGaugeScript throwGauge;
        private ServerTimerScript serverTimer;      //カウントを行うためのスクリプト。
        private Vector2 m_throwPow = new Vector2(0.0f,0.0f);                //投げる力
        private Vector3 m_force = new Vector3(0.0f, 0.0f, 0.0f);      //初速。
        private float m_throwPosHeight = 0.0f;                //ボールを投げる高さの割合。
        [SerializeField] private float m_throwPosHeightMax = 1.7f;               //投げる地点の高さの最大値。
        [SerializeField] private float m_throwPosHeightMin = 0.3f;               //投げる地点の高さの最小値。
        private Vector3 m_throwPos = new Vector3(0.0f, 0.0f, 0.0f);   //ボールの始点。

        private const float THROW_DELAY = 0.2f;           //投げるまでのディレイの時間。
        private bool isDecision = false;           //投げる力を決定したか。
        public bool isMyTeam = false;               //自分のチームかどうか。

        //定数。
        const float SWITCH_ARM_BORDER = 0.6f;
        const float MAX_THROW_POW = 200.0f;
        const float MAX_ANGLE_POW = 50.0f;
        //インスタンス生成時に呼ばれる。
        private void Awake()
        {
            //腕のスクリプト。
            armScript = armObj.GetComponent<ArmScript>();

            //ゲージ表示のスクリプトを取得。
            throwGauge = ThrowGaugeScript.GetInstance();
            throwGauge.gameObject.SetActive(false);

            //予測線の処理を取得。
            throwDummy = ThrowDummyScript.Instance();
            throwDummyObj = throwDummy.gameObject;
            throwDummyObj.SetActive(false);

            //SEを取得。
            throwSE = GetComponent<AudioSource>();

            //カウントを行うスクリプト取得。
            serverTimer = bocciaPlayer.GetComponent<ServerTimerScript>();

        }
        // Start is called before the first frame update
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
            //投げるときには更新しない。
            if (isDecision) return;
            //ボールを投げる座標を計算。
            CalcPosition();
            throwDummy.SetPosition(m_throwPos);
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
            if(m_throwPosHeight > SWITCH_ARM_BORDER)
            {
                armScript.HoldUp();
            }
            else
            {
                armScript.HoldDown();
            }
        }
        /// <summary>
        /// 投げる力をセットする。
        /// </summary>
        /// <param name="throwPow">x = -1.0～1.0 , y = 0.0～1.0</param>
        public void SetThrowPow(Vector2 throwPow)
        {
            m_throwPow = throwPow;
            throwGauge.SetThrowPow(m_throwPow.y);   //ゲージの位置を設定。
            CalcThrowForce();
            throwDummy.SetForce(m_force);           //投げる力を設定。
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
            m_throwPos = armScript.GetPosition();
            m_throwPos.y = Mathf.Lerp(m_throwPosHeightMin, m_throwPosHeightMax, m_throwPosHeight + armScript.GetFluctuation());
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
        /// <summary>
        /// 投げる位置を取得。
        /// </summary>
        public Vector3 GetThrowPosition()
        {
            return m_throwPos;
        }
        /// <summary>
        /// 投げる位置を直接設定。
        /// </summary>
        public void SetThrowPosition(Vector3 pos)
        {
            m_throwPos = pos;
        }

        //ボールを投げる。
        public void ThrowBall()
        {
            object[] param = {
                m_force,
                m_throwPos,
                this.transform.rotation
            };

            //カウントする。
            serverTimer.SetCountTimeSecond(THROW_DELAY);
            this.photonView.RPC(nameof(ThrowBallRPC), Photon.Pun.RpcTarget.All, param);
        }

        //ボールを投げるコルーチン開始。
        [Photon.Pun.PunRPC]
        void ThrowBallRPC(Vector3 force, Vector3 throwPos, Quaternion throwRot)
        {
            //コルーチンを開始。
            StartCoroutine(ThrowCoroutine(force, throwPos, throwRot));
        }

        //ボールを投げる処理。
        IEnumerator ThrowCoroutine(Vector3 force, Vector3 throwPos, Quaternion throwRot)
        {
            isDecision = true;
            //yield return new WaitForSeconds(THROW_DELAY);
            //カウントが終わっているか？
            while(!serverTimer.IsCountEnd())
            {
                yield return 0;
            }

            //タイムアップしているなら中断。
            if(timerFill.IsTimeUp())
            {
                //コルーチンの中断。
                yield break;
            }

            //現在投げるボールを取得する。
            var obj = ballHolder.GetCurrentBall();
            if (obj == null)
            {
                //コルーチンの中断。
                yield break;
            }

            //ボールの位置を合わせる。
            obj.transform.position = throwPos;
            obj.transform.rotation = throwRot;

            //ボールに投げる力を加える。
            var ballOperate = obj.GetComponent<BallOperateScript>();

            ballOperate.Throw(force); //ボールを投げる。

            throwSE.Play();     //投げるときのSE。

            //もろもろの処理を停止。
            throwGauge.gameObject.SetActive(false);
            throwDummyObj.SetActive(false);

            isDecision = false;
        }

        public bool IsDecision()
        {
            return isDecision;
        }

        public ArmScript GetArmScript()
        {
            return armScript;
        }

        private void OnEnable()
        {
            //有効にする。
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
            //色々な処理を無効にする。
            throwGauge.gameObject.SetActive(false);
            throwDummyObj.SetActive(false);
            armScript.HoldDown();
        }
    }
}
