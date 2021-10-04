using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameScore
{
    public struct EndResult
    {
        int redTeamScore;
        int blueTeamScore;    
    }

    public class GameScoreScript : MonoBehaviour
    {
        static private int MAX_END_NUM = 6;         //記録が出来る最大エンド数。
        private EndResult[] results = new EndResult[MAX_END_NUM];    //最大6エンドまで記録可能。
        private int currentGameEndNum = 0;           //現在のゲームのエンド数。
        private int currendEndNo = 0;                //現在行っているエンド。

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void RecordResult(EndResult endResult)
        {
            results[currendEndNo] = endResult;
        }
    }
}
