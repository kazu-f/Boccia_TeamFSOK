using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BocciaPlayer
{
    public class ThrowBallControler : MonoBehaviour
    {
        public BallHolderController ballHolder;     //�{�[�����L�ҁB
        public GameObject armObj;                 //���\��������X�N���v�g�B
        public GameObject bocciaPlayer;             //�v���C���[�I�u�W�F�N�g�B
        private ArmScript armScript;                 //���\��������X�N���v�g�B
        private GameObject throwDummyObj;            //�\�����\���I�u�W�F�N�g�B
        private ThrowDummyScript throwDummy;        //�\�����\���X�N���v�g�B
        private AudioSource throwSE;                //������Ƃ���SE�B
        private ThrowGaugeScript throwGauge;
        private Vector2 m_throwPow = new Vector2(0.0f,0.0f);                //�������
        private Vector3 m_force = new Vector3(0.0f, 0.0f, 0.0f);      //�����B
        private float m_throwPosHeight = 0.0f;                //�{�[���𓊂��鍂���̊����B
        private Vector3 m_throwPos = new Vector3(0.0f, 0.0f, 0.0f);   //�{�[���̎n�_�B

        private const float THROW_DELAY = 0.2f;           //������܂ł̃f�B���C�̎��ԁB
        private bool isDecision = false;           //������͂����肵�����B

        //�萔�B
        const float MAX_THROW_POW = 200.0f;
        const float MAX_ANGLE_POW = 50.0f;
        //�C���X�^���X�������ɌĂ΂��B
        private void Awake()
        {
            armScript = armObj.GetComponent<ArmScript>();

            throwGauge = ThrowGaugeScript.GetInstance();
            throwGauge.gameObject.SetActive(false);

            //�\�����̏����B
            throwDummy = ThrowDummyScript.Instance();
            throwDummyObj = throwDummy.gameObject;

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
            
        }
        
        /// <summary>
        /// �{�[���𓊂��邽�߂̏������J�n�B
        /// </summary>
        /// <param name="screenPos">�Q�[�W�̃X�N���[����ԏ�̍��W�Bxy = 0.0�`1.0</param>
        public void StartThrowBall(Vector2 screenPos)
        {
            throwGauge.SetPosition(screenPos);
            SetThrowHeight(screenPos.y);
            //�{�[���𓊂�����W���v�Z�B
            CalcPosition();
            throwDummy.SetPosition(m_throwPos);
            //�r�̍\������؂�ւ���B
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
        /// ������͂��Z�b�g����B
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
        /// ������ݒ�B
        /// </summary>
        /// <param name="height">0.0�`1.0�͈̔͂ŗ^����B</param>
        void SetThrowHeight(float height)
        {
            m_throwPosHeight = height;
        }
        /// <summary>
        /// �n�_���v�Z�B
        /// </summary>
        void CalcPosition()
        {
            m_throwPos = this.transform.position;
            m_throwPos.y *= m_throwPosHeight;
        }
        /// <summary>
        /// �������v�Z�B
        /// </summary>
        void CalcThrowForce()
        {
            Vector3 forward = bocciaPlayer.transform.forward;       //�v���C���[�̑O���������B
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

        //�{�[���𓊂���B
        public void ThrowBall()
        {
            StartCoroutine(ThrowCoroutine());
        }

        //�{�[���𓊂��鏈���B
        IEnumerator ThrowCoroutine()
        {
            isDecision = true;
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

            //�{�[���ɓ�����͂�������B
            var ballOperate = obj.GetComponent<BallOperateScript>();

            ballOperate.Throw(m_force);

            throwSE.Play();

            throwGauge.gameObject.SetActive(false);
            throwDummyObj.SetActive(false);

            //�{�[�������ɐi�߂�B
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
            //������͏������B
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
