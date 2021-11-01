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


        private static ThrowGaugeScript instance = null;        //�C���X�^���X�ϐ��B
        public static ThrowGaugeScript GetInstance()
        {
            if (instance == null)
            {
                Debug.LogError("ThrowGaugeScript����������Ă��Ȃ��B");
            }
            return instance;
        }
        private void OnDestroy()
        {
            // �j�����ɁA�o�^�������̂̉������s��
            if (this == instance) instance = null;
        }

        private void Awake()
        {
            // �����C���X�^���X�����݂���Ȃ�A�����j������
            if (instance != null)
            {
                Destroy(this.gameObject);
                return;
            }

            instance = this;

            touchManager = TouchManager.GetInstance();

            //�}�e���A���̎擾�B�B
            var image = this.gameObject.GetComponent<Image>();
            m_gaugeImageMat = image.material;

            var canvasRect = touchManager.GetCavasRect();
            m_gaugeTransform = this.gameObject.GetComponent<RectTransform>();

            //�Q�[�W�̃T�C�Y���v�Z�B
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
        /// �Q�[�W�̍��W��ݒ�B
        /// </summary>
        /// <param name="pos">
        /// ��ʍ�������_�ɂ����X�N���[����ԍ��W�B
        /// <remark>X = 0.0 �` 1.0</remark>
        /// <remark>Y = 0.0 �` 1.0</remark>
        /// </param>
        public void SetPosition(Vector2 pos)
        {
            var canvasRect = touchManager.GetCavasRect();
            pos.x -= 0.5f;
            pos.y -= 0.5f;
            //��ʏ�̍��W�ɕϊ��B
            m_gaugeTransform.anchoredPosition = pos * canvasRect.sizeDelta;
        }

        private void OnEnable()
        {
            //��ʏ�̍��W��ݒ�B
            SetPosition(touchManager.GetTouchPosInScreen());
        }
        /// <summary>
        /// ������͂��Z�b�g�B
        /// </summary>
        public void SetThrowPow(float pow)
        {
            m_throwPow = pow;
        }
        /// <summary>
        /// �Q�[�W�̃T�C�Y���擾�B
        /// </summary>
        public Vector2 GetGaugeSize()
        {
            return m_gaugeSize;
        }
    }
}
