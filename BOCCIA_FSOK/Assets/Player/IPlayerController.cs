using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BocciaPlayer
{
    public abstract class IPlayerController : MonoBehaviour
    {
        protected NetworkSendManagerScript netSendManager;       //�l�b�g���g���ăf�[�^�𑗂邽�߂̃X�N���v�g�B
        protected ThrowBallControler throwBallControler;        //�{�[���𓊂���X�N���v�g�B
        protected ThrowAngleController throwAngleController;    //�����ύX�X�N���v�g�B
        protected BallHolderController ballHolderController;    //�{�[���Ǘ��X�N���v�g�B
        protected PlayerMoveScript playerMoveScript;            //�v���C���[�ړ��X�N���v�g�B

        /// <summary>
        /// �v���C���[�����֌W�̃X�N���v�g���擾���Ă���B
        /// </summary>
        protected void InitPlayerScript()
        {
            //�q�I�u�W�F�N�g���炻�ꂼ��̃X�N���v�g���擾����B
            throwBallControler = this.gameObject.GetComponentInChildren<ThrowBallControler>();
            throwAngleController = this.gameObject.GetComponentInChildren<ThrowAngleController>();
            ballHolderController = this.gameObject.GetComponentInChildren<BallHolderController>();
            playerMoveScript = this.gameObject.GetComponentInChildren<PlayerMoveScript>();
            //�l�b�g���[�N�p�̃I�u�W�F�N�g���擾�B
            netSendManager = GameObject.FindGameObjectWithTag("Network").GetComponent<NetworkSendManagerScript>();

            if(netSendManager == null)
            {
                Debug.Log("NetworkSendManager���擾�ł��܂���ł����B");
            }
        }

        //�{�[���𓊂��鏈���̃X�N���v�g�B
        public ThrowBallControler GetThrowBallController()
        {
            return throwBallControler;
        }
        //������ύX���鏈���̃X�N���v�g�B
        public ThrowAngleController GetThrowAngleController()
        {
            return throwAngleController;
        }
        //�{�[�����Ǘ�����X�N���v�g�B
        public BallHolderController GetBallHolderController()
        {
            return ballHolderController;
        }
        //�v���C���[�ړ��̃X�N���v�g�B
        public PlayerMoveScript GetPlayerMoveScript()
        {
            return playerMoveScript;
        }
        //�f�[�^���l�b�g�ł���肷��X�N���v�g�B
        public NetworkSendManagerScript GetNetSendManager()
        {
            return netSendManager;
        }

        /// <summary>
        /// �G���h�I�����A���G���h�J�n�̂��߂̃��Z�b�g�����B
        /// </summary>
        abstract public void ResetPlayer();
        /// <summary>
        /// ������v���C���[�̃`�[����؂�ւ��鏈���B
        /// </summary>
        /// <param name="isEnable">�L�����t���O�B</param>
        abstract public void SwitchPlayer(bool isEnable);
    }

}
