using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BocciaPlayer
{
    public abstract class IPlayerController : MonoBehaviour
    {
        protected ThrowBallControler throwBallControler;        //�{�[���𓊂���X�N���v�g�B
        protected ThrowAngleController throwAngleController;    //�����ύX�X�N���v�g�B
        protected BallHolderController ballHolderController;    //�{�[���Ǘ��X�N���v�g�B
        protected PlayerMoveScript playerMoveScript;            //�v���C���[�ړ��X�N���v�g�B

        /// <summary>
        /// �v���C���[�����֌W�̃X�N���v�g���擾���Ă���B
        /// </summary>
        protected void InitPlayerScript()
        {
            throwBallControler = this.gameObject.GetComponentInChildren<ThrowBallControler>();
            throwAngleController = this.gameObject.GetComponentInChildren<ThrowAngleController>();
            ballHolderController = this.gameObject.GetComponentInChildren<BallHolderController>();
            playerMoveScript = this.gameObject.GetComponentInChildren<PlayerMoveScript>();
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
