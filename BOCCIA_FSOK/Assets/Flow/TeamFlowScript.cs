using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamFlowScript : MonoBehaviour
{
    public Team m_NextTeam = Team.Num;
    private Team m_firstTeam = Team.Red;
    private BallFlowScript m_BallFlow = null;
    private GameObject m_Jack = null;
    private BallState m_JackState = BallState.Num;
    private Vector3 m_JackPos = Vector3.zero;
    private Vector2Int m_RemainBalls = Vector2Int.one;
    private int m_Remain = 6;
    private EndFlowScript m_GameFlowScript = null;
    private bool m_IsMoving = false;

    private void Awake()
    {
        m_BallFlow = GetComponent<BallFlowScript>();
        m_GameFlowScript = GetComponent<EndFlowScript>();
    }
    // Start is called before the first frame update
    void Start()
    {
        //���߂ɐԃ`�[����������
        m_NextTeam = m_firstTeam;
        m_RemainBalls *= m_Remain;
        //���O�𗬂�
        NextTeamLog();
    }

    // Update is called once per frame
    void Update()
    {
        if(m_IsMoving == true)
        {
            return;
        }
    }

    public Team GetNowTeam()
    {
        return m_NextTeam;
    }

    /// <summary>
    /// ���ɓ�����`�[�����v�Z����
    /// </summary>
    /// <returns>�߂�l�͌v�Z���ł������ǂ���</returns>
    public bool CalucNextTeam()
    {
        //�W���b�N�{�[�����擾
        m_Jack = m_BallFlow.GetJackBall();
        if (m_Jack == null)
        {
            //�C���X�^���X�̎擾�Ɏ��s�����Ƃ�
            Debug.Log("<color=red>�G���[�F�W���b�N�{�[���̎擾�Ɏ��s���܂���</color>");
            return false;
        }
        //�W���b�N�{�[���̃X�e�[�g���擾
        m_JackState = m_Jack.GetComponent<BallStateScript>().GetState();
        if (m_JackState == BallState.Move)
        {
            //�W���b�N�{�[�����܂������Ă���̂Ōv�Z�͎��s
            m_NextTeam = Team.Num;
            return false;
        }

        //ball�Ƃ����^�O�̂����Q�[���I�u�W�F�N�g��z��ɓ����
        GameObject[] m_balls;
        m_balls = GameObject.FindGameObjectsWithTag("Ball");
        if (m_balls.Length == 0)
        {
            m_NextTeam = m_firstTeam;
            m_IsMoving = false;
            NextTeamLog();
            return true;
        }
        else
        {
            for (int ballnum = 0; ballnum < m_balls.Length; ballnum++)
            {
                if (m_balls[ballnum].GetComponent<BallStateScript>().GetState() != BallState.Stop)
                {
                    //�~�܂�؂��Ă��Ȃ��������邽�ߎ��s
                    m_NextTeam = Team.Num;
                    return false;
                }
            }
        }
        //�S�Ă̋����~�܂��Ă����̂Ōv�Z�𑱂���
        m_IsMoving = false;
        int NearestBallNum = 0;
        float NearestDist = 10000;
        for(int ballnum = 0; ballnum < m_balls.Length;ballnum++)
        {
            //�{�[������W���b�N�{�[���ւ̋���
            Vector3 Dir = m_balls[ballnum].GetComponent<Rigidbody>().position - m_JackPos;
            float dist = Dir.magnitude;
            if(dist < NearestDist)
            {
                //����̃{�[���̕����W���b�N�{�[���ɋ߂��̂ŋ����Ɣԍ�����
                NearestDist = dist;
                NearestBallNum = ballnum;
            }
        }

        //���ɓ�����`�[����ύX
        if (m_balls[NearestBallNum].GetComponent<TeamDivisionScript>().GetTeam() == Team.Red)
        {
            m_NextTeam = Team.Blue;
        }
        else
        {
            m_NextTeam = Team.Red;
        }

        if(m_RemainBalls.x == 0 && m_RemainBalls.y == 0)
        {
            //���������I����Ă���̂ŃQ�[���G���h
            m_GameFlowScript.GameEnd();
        }
        else if(m_RemainBalls.x == 0)
        {
            //�ԃ`�[���������I����Ă���̂Ŏ��͐`�[��
            m_NextTeam = Team.Blue;
        }
        else if(m_RemainBalls.y == 0)
        {
            //�`�[���������I����Ă���̂Ŏ��͐ԃ`�[��
            m_NextTeam = Team.Red;
        }

        NextTeamLog();
        return true;
    }

    /// <summary>
    /// �W���b�N�{�[���̈ʒu��ۑ�
    /// </summary>
    /// <param name="pos">�ʒu</param>
    public void SetJackPos(Vector3 pos)
    {
        m_JackPos = pos;
    }

    /// <summary>
    /// �c��̋��������炷
    /// </summary>
    public void DecreaseBalls()
    {
        switch (m_NextTeam)
        {
            case Team.Red:
                //���̃`�[�����Ԃ̏ꍇx�����炷
                m_RemainBalls.x--;
                break;

            case Team.Blue:
                //���̃`�[�����̏ꍇy�����炷
                m_RemainBalls.y--;
                break;

            default:
                break;
        }
    }

    /// <summary>
    /// �{�[���������Ă��邩���擾
    /// </summary>
    /// <returns></returns>
    public bool GetIsMoving()
    {
        return m_IsMoving;
    }

    /// <summary>
    /// �{�[���������Ă���t���O�𗧂Ă�
    /// </summary>
    public void SetMove(bool flag)
    {
        m_IsMoving = flag;
    }

    public void ChangeNextTeam()
    {
        if(m_NextTeam == Team.Red)
        {
            m_NextTeam = Team.Blue;
        }
        else
        {
            m_NextTeam = Team.Red;
        }
        NextTeamLog();
    }

    /// <summary>
    /// ���̃`�[���̃��O
    /// </summary>
    public void NextTeamLog()
    {
        //�G���h���I����Ă��Ȃ��Ƃ�
        if (m_GameFlowScript.GetIsEnd() == false)
        {
            //���ɓ�����`�[���ɂ���ď�����ς���
            switch (m_NextTeam)
            {
                case Team.Red:
                    Debug.Log("���ɐԃ`�[���������܂�");
                    break;

                case Team.Blue:
                    Debug.Log("���ɐ`�[���������܂�");
                    break;

                case Team.Num:
                    Debug.Log("���Ƀ{�[���𓊂���`�[�������܂��Ă��܂���");
                    break;
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public void ResetVar()
    {
        //���߂ɐԃ`�[����������
        m_NextTeam = m_firstTeam;
        //�{�[���̐���������
        m_RemainBalls = Vector2Int.one;
        m_RemainBalls *= m_Remain;
        //���O�𗬂�
        NextTeamLog();
    }
    /// <summary>
    /// �����n�߂�`�[����ݒ肷��B
    /// </summary>
    public void SetFirstTeam(Team firstTeam)
    {
        m_firstTeam = firstTeam;
    }
}
