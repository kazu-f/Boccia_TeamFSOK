using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallFlowScript : Photon.Pun.MonoBehaviourPun
{
    //�V�[����ɃW���b�N�{�[�������邩�ǂ���
    private bool m_IsPreparedJack = false;
    public GameObject JackPrefab;
    private GameObject m_Jack = null;
    private BallStateScript m_JackState = null;
    private BallOperateScript m_BallOperate = null;

    private const float LOG_TIME = 10.0f;
    private float m_currentTime = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        if(photonView.IsMine)
        {
            photonView.RPC(nameof(CreateJackBallRPC), Photon.Pun.RpcTarget.AllBuffered, Photon.Pun.PhotonNetwork.AllocateViewID(true));
        }
        if(m_Jack == null)
        {
            Debug.LogError("JackBall��null�ł�");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (m_IsPreparedJack == false)
        {
            if (m_Jack != null)
            {
                //�X�e�[�g���擾
                m_JackState = m_Jack.GetComponent<BallStateScript>();
                if (m_JackState.GetState() == BallState.Stop && m_BallOperate.GetInArea())
                {
                    //�W���b�N�{�[�����������ꂽ�Ƃ��Ƀt���O�𗧂Ă�
                    m_IsPreparedJack = true;
                }
            }
        }

        m_currentTime -= Time.deltaTime;

        if(m_currentTime < 0.0f)
        {
            if (m_Jack == null)
            {
                Debug.LogError("JackBall�͑��݂��Ă��Ȃ��B");
            }
            else
            {
                Debug.Log("JackBall�͑��݂��Ă���B");
            }
            m_currentTime = LOG_TIME;
        }
    }

    //�V�[����ɃW���b�N�{�[�������邩�ǂ���
    public bool IsPreparedJack()
    {
        return m_IsPreparedJack;
    }

    public GameObject GetJackBall()
    {
        if (m_Jack == null)
        {
            Debug.LogError("GetJackBall() == null");
        }
        return m_Jack;
    }

    public void ResetVar()
    {
        var ballOperate = m_Jack.GetComponent<BallOperateScript>();
        ballOperate.ResetVar();

        m_IsPreparedJack = false;
        m_Jack.SetActive(false);
    }

    /// <summary>
    /// �{�[���̌������N�G�X�g�����s���B
    /// </summary>
    /// <param name="isRequest">���N�G�X�g���鑤���ǂ����B</param>
    public void RequestOwnerShip(bool isRequest)
    {
        if(isRequest)
        {
            var photonV = m_Jack.GetComponent<Photon.Pun.PhotonView>();
            if (photonV != null)
            {
                photonV.RequestOwnership();
            }
        }
        //���W�b�h�{�f�B�擾�B
        var RB = m_Jack.GetComponent<Rigidbody>();
        if(RB != null)
        {
            //�I�[�i�[���ǂ����ŕ������Z�����邩��؂�ւ���B
            RB.isKinematic = !isRequest;
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

    [Photon.Pun.PunRPC]
    public void CreateJackBallRPC(int viewID)
    {
        m_Jack = Instantiate(JackPrefab);
        m_Jack.SetActive(false);
        m_BallOperate = m_Jack.GetComponent<BallOperateScript>();
        //PhotonView�̎擾�B
        var photonV = m_Jack.GetComponent<Photon.Pun.PhotonView>();
        photonV.ViewID = viewID;
        photonV.ObservedComponents = new List<Component>();
        var photonTransformView = m_Jack.gameObject.GetComponent<Photon.Pun.PhotonTransformView>();
        var photonRigidBodyView = m_Jack.gameObject.GetComponent<Photon.Pun.PhotonRigidbodyView>();
        photonTransformView.m_SynchronizePosition = true;
        photonTransformView.m_SynchronizeRotation = true;
        photonRigidBodyView.m_SynchronizeVelocity = true;

        photonV.ObservedComponents.Add(photonRigidBodyView);
        photonV.ObservedComponents.Add(photonTransformView);

        photonV.OwnershipTransfer = Photon.Pun.OwnershipOption.Request;

        if (m_Jack != null)
        {
            Debug.Log("JackBall���쐬�B");
        }
        else
        {
            Debug.Log("JackBall�̍쐬�����s�B");
        }

        //var RB = m_Jack.GetComponent<Rigidbody>();
        //if (photonV.IsMine)
        //{
        //    RB.isKinematic = false;
        //}
        //else
        //{
        //    RB.isKinematic = true;
        //}
    }
}
