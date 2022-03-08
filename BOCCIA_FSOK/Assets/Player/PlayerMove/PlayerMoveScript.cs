using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BocciaPlayer
{
    public class PlayerMoveScript : MonoBehaviour
    {
        new private Rigidbody rigidbody = null;
        private GameObject mainCamera;
        private bool isMove = false;        //動いているかどうか。

        private void Awake()
        {
            rigidbody = this.gameObject.GetComponentInParent<Rigidbody>();
            //カメラを探す。
            mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        }

        private void Update()
        {
            //動いている時だけカメラを動かす。
            if (isMove)
            {
                mainCamera.transform.position = this.gameObject.transform.position;
                isMove = false;
            }
        }

        public void PlayerMove(Vector3 moveSpeed)
        {
            rigidbody.velocity = moveSpeed;
        }

        public void SetIsMove(bool flag)
        {
            isMove = flag;
        }
    }

}