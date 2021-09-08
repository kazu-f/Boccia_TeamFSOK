using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameFlowScript : MonoBehaviour
{
    private bool m_IsEnd = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (m_IsEnd)
        {
            CalcScore();
        }
    }

    /// <summary>
    /// �Q�[���I���̃t���O�𗧂Ă�
    /// </summary>
    public void GameEnd()
    {
        m_IsEnd = true;
    }

    /// <summary>
    /// ���_���v�Z����
    /// </summary>
    private void CalcScore()
    {
        GameObject m_Jack = null;
        m_Jack = GameObject.Find("JackBall");
        if (m_Jack == null)
        {
            //�C���X�^���X�̎擾�Ɏ��s�����Ƃ�
            Debug.Log("<color=red>�G���[�F�W���b�N�{�[���̎擾�Ɏ��s���܂���</color>");
        }
        Vector3 JackBallPos = m_Jack.GetComponent<Rigidbody>().position;
        //ball�Ƃ����^�O�̂����Q�[���I�u�W�F�N�g��z��ɓ����
        GameObject[] m_balls;
        m_balls = GameObject.FindGameObjectsWithTag("Ball");
        float[] m_BallsDist;
        //�{�[���̒������̔z����m��
        m_BallsDist = new float[m_balls.Length];
        Team[] Teams;
        Teams = new Team[m_balls.Length];

        //�ł��߂��{�[���܂ł̋���
        float[] NearDist;
        //�ԃ`�[���Ɛ`�[���̍ł��߂��{�[���̋�����ۑ����Ă����̂Ŕz��̃T�C�Y��2
        NearDist = new float[2];
        NearDist[0] = 10000;
        NearDist[1] = 10000;

        for (int ballnum = 0; ballnum < m_balls.Length; ballnum++)
        {
            //�`�[�����擾
            Teams[ballnum] = m_balls[ballnum].GetComponent<TeamDivisionScript>().GetTeam();
            //�{�[���̈ʒu���擾
            Vector3 BallPos = m_balls[ballnum].GetComponent<Rigidbody>().position;
            //�W���b�N�{�[���܂ł̋������v�Z
            Vector3 Dist = JackBallPos - BallPos;
            m_BallsDist[ballnum] = Dist.magnitude;

            if (Teams[ballnum] == Team.Red)
            {
                //���������߂����̂�ݒ肷��
                if (m_BallsDist[ballnum] < NearDist[0])
                {
                    NearDist[0] = m_BallsDist[ballnum];
                }
            }
            else
            {
                //���������߂����̂�ݒ肷��
                if (m_BallsDist[ballnum] < NearDist[1])
                {
                    NearDist[1] = m_BallsDist[ballnum];
                }
            }

        }

        int NearestTeamNum = 0;
        if (NearDist[0] < NearDist[1])
        {
            NearestTeamNum = 0;
        }
        else
        {
            NearestTeamNum = 1;
        }
        float SecondNearDist = Mathf.Max(NearDist[0], NearDist[1]);
        int score = 0;
        for (int ballnum = 0; ballnum < m_balls.Length; ballnum++)
        {
            if (m_BallsDist[ballnum] < SecondNearDist)
            {
                //�����Е��̈�ԋ߂��{�[�������߂��ʒu�ɂ���̂ŃX�R�A���グ��
                score++;
            }
        }

        if (NearestTeamNum == 0)
        {
            Debug.Log("�ԃ`�[��" + score);
        }
        else if(NearestTeamNum == 1)
        {
            Debug.Log("�`�[��" + score);
        }
    }
}
