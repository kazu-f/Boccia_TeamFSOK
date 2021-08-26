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
        //private�B
        private RectTransform m_gaugeTransform;
        private Material m_gaugeImageMat;
        private Vector2 m_gaugeSize;
        private Vector2 m_touchStartPos = new Vector2(0.0f, 0.0f);     //�G��n�߂����W�B
        private Vector2 m_touchEndPos = new Vector2(0.0f, 0.0f);       //�����؂������W�B
        private Vector2 m_endToStart = new Vector2(0.0f, 0.0f);       //�J�n���W��������؂������W�܂ł̃x�N�g���B
        private Vector2 m_touchPosInScreen = new Vector2(0.0f, 0.0f); //���݂̃^�b�`���Ă�����W(�X�N���[�����W�n�H)
        private float m_throwPow = 0.0f;                //�������
        private bool m_isThrowing = false;              //���������ǂ����H
        //�萔�B
        const float FLICK_POWER = 0.005f;       //�t���b�N����p�̒萔�B
        const float MAX_THROW_POW = 300.0f;

        // Start is called before the first frame update
        void Start()
        {
            //�}�e���A���̎擾�B�B
            var image = throwGauge.GetComponent<Image>();
            m_gaugeImageMat = image.material;

            m_gaugeTransform = throwGauge.GetComponent<RectTransform>();
            m_gaugeSize.x = m_gaugeTransform.rect.width * m_gaugeTransform.localScale.x / canvasRect.sizeDelta.x;
            m_gaugeSize.y = m_gaugeTransform.rect.height * m_gaugeTransform.localScale.y / canvasRect.sizeDelta.y;

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
                    m_touchPosInScreen = touchManager.GetTouchPosInScreen();
                }

                if (touchManager.GetPhase() == TouchPhase.Moved)
                {
                    var deltaMoveVec = touchManager.GetDeltaPosInScreen();
                    //������Ƀt���b�N���Ă��Ȃ���΁B
                    if (deltaMoveVec.y < FLICK_POWER
                        && m_touchPosInScreen.y < m_touchStartPos.y)
                    {
                        //�ړ�������̍��W�B
                        m_touchEndPos = m_touchPosInScreen;
                        m_endToStart = m_touchStartPos - m_touchEndPos;
                        m_throwPow = m_endToStart.y / m_gaugeSize.y;
                        m_throwPow = Mathf.Min(1.0f, Mathf.Max(m_throwPow, 0.0f));
                    }
                }
                else if (touchManager.GetPhase() == TouchPhase.Ended)
                {
                    if (m_touchStartPos.y < m_touchPosInScreen.y
                        && m_endToStart.y > 0.01f)
                    {
                        var ballPos = this.transform.position;
                        ballPos.y *= m_touchStartPos.y;
                        var obj = Instantiate(ball, ballPos, this.transform.rotation);
                        Rigidbody ballRB = obj.GetComponent<Rigidbody>();

                        Vector3 vec = bocciaPlayer.transform.forward;       //�v���C���[�̑O���������B
                        vec.x *= MAX_THROW_POW * m_throwPow;
                        vec.z *= MAX_THROW_POW * m_throwPow;
                        vec.y = 10.0f;

                        ballRB.AddForce(vec);

                        m_isThrowing = true;
                    }
                }
                m_gaugeImageMat.SetFloat("_ThrowPow", m_throwPow);
            }
            else
            {
                throwGauge.SetActive(false);
                m_isThrowing = false;
            }
        }

        public void ThrowBallEnable()
        {
            this.enabled = true;
            m_touchStartPos = touchManager.GetTouchPosInScreen();
            m_touchEndPos = touchManager.GetTouchPosInScreen();
            m_gaugeTransform.anchoredPosition = touchManager.GetTouchPos();
            m_throwPow = 0.0f;
        }

        public void ThrowBallDisable()
        {
            this.enabled = false;
        }
        /// <summary>
        /// ���������u�Ԃǂ����H
        /// </summary>
        public bool IsThrowing()
        {
            return m_isThrowing;
        }
    }
}
