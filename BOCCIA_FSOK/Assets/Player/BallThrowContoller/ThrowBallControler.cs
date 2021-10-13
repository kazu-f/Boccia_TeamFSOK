using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BocciaPlayer
{
    public class ThrowBallControler : MonoBehaviour
    {
        TouchManager touchManager;                  //�^�b�`�R���\�[���B
        public BallHolderController ballHolder;     //�{�[�����L�ҁB
        private BallStateScript currentBallState = null;    //���ݓ�����{�[����ԁB
        public GameObject bocciaPlayer;             //�v���C���[�I�u�W�F�N�g�B
        public GameObject throwDummyObj;            //�\�����\���I�u�W�F�N�g�B
        private ThrowDummyScript throwDummy;        //�\�����\���X�N���v�g�B
        private AudioSource throwSE;                //������Ƃ���SE�B
        //private�B
        private ThrowGaugeScript throwGauge;
        private Vector2 m_touchStartPos = new Vector2(0.0f, 0.0f);     //�G��n�߂����W�B
        private Vector2 m_touchEndPos = new Vector2(0.0f, 0.0f);       //�����؂������W�B
        private Vector2 m_endToStart = new Vector2(0.0f, 0.0f);       //�J�n���W��������؂������W�܂ł̃x�N�g���B
        private Vector2 m_touchPosInScreen = new Vector2(0.0f, 0.0f); //���݂̃^�b�`���Ă�����W(�X�N���[�����W�n�H)
        private float m_throwPow = 0.0f;                //�������

        private Vector3 m_force = new Vector3(0.0f, 0.0f, 0.0f);      //�����B
        private Vector3 m_throwPos = new Vector3(0.0f, 0.0f, 0.0f);   //�{�[���̎n�_�B7

        private const float THROW_DELAY = 0.2f;           //������܂ł̃f�B���C�̎��ԁB
        private bool isDecision = false;           //������͂����肵�����B

        //�萔�B
        //const float FLICK_POWER = 0.005f;       //�t���b�N����p�̒萔�B
        const float MAX_THROW_POW = 200.0f;
        //�C���X�^���X�������ɌĂ΂��B
        private void Awake()
        {
            touchManager = TouchManager.GetInstance();

            throwGauge = ThrowGaugeScript.GetInstance();
            throwGauge.gameObject.SetActive(false);

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
            if (touchManager.IsTouch() && !isDecision)
            {
                //�O�t���[�����瓮��������B
                if (touchManager.GetPhase() != TouchInfo.Stationary)
                {
                    m_touchPosInScreen = touchManager.GetTouchPosInScreen();
                }

                if (touchManager.GetPhase() == TouchInfo.Moved)
                {
                    //�ړ�������̍��W�B
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
                        //�{�[���𓊂���B
                        isDecision = true;
                        StartCoroutine("ThrowBall");
                    }
                }
            }
            
            if(isDecision)
            {
                //Debug.Log("�{�[���������Ă��邩�B");
                if (currentBallState != null && 
                    currentBallState.GetState() == BallState.Stop)
                {
                    currentBallState = null;
                    //�{�[�������ɐi�߂�B
                    var playerCon = bocciaPlayer.GetComponent<PlayerController>();
                    playerCon.isThrowBallNone = ballHolder.UpdateCurrentBallNo();
                    isDecision = false;
                }
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
            m_force.y = 10.0f;
        }

        //�{�[���𓊂��鏈���B
        IEnumerator ThrowBall()
        {
            yield return new WaitForSeconds(THROW_DELAY);
            //���ݓ�����{�[�����擾����B
            var obj = ballHolder.GetCurrentBall();
            if(obj == null)
            {
                yield return null;
            }

            //�{�[���̈ʒu�����킹��B
            obj.transform.position = m_throwPos;
            obj.transform.rotation = this.transform.rotation;

            currentBallState = obj.GetComponent<BallStateScript>();

            //�{�[���ɓ�����͂�������B
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
            //�^�b�`�ʒu���������B
            m_touchStartPos = touchManager.GetTouchPosInScreen();
            m_touchEndPos = touchManager.GetTouchPosInScreen();
            //�{�[���𓊂�����W���v�Z�B
            CalcPosition();
            throwDummy.SetPosition(m_throwPos);
            //������͏������B
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
