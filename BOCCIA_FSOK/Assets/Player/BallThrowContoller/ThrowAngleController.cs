using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BocciaPlayer
{
    public class ThrowAngleController : MonoBehaviour
    {
        public GameObject angleArrowObj;      //イメージのオブジェクト。
        public GameObject playerCamera;         //カメラ。  
        public GameObject bocciaPlayer;         //プレイヤー。
        public float angleSpeed = 90.0f;        //回転速度。
        private Vector3 m_newCamAngle = new Vector3(0, 0, 0);
        private Vector3 m_newPlayerAngle = new Vector3(0, 0, 0);
        private Vector3 m_defaultCamRot = new Vector3(0, 0, 0);

        private const float MAX_ANGLE_Y = 80.0f;        //回転の制限。

        private void Awake()
        {
            //カメラの回転の初期値を記録。
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
        /// Y軸回転を行う。
        /// </summary>
        /// <param name="xSpeed">X方向の回転速度。</param>
        public void AngleRotation(float xSpeed)
        {
            //オイラー角でy軸回りの回転。
            m_newCamAngle.y += angleSpeed * xSpeed;
            m_newPlayerAngle.y += angleSpeed * xSpeed;

            //Y軸回転を-180～180に変換。
            m_newCamAngle.y = AngleTrans(m_newCamAngle.y);
            m_newPlayerAngle.y = AngleTrans(m_newPlayerAngle.y);

            m_newCamAngle.y = Mathf.Clamp(m_newCamAngle.y, -MAX_ANGLE_Y, MAX_ANGLE_Y);          //カメラの回転の制限。
            m_newPlayerAngle.y = Mathf.Clamp(m_newPlayerAngle.y, -MAX_ANGLE_Y, MAX_ANGLE_Y);    //プレイヤーの回転の制限。

            playerCamera.transform.localEulerAngles = m_newCamAngle;        //カメラの回転を入れる。
            bocciaPlayer.transform.localEulerAngles = m_newPlayerAngle;     //プレイヤーの回転を入れる。
        }
        /// <summary>
        /// Y軸回転をセットする。
        /// </summary>
        /// <param name="AngleY">Y軸回りの回転。オイラー角。</param>
        public void SetAngle(float AngleY)
        {
            //オイラー角でy軸回りの回転。
            m_newCamAngle.y = AngleY;
            m_newPlayerAngle.y = AngleY;

            //Y軸回転を-180～180に変換。
            m_newCamAngle.y = AngleTrans(m_newCamAngle.y);
            m_newPlayerAngle.y = AngleTrans(m_newPlayerAngle.y);

            m_newCamAngle.y = Mathf.Clamp(m_newCamAngle.y, -MAX_ANGLE_Y, MAX_ANGLE_Y);          //カメラの回転の制限。
            m_newPlayerAngle.y = Mathf.Clamp(m_newPlayerAngle.y, -MAX_ANGLE_Y, MAX_ANGLE_Y);    //プレイヤーの回転の制限。

            playerCamera.transform.localEulerAngles = m_newCamAngle;        //カメラの回転を入れる。
            bocciaPlayer.transform.localEulerAngles = m_newPlayerAngle;     //プレイヤーの回転を入れる。
        }

        //このスクリプトが有効になった時に呼ばれる。
        private void OnEnable()
        {
            //回転を記録。
            m_newCamAngle = playerCamera.transform.localEulerAngles;
            m_newPlayerAngle = bocciaPlayer.transform.localEulerAngles;
            //回転用のuGUIを有効に。
            angleArrowObj.SetActive(true);
        }

        private void OnDisable()
        {

        }

        //このプレイヤーの位置に座標を合わせる。
        public void ChangeCamPos()
        {
            //カメラの座標をプレイヤーに合わせる。
            playerCamera.transform.position = this.transform.position;
            //カメラの向きは初期値をつかう。
            playerCamera.transform.eulerAngles = m_defaultCamRot;
            this.transform.eulerAngles = Vector3.zero;
            bocciaPlayer.transform.localEulerAngles = Vector3.zero;
            m_newCamAngle = m_defaultCamRot;
            m_newPlayerAngle = Vector3.zero;
        }
        /// <summary>
        /// 回転を0～360＝＞-180～180に変換。
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
