using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameScore
{
    public struct EndResult
    {
        public int redTeamScore;
        public int blueTeamScore;    
    }

    public class GameScoreScript : MonoBehaviour
    {
        static private int MAX_END_NUM = 6;         //記録が出来る最大エンド数。
        private EndResult[] results = new EndResult[MAX_END_NUM];    //最大6エンドまで記録可能。
        private int FinalEndNum = 0;           //現在のゲームの最終エンド数。

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
        /// <summary>
        /// 最終エンド数を記録しておく。
        /// </summary>
        /// <param name="endNum">最終エンド数。</param>
        public void SetFinalEndNum(int endNum)
        {
            if (endNum >= MAX_END_NUM)
            {
                Debug.LogError("引数が超過しています。");
            }
            FinalEndNum = endNum;
        }
        /// <summary>
        /// 最終エンド数を取得。
        /// </summary>
        public int GetFinalEndNum()
        {
            return FinalEndNum;
        }

        /// <summary>
        /// スコアを記録する。
        /// </summary>
        /// <param name="endResult">スコア情報</param>
        /// <param name="currentEndNo">現在のエンド数。</param>
        public void RecordResult(EndResult endResult,int currentEndNo)
        {
            if(currentEndNo < FinalEndNum)
            {
                results[currentEndNo] = endResult;
            }
        }
        /// <summary>
        /// エンドリザルトを取得。
        /// </summary>
        /// <param name="endNo">エンド番号</param>
        /// <returns>そのエンドのリザルト。</returns>
        public EndResult GetEndResult(int endNo)
        {
            if(endNo >= FinalEndNum
                || endNo >= MAX_END_NUM)
            {
                Debug.LogError("引数が超過しています。");
            }
            return results[endNo];
        }

        /// <summary>
        /// スコアをコピーする。
        /// </summary>
        private void CopyScore(EndResult[] origin)
        {
            this.results = origin;
        }

        /// <summary>
        /// シーンを切り替えるときにスコアを送るイベント。
        /// </summary>
        public void SendScoreNextScene(Scene next, LoadSceneMode mode)
        {
            //ゲームスコアを探す。
            foreach(var gameObject in next.GetRootGameObjects())
            {
                var gameScore = gameObject.GetComponent<GameScoreScript>();
                if (gameScore != null)
                {
                    //スコアをコピー。
                    gameScore.CopyScore(results);
                    //中断。
                    break;
                }
            }
            //イベントを消す。
            SceneManager.sceneLoaded -= SendScoreNextScene;
        }
    }
}
