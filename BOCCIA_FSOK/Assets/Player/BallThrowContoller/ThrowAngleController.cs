using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BocciaPlayer
{
    public class ThrowAngleController : MonoBehaviour
    {
        TouchManager touchManager;
        public GameObject playerCamera;
        public GameObject bocciaPlayer;
        public float angleSpeed = 20.0f;
        private Vector3 m_newCamAngle = new Vector3(0, 0, 0);
        private Vector3 m_newPlayerAngle = new Vector3(0, 0, 0);
        private Vector3 m_defaultCamRot = new Vector3(0, 0, 0);

        private const float MAX_ANGLE_Y = 80.0f;

        private void Awake()
        {
            touchManager = TouchManager.GetInstance();
            m_defaultCamRot = playerCamera.transform.eulerAngles;
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
                if (touchManager.GetPhase() == TouchPhase.Moved)
                {
                    var rotVec = touchManager.GetDeltaPosInScreen();
                    m_newCamAngle.y += angleSpeed * rotVec.x;
                    m_newPlayerAngle.y += angleSpeed * rotVec.x;

                    m_newCamAngle.y = Mathf.Clamp(m_newCamAngle.y, -MAX_ANGLE_Y, MAX_ANGLE_Y);
                    m_newPlayerAngle.y = Mathf.Clamp(m_newPlayerAngle.y, -MAX_ANGLE_Y, MAX_ANGLE_Y);

                    playerCamera.transform.localEulerAngles = m_newCamAngle;
                    bocciaPlayer.transform.localEulerAngles = m_newPlayerAngle;
                }
            }
        }

        public void ThrowAngleEnable()
        {
            this.enabled = true;
            m_newCamAngle = playerCamera.transform.localEulerAngles;
            m_newPlayerAngle = bocciaPlayer.transform.localEulerAngles;
        }

        public void ThrowAngleDisable()
        {
            this.enabled = false;
        }

        //このプレイヤーの位置に座標を合わせる。
        public void ChangeCamPos()
        {
            playerCamera.transform.position = this.transform.position;
            playerCamera.transform.eulerAngles = m_defaultCamRot;
            this.transform.eulerAngles = Vector3.zero;
            bocciaPlayer.transform.localEulerAngles = Vector3.zero;
            m_newCamAngle = m_defaultCamRot;
            m_newPlayerAngle = Vector3.zero;
        }
    }
}
