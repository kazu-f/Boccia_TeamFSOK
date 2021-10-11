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
        //private�B
        private GameObject throwGauge;
        private RectTransform m_gaugeTransform;
        private Material m_gaugeImageMat;
        private Vector2 m_gaugeSize;
        private Vector2 m_touchStartPos = new Vector2(0.0f, 0.0f);     //�G��n�߂����W�B
        private Vector2 m_touchEndPos = new Vector2(0.0f, 0.0f);       //�����؂������W�B
        private Vector2 m_endToStart = new Vector2(0.0f, 0.0f);       //�J�n���W��������؂������W�܂ł̃x�N�g���B
        private Vector2 m_touchPosInScreen = new Vector2(0.0f, 0.0f); //���݂̃^�b�`���Ă�����W(�X�N���[�����W�n�H)
        private float m_throwPow = 0.0f;                //�������

        private Vector3 m_force = new Vector3(0.0f, 0.0f, 0.0f);      //�����B
        private Vector3 m_throwPos = new Vector3(0.0f, 0.0f, 0.0f);   //�{�[���̎n�_�B
        //�萔�B
        const float FLICK_POWER = 0.005f;       //�t���b�N����p�̒萔�B
        const float MAX_THROW_POW = 200.0f;
        //�C���X�^���X�������ɌĂ΂��B
        private void Awake()
        {
            touchManager = TouchManager.GetInstance();
            //�Q�[�W�̐e���L�����o�X�ɂ���B
            var canvas = touchManager.GetCanvas();
            //�����Q�[�W�쐬�B
            throwGauge = Instantiate(throwGaugePrefab, canvas.GetComponent<RectTransform>(),false);

            //�}�e���A���̎擾�B�B
            var image = throwGauge.GetComponent<Image>();
            m_gaugeImageMat = image.material;

            var canvasRect = touchManager.GetCavasRect();
            m_gaugeTransform = throwGauge.GetComponent<RectTransform>();
            m_gaugeSize.x = m_gaugeTransform.rect.width * m_gaugeTransform.localScale.x / canvasRect.sizeDelta.x;
            m_gaugeSize.y = m_gaugeTransform.rect.height * m_gaugeTransform.localScale.y / canvasRect.sizeDelta.y;

            throwGauge.SetActive(false);

            //�\�����̏����B
            throwDummy = throwDummyObj.GetComponent<ThrowDummyScript>();

            //SE���擾�B
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
                //�O�t���[�����瓮��������B
                if (touchManager.GetPhase() != TouchPhase.Stationary)
                {
                    m_touchPosInScreen = touchManager.GetTouchPosInScreen();
                }

                if (touchManager.GetPhase() == TouchPhase.Moved)
                {
                    var deltaMoveVec = touchManager.GetDeltaPosInScreen();
                    //������Ƀt���b�N���Ă��Ȃ���΁A������͂���߂悤�Ƃ��Ă���Ɣ��f����B
                    if (deltaMoveVec.y < FLICK_POWER
                        && m_touchPosInScreen.y < m_touchStartPos.y)
                    {
                        //�ړ�������̍��W�B
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
                    //�w�𗣂��^�C�~���O�œ����邩�ǂ�������B
                    if (m_touchStartPos.y < m_touchPosInScreen.y
                        && m_endToStart.y > 0.01f)
                    {
                        //�{�[���𓊂���B
                        ThrowBall();
                    }
                    throwGauge.SetActive(false);
                    throwDummyObj.SetActive(false);
                }
                m_gaugeImageMat.SetFloat("_ThrowPow", m_throwPow);
            }
        }
        /// <summary>
        /// �n�_���v�Z�B
        /// </summary>
        void CalcPosition()
        {
            m_throwPos = this.transform.position;
            m_throwPos.y *= m_touchStartPos.y;
        }
        /// <summary>
        /// �������v�Z�B
        /// </summary>
        void CalcThrowForce()
        {
            m_force = bocciaPlayer.transform.forward;       //�v���C���[�̑O���������B
            m_force.x *= MAX_THROW_POW * m_throwPow;
            m_force.z *= MAX_THROW_POW * m_throwPow;
            //m_force.y = 10.0f;
        }

        //�{�[���𓊂��鏈���B
        void ThrowBall()
        {
            //���ݓ�����{�[�����擾����B
            var obj = ballHolder.GetCurrentBall();
            if(obj == null)
            {
                return;
            }

            //�{�[���̈ʒu�����킹��B
            obj.transform.position = m_throwPos;
            obj.transform.rotation = this.transform.rotation;

            //�{�[���ɓ�����͂�������B
            var ballOperate = obj.GetComponent<BallOperateScript>();

            ballOperate.Throw(m_force);

            throwSE.Play();

            //�{�[�������ɐi�߂�B
            var playerCon = bocciaPlayer.GetComponent<PlayerController>();
            playerCon.isThrowBallNone = ballHolder.UpdateCurrentBallNo();
        }

        //�{�[���𓊂��鐧����J�n�B
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
