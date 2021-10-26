using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BocciaPlayer
{
    public class PlayerMoveScript : MonoBehaviour
    {
        new private Rigidbody rigidbody = null;
        [SerializeField]private Camera mainCamera;

        private void Awake()
        {
            rigidbody = GetComponent<Rigidbody>();
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