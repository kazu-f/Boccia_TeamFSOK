using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BocciaPlayer
{
    public abstract class IPlayerController : MonoBehaviour
    {
        protected ThrowBallControler throwBallControler;
        protected ThrowAngleController throwAngleController;
        protected BallHolderController ballHolderController;

        /// <summary>
        /// プレイヤー処理関係のスクリプトを取得してくる。
        /// </summary>
        protected void InitPlayerScript()
        {
            throwBallControler = this.gameObject.GetComponentInChildren<ThrowBallControler>();
            throwAngleController = this.gameObject.GetComponentInChildren<ThrowAngleController>();
            ballHolderController = this.gameObject.GetComponentInChildren<BallHolderController>();
        }

        public ThrowBallControler GetThrowBallController()
        {
            return throwBallControler;
        }
        public ThrowAngleController GetThrowAngleController()
        {
            return throwAngleController;
        }
        public BallHolderController GetBallHolderController()
        {
            return ballHolderController;
        }

        abstract public void ResetPlayer();
        abstract public void SwitchPlayer(bool isEnable);
    }

}
