using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BocciaPlayer
{
    public class BallHolderController : MonoBehaviour
    {
        public GameObject ballObj;      //�{�[���̃v���t�@�u�B
        public GameObject jackBallObj;  //�W���b�N�{�[���̃v���t�@�u�B
        public int ballCount = 6;       //�`�[���̃{�[���̐��B
        GameObject[] teamBalls;         //�{�[���̔z��B
        int currentBallNo = 0;          //���ݎg���{�[���̔ԍ��B
        GameObject gameFlowObj;         //�Q�[���t���[�I�u�W�F�N�g�擾�B
        BallFlowScript ballFlow;        //�{�[���t���[�B

        private void Awake()
        {
            //�{�[���̔z��m�ہB
            teamBalls = new GameObject[ballCount];
            for (int i = 0; i < ballCount; i++)
            {
                //�{�[���𐶐��B
                teamBalls[i] = Instantiate(ballObj);
                //�܂������Ȃ����ߗL���t���O�������B
                teamBalls[i].SetActive(false);
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

        //���ݎg���{�[�����擾����B
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
                //�L���ɂ���B
                teamBalls[currentBallNo].SetActive(true);
                return teamBalls[currentBallNo];
            }
            return null;
        }
        /// <summary>
        /// �{�[���ԍ���i�߂�B
        /// </summary>
        /// <returns>�����Ă���{�[���̐����z������false��Ԃ��B</returns>
        public bool UpdateCurrentBallNo()
        {
            if(teamBalls[currentBallNo].activeSelf)
            {
                currentBallNo++;
            }
            //�����Ă���{�[���̐����z���Ă��Ȃ����H�B
            return currentBallNo < ballCount;
        }
        /// <summary>
        /// �{�[�������Z�b�g����B
        /// </summary>
        public void ResetBall()
        {
            for (int i = 0; i < ballCount; i++)
            {
                //�L���t���O�������B
                teamBalls[i].SetActive(false);
                //�K���Ɉʒu���������B
                teamBalls[i].transform.position = Vector3.zero;
                teamBalls[i].transform.rotation = Quaternion.identity;
            }

        }
    }
}
