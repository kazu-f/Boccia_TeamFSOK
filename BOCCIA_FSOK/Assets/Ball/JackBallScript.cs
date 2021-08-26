using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JackBallScript : MonoBehaviour
{
    //�W���b�N�{�[���̖߂�ׂ��ʒu
    private Vector3 m_OriginPos = new Vector3(0.0f, 0.0f, 0.0f);
    //�W���b�N�{�[���̈ʒu
    private Vector3 m_pos = Vector3.zero;

    //�G���A�����ǂ������肷�邽�߂̃I�u�W�F�N�g
    private GameObject m_CourtArea;
    //�G���A���ɂ��邩�ǂ����̃t���O
    private bool m_InArea = true;

    private BallState m_state = BallState.Num;
    private Rigidbody m_rigidbody;
    private Vector3 m_moveSpeed;
    private BallStateScript m_BallStateScript;
    // Start is called before the first frame update
    void Start()
    {
        m_BallStateScript = GetComponent<BallStateScript>();

    }

    // Update is called once per frame
    void Update()
    {
        //�e���Ԃ��擾
        m_state = m_BallStateScript.GetState();
        m_rigidbody = m_BallStateScript.GetRigidbody();
        m_moveSpeed = m_BallStateScript.GetMoveSpeed();

        //�I�u�W�F�N�g��transform���擾
        Transform myTrans = this.transform;

        //transform������W���擾
        m_pos = myTrans.position;

        switch (m_state)
        {
            case BallState.Move:
                if(m_InArea == false)
                {
                    //�͈͊O�̂��ߋ}���Ɏ~�߂�
                    m_rigidbody.velocity += m_moveSpeed * -0.5f * Time.deltaTime;
                }

                break;

            case BallState.Stop:
                //�G���A�O�ɂ���Ƃ�
                if (m_InArea == false)
                {
                    //�N���X�ɖ߂�
                    m_pos = m_OriginPos;

                    m_InArea = true;
                }
                //���x��0�ɃZ�b�g
                m_rigidbody.velocity = Vector3.zero;

                break;
        }

        //���W��ݒ�
        myTrans.position = m_pos;

    }

    /// <summary>
    /// ���_�v�Z
    /// </summary>
    private void CalcScore()
    {

    }

    private void OnTriggerExit(Collider other)
    {
        m_InArea = false;
    }
}
