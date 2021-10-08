using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct VictoryTeam
{
    private Team NearestTeam;       //�ł��߂��`�[��
    private int Score;      //�_��

    public void SetNearestTeam(Team team)
    {
        NearestTeam = team;
    }

    public Team GetNearestTeam()
    {
        return NearestTeam;
    }

    public void SetScore(int score)
    {
        Score = score;
    }

    public int GetScore()
    {
        return Score;
    }
};

public class EndFlowScript : MonoBehaviour
{
    private bool m_IsEnd = false;       //�G���h���I���������ǂ���
    private bool m_Calced = false;
    private VictoryTeam m_VicTeam;
    private bool IsShowLog = false;
    BallFlowScript ballFlow;
    DispScoreScript DispScore = null;
    TeamFlowScript m_TeamFlow = null;
    private void Awake()
    {
        ballFlow = this.gameObject.GetComponent<BallFlowScript>();
        DispScore = this.gameObject.GetComponent<DispScoreScript>();
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (m_IsEnd)
        {
            //�X�R�A���v�Z
            CalcScore();
            if (IsShowLog == false)
            {
                //�X�R�A��\��
                ScoreLog();
                IsShowLog = true;
            }
            //�e�L�X�g�̐F��ύX
            DispScore.SetTextTeam(m_VicTeam.GetNearestTeam());
            //���U���g��\��
            DispScore.DispResult(m_VicTeam.GetScore());
            m_Calced = true;
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
    /// ���_�v�Z�I����Ă��邩�ǂ���
    /// </summary>
    /// <returns>�v�Z�I�����Ă��邩�ǂ���</returns>
    public bool GetCalced()
    {
        return m_Calced;
    }
    /// <summary>
    /// �G���h���I�����Ă��邩�ǂ������擾
    /// </summary>
    /// <returns>�I�����Ă��邩�ǂ���</returns>
    public bool GetIsEnd()
    {
        return m_IsEnd;
    }
    /// <summary>
    /// ���_���v�Z����
    /// </summary>
    private void CalcScore()
    {
        GameObject m_Jack = null;
        m_Jack = ballFlow.GetJackBall();
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

        if (NearDist[0] < NearDist[1])
        {
            m_VicTeam.SetNearestTeam(Team.Red);
        }
        else
        {
            m_VicTeam.SetNearestTeam(Team.Blue);
        }

        float SecondNearDist = Mathf.Max(NearDist[0], NearDist[1]);
        //�X�R�A
        int score = 0;
        for (int ballnum = 0; ballnum < m_balls.Length; ballnum++)
        {
            if (m_BallsDist[ballnum] < SecondNearDist)
            {
                //�����Е��̈�ԋ߂��{�[�������߂��ʒu�ɂ���̂ŃX�R�A���グ��
                score++;
            }
        }
        m_VicTeam.SetScore(score);
    }

    public VictoryTeam GetVictoryTeam()
    {
        return m_VicTeam;
    }

    private void ScoreLog()
    {
        if (m_VicTeam.GetNearestTeam() == Team.Red)
        {
            Debug.Log("�ԃ`�[��" + m_VicTeam.GetScore());
        }
        else if (m_VicTeam.GetNearestTeam() == Team.Blue)
        {
            Debug.Log("�`�[��" + m_VicTeam.GetScore());
        }
    }

    /// <summary>
    /// �ϐ��Ȃǂ����Z�b�g����֐�
    /// </summary>
    public void ResetVar()
    {
        m_IsEnd = false;
        m_Calced = false;
        IsShowLog = false;
        DispScore.ResetVar();
        //TeamFlow�R���|�[�l���g�̎擾
        m_TeamFlow = this.gameObject.GetComponent<TeamFlowScript>();
        if(m_TeamFlow == null)
        {
            Debug.LogError("TeamFlow�R���|�[�l���g�̎擾�Ɏ��s���܂���");
        }
        m_TeamFlow.ResetVar();
        ballFlow.ResetVar();
    }
}
