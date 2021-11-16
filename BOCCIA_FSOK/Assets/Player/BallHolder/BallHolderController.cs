using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BocciaPlayer
{
    public class BallHolderController : Photon.Pun.MonoBehaviourPun
    {
        public GameObject ballObj;      //ボールのプレファブ。
        public int ballCount = 6;       //チームのボールの数。
        GameObject[] teamBalls;         //ボールの配列。
        int currentBallNo = 0;          //現在使うボールの番号。
        GameObject gameFlowObj;         //ゲームフローオブジェクト取得。
        BallFlowScript ballFlow;        //ボールフロー。

        private void Awake()
        {
            if (photonView.IsMine)
            {
                //ボールの配列確保。
                photonView.RPC(nameof(CreateList), Photon.Pun.RpcTarget.AllBuffered);

                for (int i = 0; i < ballCount; i++)
                {
                    object[] param = {
                    i,
                    Photon.Pun.PhotonNetwork.AllocateViewID(true)
                    };

                    photonView.RPC(nameof(CreateBallRPC), Photon.Pun.RpcTarget.AllBuffered, param);
                }
                Debug.Log("TeamBallを作成。");
            }

            gameFlowObj = GameObject.FindGameObjectWithTag("GameFlow");
            ballFlow = gameFlowObj.GetComponent<BallFlowScript>();
        }

        // Start is called before the first frame update
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {

        }

        //現在使うボールを取得する。
        public GameObject GetCurrentBall()
        {
            if(!ballFlow.IsPreparedJack())
            {
                var jackBall = ballFlow.GetJackBall();
                jackBall.SetActive(true);
                return jackBall;
            }
            if(currentBallNo < ballCount)
            {
                //有効にする。
                teamBalls[currentBallNo].SetActive(true);
                return teamBalls[currentBallNo];
            }
            return null;
        }
        /// <summary>
        /// ボール番号を進める。
        /// </summary>
        /// <returns>持っているボールの数を越えたらfalseを返す。</returns>
        public bool UpdateCurrentBallNo()
        {
            if(teamBalls[currentBallNo].activeSelf)
            {
                currentBallNo++;
            }
            //持っているボールの数を越えていないか？。
            return currentBallNo < ballCount;
        }
        /// <summary>
        /// ボールをリセットする。
        /// </summary>
        public void ResetBall()
        {
            currentBallNo = 0;
            for (int i = 0; i < ballCount; i++)
            {
                //有効フラグを消す。
                teamBalls[i].SetActive(false);
                //適当に位置を初期化。
                teamBalls[i].transform.position = Vector3.zero;
                teamBalls[i].transform.rotation = Quaternion.identity;

                var ballOperate = teamBalls[i].GetComponent<BallOperateScript>();
                ballOperate.ResetVar();
            }

        }
        /// <summary>
        /// リストの作成を同期。
        /// </summary>
        /// <remarks>ボールの作成にリストがないと上手く動作しなくなる?</remarks>
        [Photon.Pun.PunRPC]
        public void CreateList()
        {
            teamBalls = new GameObject[ballCount];
        }
        /// <summary>
        /// ボールを作成する。
        /// </summary>
        /// <param name="ballNo"></param>
        /// <param name="viewID"></param>
        [Photon.Pun.PunRPC]
        public void CreateBallRPC(int ballNo,int viewID)
        {
            //ボールを生成。
            teamBalls[ballNo] = Instantiate(ballObj, Vector3.zero, Quaternion.identity);
            //まだ投げないため有効フラグを消す。
            teamBalls[ballNo].SetActive(false);
            //PhotonViewの取得。
            var photonV = teamBalls[ballNo].GetComponent<Photon.Pun.PhotonView>();
            photonV.ViewID = viewID;
            photonV.ObservedComponents = new List<Component>();
            var photonTransformView = teamBalls[ballNo].gameObject.GetComponent<Photon.Pun.PhotonTransformView>();
            photonTransformView.m_SynchronizePosition = true;
            photonTransformView.m_SynchronizeRotation = true;
            var photonRigidbodyView = teamBalls[ballNo].gameObject.GetComponent<Photon.Pun.PhotonRigidbodyView>();
            photonV.ObservedComponents.Add(photonTransformView);
            photonV.ObservedComponents.Add(photonRigidbodyView);
        }
    }
}
