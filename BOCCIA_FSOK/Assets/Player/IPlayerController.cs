using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BocciaPlayer
{
    public abstract class IPlayerController : MonoBehaviour
    {
        protected NetworkSendManagerScript netSendManager;       //ネットを使ってデータを送るためのスクリプト。
        protected ThrowBallControler throwBallControler;        //ボールを投げるスクリプト。
        protected ThrowAngleController throwAngleController;    //方向変更スクリプト。
        protected BallHolderController ballHolderController;    //ボール管理スクリプト。
        protected PlayerMoveScript playerMoveScript;            //プレイヤー移動スクリプト。

        /// <summary>
        /// プレイヤー処理関係のスクリプトを取得してくる。
        /// </summary>
        protected void InitPlayerScript()
        {
            //子オブジェクトからそれぞれのスクリプトを取得する。
            throwBallControler = this.gameObject.GetComponentInChildren<ThrowBallControler>();
            throwAngleController = this.gameObject.GetComponentInChildren<ThrowAngleController>();
            ballHolderController = this.gameObject.GetComponentInChildren<BallHolderController>();
            playerMoveScript = this.gameObject.GetComponentInChildren<PlayerMoveScript>();
            //ネットワーク用のオブジェクトを取得。
            netSendManager = GameObject.FindGameObjectWithTag("Network").GetComponent<NetworkSendManagerScript>();

            if(netSendManager == null)
            {
                Debug.Log("NetworkSendManagerが取得できませんでした。");
            }
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
        //データをネットでやり取りするスクリプト。
        public NetworkSendManagerScript GetNetSendManager()
        {
            return netSendManager;
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
