using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BocciaPlayer
{
    public class PlayerMoveScript : MonoBehaviour
    {
        new private Rigidbody rigidbody = null;
        private GameObject mainCamera;

        private void Awake()
        {
            rigidbody = this.gameObject.GetComponentInParent<Rigidbody>();
            //ÉJÉÅÉâÇíTÇ∑ÅB
            mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        }

        private void Update()
        {
            mainCamera.transform.position = this.gameObject.transform.position;
        }

        public void PlayerMove(Vector3 moveSpeed)
        {
            rigidbody.velocity = moveSpeed;
        }
    }

}