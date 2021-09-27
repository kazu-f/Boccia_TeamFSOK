using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallFlowScript : MonoBehaviour
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
        m_Jack = Instantiate(JackPrefab);
        m_Jack.SetActive(false);
        m_BallOperate = m_Jack.GetComponent<BallOperateScript>();

    }

    // Update is called once per frame
    void Update()
    {
        if (m_Jack != null)
        {
            //�X�e�[�g���擾
            m_JackState = m_Jack.GetComponent<BallStateScript>();
            if(m_JackState.GetState() == BallState.Stop && m_BallOperate.GetIsThrow())
            {
                //�W���b�N�{�[�����������ꂽ�Ƃ��Ƀt���O�𗧂Ă�
                m_IsPreparedJack = true;
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
}
