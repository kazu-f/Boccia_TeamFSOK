using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallFlowScript : MonoBehaviour
{
    private bool m_IsPreparedJack = false;
    private GameObject m_Jack = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (m_IsPreparedJack == false)
        {
            //�W���b�N�{�[������������Ă��Ȃ��Ƃ��W���b�N�{�[��������
            m_Jack = GameObject.FindGameObjectWithTag("Jack");
            if (m_Jack != null)
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
}
