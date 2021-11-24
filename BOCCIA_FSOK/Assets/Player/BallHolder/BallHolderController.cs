using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BocciaPlayer
{
    public class BallHolderController : Photon.Pun.MonoBehaviourPun
    {
        public GameObject ballObj;      //�{�[���̃v���t�@�u�B
        public int ballCount = 6;       //�`�[���̃{�[���̐��B
        GameObject[] teamBalls;         //�{�[���̔z��B
        GameObject gameFlowObj;         //�Q�[���t���[�I�u�W�F�N�g�擾�B
        BallFlowScript ballFlow;        //�{�[���t���[�B
        TeamFlowScript teamFlow;        //�`�[���t���[�B

        private const float LOG_TIME = 10.0f;
        private float m_currentTime = 0.0f;

        private void Awake()
        {
            gameFlowObj = GameObject.FindGameObjectWithTag("GameFlow");
            ballFlow = gameFlowObj.GetComponent<BallFlowScript>();
            if (ballFlow == null)
            {
                Debug.LogError("BallFlowScript��������Ȃ��B");
            }
            teamFlow = gameFlowObj.GetComponent<TeamFlowScript>();
            if (teamFlow == null)
            {
                Debug.LogError("TeamFlowScript��������Ȃ��B");
            }
            else
            {
                ballCount = teamFlow.m_Remain;
            }


            if (photonView.IsMine)
            {
                //�{�[���̔z��m�ہB
                photonView.RPC(nameof(CreateList), Photon.Pun.RpcTarget.AllBuffered);

                for (int i = 0; i < ballCount; i++)
                {
                    object[] param = {
                    i,
                    Photon.Pun.PhotonNetwork.AllocateViewID(true)
                    };

                    photonView.RPC(nameof(CreateBallRPC), Photon.Pun.RpcTarget.AllBuffered, param);
                }
                Debug.Log("TeamBall���쐬�B");
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
                    Debug.LogError("�`�[���{�[�������݂��Ȃ��B");
                }
                else
                {
                    Debug.Log("�`�[���{�[�������݂���B");
                }

                m_currentTime = LOG_TIME;
            }
        }

        //���ݎg���{�[�����擾����B
        public GameObject GetCurrentBall()
        {
            if (ballFlow == null)
            {
                Debug.LogError("�W���b�N�{�[���t���[��������Ȃ��B");
            }
            if (!ballFlow.IsPreparedJack())
            {
                var jackBall = ballFlow.GetJackBall();
                if(jackBall == null)
                {
                    Debug.LogError("�W���b�N�{�[����������Ȃ��B");
                }
                jackBall.SetActive(true);
                return jackBall;
            }
            int currentNo = ballCount - teamFlow.GetRemainBall();
            if (currentNo < ballCount)
            {
                //�L���ɂ���B
                teamBalls[currentNo].SetActive(true);
                return teamBalls[currentNo];
            }
            return null;
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

                var ballOperate = teamBalls[i].GetComponent<BallOperateScript>();
                ballOperate.ResetVar();
            }
        }

        /// <summary>
        /// �{�[���̌������N�G�X�g�����s���B
        /// </summary>
        /// <param name="isRequest">���N�G�X�g���鑤���ǂ����B</param>
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
                        //���N�G�X�g�B
                        photonV.RequestOwnership();
                    }
                }
                //���W�b�h�{�f�B�擾�B
                var RB = teamBalls[i].GetComponent<Rigidbody>();
                if (RB != null)
                {
                    //�I�[�i�[���ǂ����ŕ������Z�����邩��؂�ւ���B
                    RB.isKinematic = !isRequest;
                }

                if(i == ballCount - 1)
                {
                    if (isRequest)
                    {
                        Debug.Log("�{�[���̕��������J�n�B");
                    }
                    else
                    {
                        Debug.Log("�{�[���̕���������~�B");
                    }
                }
            }

        }

        /// <summary>
        /// ���X�g�̍쐬�𓯊��B
        /// </summary>
        /// <remarks>�{�[���̍쐬�Ƀ��X�g���Ȃ��Ə�肭���삵�Ȃ��Ȃ�?</remarks>
        [Photon.Pun.PunRPC]
        public void CreateList()
        {
            teamBalls = new GameObject[ballCount];
        }
        /// <summary>
        /// �{�[�����쐬����B
        /// </summary>
        /// <param name="ballNo"></param>
        /// <param name="viewID"></param>
        [Photon.Pun.PunRPC]
        public void CreateBallRPC(int ballNo,int viewID)
        {
            //�{�[���𐶐��B
            teamBalls[ballNo] = Instantiate(ballObj, Vector3.zero, Quaternion.identity);
            //�܂������Ȃ����ߗL���t���O�������B
            teamBalls[ballNo].SetActive(false);
            //PhotonView�̎擾�B
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

            Debug.Log(ballNo + "�ڂ̃{�[�����쐬�B");
        }
    }
}
