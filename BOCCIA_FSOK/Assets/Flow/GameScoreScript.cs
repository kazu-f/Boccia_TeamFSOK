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
        static private int MAX_END_NUM = 6;         //�L�^���o����ő�G���h���B
        private EndResult[] results = new EndResult[MAX_END_NUM];    //�ő�6�G���h�܂ŋL�^�\�B
        private int FinalEndNum = 0;           //���݂̃Q�[���̍ŏI�G���h���B

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
        /// <summary>
        /// �ŏI�G���h�����L�^���Ă����B
        /// </summary>
        /// <param name="endNum">�ŏI�G���h���B</param>
        public void SetFinalEndNum(int endNum)
        {
            if (endNum >= MAX_END_NUM)
            {
                Debug.LogError("���������߂��Ă��܂��B");
            }
            FinalEndNum = endNum;
        }
        /// <summary>
        /// �ŏI�G���h�����擾�B
        /// </summary>
        public int GetFinalEndNum()
        {
            return FinalEndNum;
        }

        /// <summary>
        /// �X�R�A���L�^����B
        /// </summary>
        /// <param name="endResult">�X�R�A���</param>
        /// <param name="currentEndNo">���݂̃G���h���B</param>
        public void RecordResult(EndResult endResult,int currentEndNo)
        {
            if(currentEndNo < FinalEndNum)
            {
                results[currentEndNo] = endResult;
            }
        }
        /// <summary>
        /// �G���h���U���g���擾�B
        /// </summary>
        /// <param name="endNo">�G���h�ԍ�</param>
        /// <returns>���̃G���h�̃��U���g�B</returns>
        public EndResult GetEndResult(int endNo)
        {
            if(endNo >= FinalEndNum
                || endNo >= MAX_END_NUM)
            {
                Debug.LogError("���������߂��Ă��܂��B");
            }
            return results[endNo];
        }

        /// <summary>
        /// �X�R�A���R�s�[����B
        /// </summary>
        private void CopyScore(EndResult[] origin)
        {
            this.results = origin;
        }

        /// <summary>
        /// �V�[����؂�ւ���Ƃ��ɃX�R�A�𑗂�C�x���g�B
        /// </summary>
        public void SendScoreNextScene(Scene next, LoadSceneMode mode)
        {
            //�Q�[���X�R�A��T���B
            foreach(var gameObject in next.GetRootGameObjects())
            {
                var gameScore = gameObject.GetComponent<GameScoreScript>();
                if (gameScore != null)
                {
                    //�X�R�A���R�s�[�B
                    gameScore.CopyScore(results);
                    //���f�B
                    break;
                }
            }
            //�C�x���g�������B
            SceneManager.sceneLoaded -= SendScoreNextScene;
        }
    }
}
