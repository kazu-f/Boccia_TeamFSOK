using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BocciaPlayer
{
    /// <summary>
    /// �v���C���[�̃f�[�^�X�e�[�g�B
    /// </summary>
    public enum EnPlayerDataState 
    { 
        enPlayerData_None = 0,
        enPlayerData_Gauge = 1 << 0,
        enPlayerData_Throw = 1 << 1,
    }


    public class PhotonPlayerController : IPlayerController
    {
        private Vector3 startPosition = new Vector3();           //�J�n���_�̍��W�B
        private Quaternion startRotation = new Quaternion();     //�J�n���_�̉�]�B
        private Vector3 oldPosition = new Vector3();           //�O�t���[���̍��W�B
        private Quaternion oldRotation = new Quaternion();           //�O�t���[���̍��W�B
        private bool isUpdating = false;
        private bool isThrowing = false;                        //�����ďI��������B

        private void Awake()
        {
            startPosition = this.gameObject.transform.position;
            startRotation = this.gameObject.transform.rotation;
            oldPosition = startPosition;
            InitPlayerScript();
            //�{�[��������X�N���v�g��؂�B
            throwBallControler.enabled = false;
            playerMoveScript.SetIsMove(false);
        }
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (!isUpdating || throwBallControler.IsDecision()) return;
            if(netSendManager == null)
            {
                Debug.Log("SendManager���擾�ł��Ă��Ȃ��B");
                isUpdating = false;
            }
            //���W�̒l���ω������B
            if(oldPosition != this.gameObject.transform.position)
            {
                playerMoveScript.SetIsMove(true);
                oldPosition = this.gameObject.transform.position;
            }
            else
            {
                playerMoveScript.SetIsMove(false);
            }

            //��ʂ�G���Ă���B
            if(netSendManager.ReceiveState() == (int)EnPlayerDataState.enPlayerData_Gauge)
            {
                throwBallControler.enabled = true;
                throwBallControler.StartThrowBall(netSendManager.ReceiveThrowGaugePos());
                throwBallControler.SetThrowPow(netSendManager.ReceiveThrowPower());
            }
            else
            {
                throwBallControler.enabled = false;
            }

        }

        override public void SwitchPlayer(bool isEnable)
        {
            isUpdating = isEnable;
            //�J�n���_�̃g�����X�t�H�[���ֈړ��B
            if (isEnable == true)
            {
                //�v���C���[���؂�ւ�鎞�ɃJ�����̈ʒu�����킹��B
                throwAngleController.ChangeCamPos();
            }
        }

        /// <summary>
        /// �v���C���[�����Z�b�g�B
        /// </summary>
        override public void ResetPlayer()
        {
            ballHolderController.ResetBall();
        }
    }

}
