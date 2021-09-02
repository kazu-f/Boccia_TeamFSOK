using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamFlowScript : MonoBehaviour
{
    public Team m_team = Team.Num;
    private BallFlowScript m_BallFlow;
    // Start is called before the first frame update
    void Start()
    {
        m_team = Team.Jack;
        m_BallFlow = GetComponent<BallFlowScript>();
    }

    // Update is called once per frame
    void Update()
    {
        //���ɓ�����`�[���ɂ���ď�����ς���
        switch(m_team)
        {
            //�W���b�N�{�[���𓊂���Ƃ�
            case Team.Jack:
                Debug.Log("���߂ɃW���b�N�{�[���𓊂��܂�");
                if(m_BallFlow.IsPreparedJack())
                {
                    //�W���b�N�{�[�����������ꂽ�玟�͐ԃ`�[����������
                    m_team = Team.Red;
                }
                break;

            case Team.Red:
                Debug.Log("���ɐԃ`�[���������܂�");
                break;

            case Team.Blue:
                Debug.Log("���ɐ`�[���������܂�");
                break;

            default:
                Debug.Log("<color=red>�G���[�F���ɓ�����`�[�������܂��Ă��܂���</color>");
                break;
        }
    }

    public Team GetNowTeam()
    {
        return m_team;
    }
}