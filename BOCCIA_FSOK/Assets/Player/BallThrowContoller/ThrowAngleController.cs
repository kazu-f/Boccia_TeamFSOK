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
            playerCamera.transform.eulerAngles = Vector3.zero;
            this.transform.eulerAngles = Vector3.zero;
        }

        //タッチマネージャーを取得。
        public void SetTouchManager(TouchManager manager)
        {
            touchManager = manager;
        }
    }
}
