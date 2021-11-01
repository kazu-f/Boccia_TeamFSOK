using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BocciaPlayer
{
    public class ThrowGaugeScript : MonoBehaviour
    {
        TouchManager touchManager;
        private RectTransform m_gaugeTransform;
        private Material m_gaugeImageMat;
        private Vector2 m_gaugeSize;
        private float m_throwPow = 0.0f;


        private static ThrowGaugeScript instance = null;        //インスタンス変数。
        public static ThrowGaugeScript GetInstance()
        {
            if (instance == null)
            {
                Debug.LogError("ThrowGaugeScriptが生成されていない。");
            }
            return instance;
        }
        private void OnDestroy()
        {
            // 破棄時に、登録した実体の解除を行う
            if (this == instance) instance = null;
        }

        private void Awake()
        {
            // もしインスタンスが存在するなら、自らを破棄する
            if (instance != null)
            {
                Destroy(this.gameObject);
                return;
            }

            instance = this;

            touchManager = TouchManager.GetInstance();

            //マテリアルの取得。。
            var image = this.gameObject.GetComponent<Image>();
            m_gaugeImageMat = image.material;

            var canvasRect = touchManager.GetCavasRect();
            m_gaugeTransform = this.gameObject.GetComponent<RectTransform>();

            //ゲージのサイズを計算。
            m_gaugeSize.x = m_gaugeTransform.rect.width * m_gaugeTransform.localScale.x / canvasRect.sizeDelta.x;
            m_gaugeSize.y = m_gaugeTransform.rect.height * m_gaugeTransform.localScale.y / canvasRect.sizeDelta.y;
        }
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            m_gaugeImageMat.SetFloat("_ThrowPow", m_throwPow);
        }

        /// <summary>
        /// ゲージの座標を設定。
        /// </summary>
        /// <param name="pos">
        /// 画面左下を基点にしたスクリーン空間座標。
        /// <remark>X = 0.0 ～ 1.0</remark>
        /// <remark>Y = 0.0 ～ 1.0</remark>
        /// </param>
        public void SetPosition(Vector2 pos)
        {
            var canvasRect = touchManager.GetCavasRect();
            pos.x -= 0.5f;
            pos.y -= 0.5f;
            //画面上の座標に変換。
            m_gaugeTransform.anchoredPosition = pos * canvasRect.sizeDelta;
        }

        private void OnEnable()
        {
            //画面上の座標を設定。
            SetPosition(touchManager.GetTouchPosInScreen());
        }
        /// <summary>
        /// 投げる力をセット。
        /// </summary>
        public void SetThrowPow(float pow)
        {
            m_throwPow = pow;
        }
        /// <summary>
        /// ゲージのサイズを取得。
        /// </summary>
        public Vector2 GetGaugeSize()
        {
            return m_gaugeSize;
        }
    }
}
