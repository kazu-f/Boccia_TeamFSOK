using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BocciaPlayer
{
    public class ThrowAngleController : MonoBehaviour
    {
        public GameObject angleArrowObj;      //�C���[�W�̃I�u�W�F�N�g�B
        public GameObject playerCamera;         //�J�����B  
        public GameObject bocciaPlayer;         //�v���C���[�B
        public float angleSpeed = 90.0f;        //��]���x�B
        private Vector3 m_newCamAngle = new Vector3(0, 0, 0);
        private Vector3 m_newPlayerAngle = new Vector3(0, 0, 0);
        private Vector3 m_defaultCamRot = new Vector3(0, 0, 0);

        private const float MAX_ANGLE_Y = 80.0f;        //��]�̐����B

        private void Awake()
        {
            m_defaultCamRot = playerCamera.transform.eulerAngles;
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
        /// Y����]���s���B
        /// </summary>
        /// <param name="xSpeed">X�����̉�]���x�B</param>
        public void AngleRotation(float xSpeed)
        {
            m_newCamAngle.y += angleSpeed * xSpeed;
            m_newPlayerAngle.y += angleSpeed * xSpeed;

            m_newCamAngle.y = AngleTrans(m_newCamAngle.y);
            m_newPlayerAngle.y = AngleTrans(m_newPlayerAngle.y);

            m_newCamAngle.y = Mathf.Clamp(m_newCamAngle.y, -MAX_ANGLE_Y, MAX_ANGLE_Y);
            m_newPlayerAngle.y = Mathf.Clamp(m_newPlayerAngle.y, -MAX_ANGLE_Y, MAX_ANGLE_Y);

            playerCamera.transform.localEulerAngles = m_newCamAngle;
            bocciaPlayer.transform.localEulerAngles = m_newPlayerAngle;
        }

        private void OnEnable()
        {
            m_newCamAngle = playerCamera.transform.localEulerAngles;
            m_newPlayerAngle = bocciaPlayer.transform.localEulerAngles;
            angleArrowObj.SetActive(true);
        }

        private void OnDisable()
        {

        }

        //���̃v���C���[�̈ʒu�ɍ��W�����킹��B
        public void ChangeCamPos()
        {
            playerCamera.transform.position = this.transform.position;
            playerCamera.transform.eulerAngles = m_defaultCamRot;
            this.transform.eulerAngles = Vector3.zero;
            bocciaPlayer.transform.localEulerAngles = Vector3.zero;
            m_newCamAngle = m_defaultCamRot;
            m_newPlayerAngle = Vector3.zero;
        }
        /// <summary>
        /// ��]��0�`360����-180�`180�ɕϊ��B
        /// </summary>
        private float AngleTrans(float angle)
        {
            if(angle > 180.0f)
            {
                angle -= 360.0f;
            }
            return angle;
        }
    }
}
