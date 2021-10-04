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
        static private int MAX_END_NUM = 6;         //�L�^���o����ő�G���h���B
        private EndResult[] results = new EndResult[MAX_END_NUM];    //�ő�6�G���h�܂ŋL�^�\�B
        private int currentGameEndNum = 0;           //���݂̃Q�[���̃G���h���B
        private int currendEndNo = 0;                //���ݍs���Ă���G���h�B

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
