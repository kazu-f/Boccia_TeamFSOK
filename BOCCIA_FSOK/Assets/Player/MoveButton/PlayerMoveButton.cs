using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BocciaPlayer
{
    public class PlayerMoveButton : MonoBehaviour
    {
        private bool isForward = false;     //�O�{�^���������Ă��邩�H
        private bool isBack = false;        //��{�^���������Ă��邩�H
        private bool isRight = false;       //�E�{�^���������Ă��邩�H
        private bool isLeft = false;        //���{�^���������Ă��邩�H

        //�V���O���g���ɂ���B
        static private PlayerMoveButton instance = null;    //�C���X�^���X�B
        //�C���X�^���X�擾�B
        static public PlayerMoveButton GetInstance()
        {
            return instance;
        }

        private void Awake()
        {
            if(instance != null)
            {
                Debug.LogWarning("PlayerMoveButton����������悤�Ƃ��Ă��܂��B");
                return;
            }
            instance = this;
        }
        private void OnDestroy()
        {
            if(instance == this)
            {
                instance = null;
            }
        }

        public bool IsPressAnyButton()
        {
            return isForward || isBack || isRight || isLeft;
        }
        public bool IsPressForward()
        {
            return isForward;
        }
        public bool IsPressBack()
        {
            return isBack;
        }
        public bool IsPressRight()
        {
            return isRight;
        }
        public bool IsPressLeft()
        {
            return isLeft;
        }

        public void ForwardButtonDown()
        {
            isForward = true;
        }
        public void ForwardButtonUp()
        {
            isForward = false;
        }
        public void BackButtonDown()
        {
            isBack = true;
        }
        public void BackButtonUp()
        {
            isBack = false;
        }
        public void RightButtonDown()
        {
            isRight = true;
        }
        public void RightButtonUp()
        {
            isRight = false;
        }
        public void LeftButtonDown()
        {
            isLeft = true;
        }
        public void LeftButtonUp()
        {
            isLeft = false;
        }
    }
}