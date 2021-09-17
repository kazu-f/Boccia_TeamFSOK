using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BocciaPlayer
{
    public class BallHolderController : MonoBehaviour
    {
        public GameObject ballObj;      //ボールのプレファブ。
        public GameObject jackBallObj;  //ジャックボールのプレファブ。
        public int ballCount = 6;       //チームのボールの数。
        GameObject[] teamBalls;         //ボールの配列。
        int currentBallNo = 0;          //現在使うボールの番号。
        GameObject gameFlowObj;         //ゲームフローオブジェクト取得。
        BallFlowScript ballFlow;        //ボールフロー。
        
        // Start is called before the first frame update
        void Start()
        {
            //ボールの配列確保。
            teamBalls = new GameObject[ballCount];
            for (int i = 0; i < ballCount; i++)
            {
                //ボールを生成。
                teamBalls[i] = Instantiate(ballObj);
                //親をこのオブジェクトに設定する。
                teamBalls[i].transform.parent = this.gameObject.transform;
                //まだ投げないため有効フラグを消す。
                teamBalls[i].SetActive(false);
            }

            gameFlowObj = GameObject.FindGameObjectWithTag("GameFlow");
            ballFlow = gameFlowObj.GetComponent<BallFlowScript>();
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
                var jackBall = Instantiate(jackBallObj);
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
            for (int i = 0; i < ballCount; i++)
            {
                //有効フラグを消す。
                teamBalls[i].SetActive(false);
                //適当に位置を初期化。
                teamBalls[i].transform.position = Vector3.zero;
                teamBalls[i].transform.rotation = Quaternion.identity;
            }

        }
    }
}
