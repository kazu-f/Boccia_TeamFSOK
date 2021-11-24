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
        GameObject gameFlowObj;         //ゲームフローオブジェクト取得。
        BallFlowScript ballFlow;        //ボールフロー。
        TeamFlowScript teamFlow;        //チームフロー。

        private const float LOG_TIME = 10.0f;
        private float m_currentTime = 0.0f;

        private void Awake()
        {
            gameFlowObj = GameObject.FindGameObjectWithTag("GameFlow");
            ballFlow = gameFlowObj.GetComponent<BallFlowScript>();
            if (ballFlow == null)
            {
                Debug.LogError("BallFlowScriptが見つからない。");
            }
            teamFlow = gameFlowObj.GetComponent<TeamFlowScript>();
            if (teamFlow == null)
            {
                Debug.LogError("TeamFlowScriptが見つからない。");
            }
            else
            {
                ballCount = teamFlow.m_Remain;
            }


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
        }

        // Start is called before the first frame update
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
            m_currentTime -= Time.deltaTime;

            if (m_currentTime < 0.0f)
            {
                if (teamBalls == null)
                {
                    Debug.LogError("チームボールが存在しない。");
                }
                else
                {
                    Debug.Log("チームボールが存在する。");
                }

                m_currentTime = LOG_TIME;
            }
        }

        //現在使うボールを取得する。
        public GameObject GetCurrentBall()
        {
            if (ballFlow == null)
            {
                Debug.LogError("ジャックボールフローが見つからない。");
            }
            if (!ballFlow.IsPreparedJack())
            {
                var jackBall = ballFlow.GetJackBall();
                if(jackBall == null)
                {
                    Debug.LogError("ジャックボールが見つからない。");
                }
                jackBall.SetActive(true);
                return jackBall;
            }
            int currentNo = ballCount - teamFlow.GetRemainBall();
            if (currentNo < ballCount)
            {
                //有効にする。
                teamBalls[currentNo].SetActive(true);
                return teamBalls[currentNo];
            }
            return null;
        }

        /// <summary>
        /// ボールをリセットする。
        /// </summary>
        public void ResetBall()
        {
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
        /// ボールの権限リクエスト等を行う。
        /// </summary>
        /// <param name="isRequest">リクエストする側かどうか。</param>
        public void RequestOwnerShip(bool isRequest)
        {
            if (teamBalls == null) return;
            for (int i = 0; i < ballCount; i++)
            {
                if (isRequest)
                {
                    var photonV = teamBalls[i].GetComponent<Photon.Pun.PhotonView>();
                    if (photonV != null)
                    {
                        //リクエスト。
                        photonV.RequestOwnership();
                    }
                }
                //リジッドボディ取得。
                var RB = teamBalls[i].GetComponent<Rigidbody>();
                if (RB != null)
                {
                    //オーナーかどうかで物理演算させるかを切り替える。
                    RB.isKinematic = !isRequest;
                }

                if(i == ballCount - 1)
                {
                    if (isRequest)
                    {
                        Debug.Log("ボールの物理挙動開始。");
                    }
                    else
                    {
                        Debug.Log("ボールの物理挙動停止。");
                    }
                }
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
            var ballState = teamBalls[ballNo].gameObject.GetComponent<BallStateScript>();
            photonTransformView.m_SynchronizePosition = true;
            photonTransformView.m_SynchronizeRotation = true;

            photonV.ObservedComponents.Add(ballState);
            photonV.ObservedComponents.Add(photonTransformView);

            photonV.OwnershipTransfer = Photon.Pun.OwnershipOption.Request;

            var RB = teamBalls[ballNo].GetComponent<Rigidbody>();
            if (photonV.IsMine)
            {
                RB.isKinematic = false;
            }
            else
            {
                RB.isKinematic = true;
            }

            Debug.Log(ballNo + "個目のボールを作成。");
        }
    }
}
