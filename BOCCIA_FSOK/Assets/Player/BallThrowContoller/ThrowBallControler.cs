using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BocciaPlayer
{
    public class ThrowBallControler : Photon.Pun.MonoBehaviourPun
    {
        public BallHolderController ballHolder;     //�{�[�����L�ҁB
        public GameObject armObj;                 //���\��������X�N���v�g�B
        public GameObject bocciaPlayer;             //�v���C���[�I�u�W�F�N�g�B
        [SerializeField]private TimerFillScript timerFill;          //�^�C�}�[�B
        private ArmScript armScript;                 //���\��������X�N���v�g�B
        private GameObject throwDummyObj;            //�\�����\���I�u�W�F�N�g�B
        private ThrowDummyScript throwDummy;        //�\�����\���X�N���v�g�B
        private AudioSource throwSE;                //������Ƃ���SE�B
        private ThrowGaugeScript throwGauge;
        private ServerTimerScript serverTimer;      //�J�E���g���s�����߂̃X�N���v�g�B
        private Vector2 m_throwPow = new Vector2(0.0f,0.0f);                //�������
        private Vector3 m_force = new Vector3(0.0f, 0.0f, 0.0f);      //�����B
        private float m_throwPosHeight = 0.0f;                //�{�[���𓊂��鍂���̊����B
        [SerializeField] private float m_throwPosHeightMax = 1.7f;               //������n�_�̍����̍ő�l�B
        [SerializeField] private float m_throwPosHeightMin = 0.3f;               //������n�_�̍����̍ŏ��l�B
        private Vector3 m_throwPos = new Vector3(0.0f, 0.0f, 0.0f);   //�{�[���̎n�_�B

        private const float THROW_DELAY = 0.2f;           //������܂ł̃f�B���C�̎��ԁB
        private bool isDecision = false;           //������͂����肵�����B
        public bool isMyTeam = false;               //�����̃`�[�����ǂ����B

        //�萔�B
        const float SWITCH_ARM_BORDER = 0.6f;
        const float MAX_THROW_POW = 200.0f;
        const float MAX_ANGLE_POW = 50.0f;
        //�C���X�^���X�������ɌĂ΂��B
        private void Awake()
        {
            //�r�̃X�N���v�g�B
            armScript = armObj.GetComponent<ArmScript>();

            //�Q�[�W�\���̃X�N���v�g���擾�B
            throwGauge = ThrowGaugeScript.GetInstance();
            throwGauge.gameObject.SetActive(false);

            //�\�����̏������擾�B
            throwDummy = ThrowDummyScript.Instance();
            throwDummyObj = throwDummy.gameObject;
            throwDummyObj.SetActive(false);

            //SE���擾�B
            throwSE = GetComponent<AudioSource>();

            //�J�E���g���s���X�N���v�g�擾�B
            serverTimer = bocciaPlayer.GetComponent<ServerTimerScript>();

        }
        // Start is called before the first frame update
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
            //������Ƃ��ɂ͍X�V���Ȃ��B
            if (isDecision) return;
            //�{�[���𓊂�����W���v�Z�B
            CalcPosition();
            throwDummy.SetPosition(m_throwPos);
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
        /// ������͂��Z�b�g����B
        /// </summary>
        /// <param name="throwPow">x = -1.0�`1.0 , y = 0.0�`1.0</param>
        public void SetThrowPow(Vector2 throwPow)
        {
            m_throwPow = throwPow;
            throwGauge.SetThrowPow(m_throwPow.y);   //�Q�[�W�̈ʒu��ݒ�B
            CalcThrowForce();
            throwDummy.SetForce(m_force);           //������͂�ݒ�B
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
            m_throwPos = armScript.GetPosition();
            m_throwPos.y = Mathf.Lerp(m_throwPosHeightMin, m_throwPosHeightMax, m_throwPosHeight + armScript.GetFluctuation());
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
        /// <summary>
        /// ������ʒu���擾�B
        /// </summary>
        public Vector3 GetThrowPosition()
        {
            return m_throwPos;
        }
        /// <summary>
        /// ������ʒu�𒼐ڐݒ�B
        /// </summary>
        public void SetThrowPosition(Vector3 pos)
        {
            m_throwPos = pos;
        }

        //�{�[���𓊂���B
        public void ThrowBall()
        {
            object[] param = {
                m_force,
                m_throwPos,
                this.transform.rotation
            };

            //�J�E���g����B
            serverTimer.SetCountTimeSecond(THROW_DELAY);
            this.photonView.RPC(nameof(ThrowBallRPC), Photon.Pun.RpcTarget.All, param);
        }

        //�{�[���𓊂���R���[�`���J�n�B
        [Photon.Pun.PunRPC]
        void ThrowBallRPC(Vector3 force, Vector3 throwPos, Quaternion throwRot)
        {
            //�R���[�`�����J�n�B
            StartCoroutine(ThrowCoroutine(force, throwPos, throwRot));
        }

        //�{�[���𓊂��鏈���B
        IEnumerator ThrowCoroutine(Vector3 force, Vector3 throwPos, Quaternion throwRot)
        {
            isDecision = true;
            //yield return new WaitForSeconds(THROW_DELAY);
            //�J�E���g���I����Ă��邩�H
            while(!serverTimer.IsCountEnd())
            {
                yield return 0;
            }

            //�^�C���A�b�v���Ă���Ȃ璆�f�B
            if(timerFill.IsTimeUp())
            {
                //�R���[�`���̒��f�B
                yield break;
            }

            //���ݓ�����{�[�����擾����B
            var obj = ballHolder.GetCurrentBall();
            if (obj == null)
            {
                //�R���[�`���̒��f�B
                yield break;
            }

            //�{�[���̈ʒu�����킹��B
            obj.transform.position = throwPos;
            obj.transform.rotation = throwRot;

            //�{�[���ɓ�����͂�������B
            var ballOperate = obj.GetComponent<BallOperateScript>();

            ballOperate.Throw(force); //�{�[���𓊂���B

            throwSE.Play();     //������Ƃ���SE�B

            //�������̏������~�B
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
            //�L���ɂ���B
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
            //�F�X�ȏ����𖳌��ɂ���B
            throwGauge.gameObject.SetActive(false);
            throwDummyObj.SetActive(false);
            armScript.HoldDown();
        }
    }
}
