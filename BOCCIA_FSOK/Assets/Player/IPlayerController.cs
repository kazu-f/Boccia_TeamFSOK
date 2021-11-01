using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BocciaPlayer
{
    public abstract class IPlayerController : MonoBehaviour
    {
        protected ThrowBallControler throwBallControler;        //ボールを投げるスクリプト。
        protected ThrowAngleController throwAngleController;    //方向変更スクリプト。
        protected BallHolderController ballHolderController;    //ボール管理スクリプト。
        protected PlayerMoveScript playerMoveScript;            //プレイヤー移動スクリプト。

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

        //ボールを投げる処理のスクリプト。
        public ThrowBallControler GetThrowBallController()
        {
            return throwBallControler;
        }
        //向きを変更する処理のスクリプト。
        public ThrowAngleController GetThrowAngleController()
        {
            return throwAngleController;
        }
        //ボールを管理するスクリプト。
        public BallHolderController GetBallHolderController()
        {
            return ballHolderController;
        }
        //プレイヤー移動のスクリプト。
        public PlayerMoveScript GetPlayerMoveScript()
        {
            return playerMoveScript;
        }

        /// <summary>
        /// エンド終了時、次エンド開始のためのリセット処理。
        /// </summary>
        abstract public void ResetPlayer();
        /// <summary>
        /// 投げるプレイヤーのチームを切り替える処理。
        /// </summary>
        /// <param name="isEnable">有効化フラグ。</param>
        abstract public void SwitchPlayer(bool isEnable);
    }

}
