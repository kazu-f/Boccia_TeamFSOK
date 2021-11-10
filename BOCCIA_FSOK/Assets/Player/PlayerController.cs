using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BocciaPlayer
{
    public enum EnPlayerState { 
        enIdle,     //�^�b�`���͑҂��B
        enMove,     //�ړ��B
        enAngle,    //�p�x�����߂鏈���B
        enThrow,    //�����鏈���B
        enWait,     //�{�[�����~�܂�܂őҋ@�B
        enStop,     //������~�B
        enStateNum  //�X�e�[�g�̐��B
    }

    public class PlayerController : IPlayerController
    {
        private IPlayerState[] playerStateList;                 //�X�e�[�g�̃��X�g�B

        private IPlayerState currentState = null;
        private EnPlayerState enCurrentState = EnPlayerState.enStateNum;
        private Vector3 startPosition = new Vector3();           //�J�n���_�̍��W�B
        private Quaternion startRotation = new Quaternion();     //�J�n���_�̉�]�B
        private Vector3 oldPosition = new Vector3();           //�O�t���[���̍��W�B

        private void Awake()
        {
            startPosition = this.gameObject.transform.position;
            startRotation = this.gameObject.transform.rotation;
            oldPosition = startPosition;
            InitPlayerScript();
            //�X�e�[�g�������B
            playerStateList = new IPlayerState[(int)EnPlayerState.enStateNum];

            playerStateList[(int)EnPlayerState.enIdle] = new PlayerIdleState();
            playerStateList[(int)EnPlayerState.enMove] = new PlayerMoveState();
            playerStateList[(int)EnPlayerState.enAngle] = new PlayerThrowAngleState();
            playerStateList[(int)EnPlayerState.enThrow] = new PlayerThrowBallState();
            playerStateList[(int)EnPlayerState.enWait] = new PlayerWaitBallState();
            playerStateList[(int)EnPlayerState.enStop] = new PlayerStopState();
            playerStateList[(int)EnPlayerState.enIdle].Init(this);
            playerStateList[(int)EnPlayerState.enMove].Init(this);
            playerStateList[(int)EnPlayerState.enAngle].Init(this);
            playerStateList[(int)EnPlayerState.enThrow].Init(this);
            playerStateList[(int)EnPlayerState.enWait].Init(this);
            playerStateList[(int)EnPlayerState.enStop].Init(this);

            currentState = playerStateList[(int)EnPlayerState.enStop];
        }
        // Start is called before the first frame update
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
            //�X�e�[�g�����s�B
            currentState.Execute();
            //���W�ω�������Α��M�B
            if(oldPosition != this.transform.position)
            {
                if (netSendManager != null)
                {
                    netSendManager.SendPlayerPos(this.transform.position);
                }
                oldPosition = this.transform.position;
            }
        }

        override public void SwitchPlayer(bool isEnable)
        {
            //�J�n���_�̃g�����X�t�H�[���ֈړ��B
            this.gameObject.transform.position = startPosition;
            this.gameObject.transform.rotation = startRotation;
            oldPosition = startPosition;
            if (isEnable == true)
            {
                //�v���C���[���؂�ւ�鎞�ɃJ�����̈ʒu�����킹��B
                throwAngleController.ChangeCamPos();
                if (netSendManager != null)
                {
                    netSendManager.SendPlayerPos(this.transform.position);
                }
                ChangeState(EnPlayerState.enIdle);
            }
            else
            {
                ChangeState(EnPlayerState.enStop);
            }
        }

        /// <summary>
        /// �X�e�[�g��ύX�B
        /// </summary>
        /// <param name="enState">�X�e�[�g�ϐ��B</param>
        public void ChangeState(EnPlayerState enState)
        {
            //�X�e�[�g���ς���Ă��Ȃ��B
            if (enCurrentState == enState)
            {
                return;
            }
            
            if (currentState != null)
            {
                //�I�������B
                currentState.Leave();
            }
            enCurrentState = enState;
            //�X�e�[�g�ύX�B
            currentState = playerStateList[(int)enState];
            //�J�n�����B
            currentState.Enter();
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
