using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BocciaPlayer
{
    public class PlayerMoveButton : MonoBehaviour
    {
        private PlayerMovePanelSlide planelSlide;
        private bool isForward = false;     //前ボタンを押しているか？
        private bool isBack = false;        //後ボタンを押しているか？
        private bool isRight = false;       //右ボタンを押しているか？
        private bool isLeft = false;        //左ボタンを押しているか？

        //シングルトンにする。
        static private PlayerMoveButton instance = null;    //インスタンス。
        //インスタンス取得。
        static public PlayerMoveButton GetInstance()
        {
            return instance;
        }

        private void Awake()
        {
            if(instance != null)
            {
                Debug.LogWarning("PlayerMoveButtonが複数作られようとしています。");
                Destroy(this);
                return;
            }
            instance = this;

            //パネル移動スクリプト取得。
            planelSlide = GetComponent<PlayerMovePanelSlide>();
        }
        private void OnDestroy()
        {
            if(instance == this)
            {
                instance = null;
            }
        }

        //有効かどうか。
        public bool IsActive()
        {
            return !planelSlide.IsMoving() && planelSlide.IsInitedUI();
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
        public void ResetButton()
        {
            isForward = false;
            isBack = false;
            isLeft = false;
            isRight = false;
        }
        private void OnDisable()
        {
            ResetButton();
        }
    }
}