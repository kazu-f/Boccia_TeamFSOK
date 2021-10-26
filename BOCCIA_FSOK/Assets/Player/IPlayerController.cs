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
        protected PlayerMoveScript playerMoveScript;

        /// <summary>
        /// プレイヤー処理関係のスクリプトを取得してくる。
        /// </summary>
        protected void InitPlayerScript()
        {
            throwBallControler = this.gameObject.GetComponentInChildren<ThrowBallControler>();
            throwAngleController = this.gameObject.GetComponentInChildren<ThrowAngleController>();
            ballHolderController = this.gameObject.GetComponentInChildren<BallHolderController>();
            playerMoveScript = this.gameObject.GetComponentInChildren<PlayerMoveScript>();
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
        public PlayerMoveScript GetPlayerMoveScript()
        {
            return playerMoveScript;
        }

        abstract public void ResetPlayer();
        abstract public void SwitchPlayer(bool isEnable);
    }

}
