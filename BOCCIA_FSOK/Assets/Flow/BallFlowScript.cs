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
    // Start is called before the first frame update
    void Start()
    {
        if(photonView.IsMine)
        {
            photonView.RPC(nameof(CreateJackBallRPC), Photon.Pun.RpcTarget.AllBuffered, Photon.Pun.PhotonNetwork.AllocateViewID(true));
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
    }

    //�V�[����ɃW���b�N�{�[�������邩�ǂ���
    public bool IsPreparedJack()
    {
        return m_IsPreparedJack;
    }

    public GameObject GetJackBall()
    {
        return m_Jack;
    }

    public void ResetVar()
    {
        var ballOperate = m_Jack.GetComponent<BallOperateScript>();
        ballOperate.ResetVar();

        m_IsPreparedJack = false;
        m_Jack.SetActive(false);
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
        photonTransformView.m_SynchronizePosition = true;
        photonTransformView.m_SynchronizeRotation = true;
        var photonRigidbodyView = m_Jack.gameObject.GetComponent<Photon.Pun.PhotonRigidbodyView>();
        photonV.ObservedComponents.Add(photonTransformView);
        photonV.ObservedComponents.Add(photonRigidbodyView);

        Debug.Log("JackBall���쐬�B");
    }
}
