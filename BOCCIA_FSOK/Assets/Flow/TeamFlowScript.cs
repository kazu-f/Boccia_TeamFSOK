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
    private bool IsThrow = false;
    private int m_Frame = 0;
    private void Awake()
    {
        m_BallFlow = GetComponent<BallFlowScript>();
        m_GameFlowScript = GetComponent<EndFlowScript>();
    }

    // Start is called before the first frame update
    void Start()
    {
        //���߂ɓ�����`�[��������
        m_NextTeam = m_firstTeam;
        //�{�[���̏������𑝂₷
        m_RemainBalls *= m_Remain;
        //���O�𗬂�
        NextTeamLog();
    }

    // Update is called once per frame
    void Update()
    {
        if (IsThrow)
        {
            //1�t���[���ڂ��Ƃ��܂������Ȃ��̂ŏ����x��������
            if (m_Frame < 10)
            {
                m_Frame++;
                return;
            }
            //�{�[���𓊂�����
            IsStopAllBalls();
            if (m_IsMoving == false)
            {
                //�S�Ẵ{�[�����~�܂��Ă���Ƃ�
                IsThrow = !CalucNextTeam();
            }
            m_Frame = 0;
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

        //ball�Ƃ����^�O�̂����Q�[���I�u�W�F�N�g��z��ɓ����
        GameObject[] m_balls;
        m_balls = GameObject.FindGameObjectsWithTag("Ball");
        if (m_balls.Length == 0)
        {
            if (m_RemainBalls.x == m_RemainBalls.y)
            {
                m_NextTeam = m_firstTeam;
            }
            else
            {
                ChangeNextTeam();
            }
            NextTeamLog();
            return true;
        }

        //��������͈�ȏ�̃{�[��������Ƃ��̏���
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
    /// �S�Ẵ{�[�����~�܂��Ă��邩�ǂ�������
    /// </summary>
    private void IsStopAllBalls()
    {
        //�W���b�N�{�[�����擾
        m_Jack = m_BallFlow.GetJackBall();
        //�W���b�N�{�[���̃X�e�[�g���擾
        m_JackState = m_Jack.GetComponent<BallStateScript>().GetState();
        if (m_JackState == BallState.Move)
        {
            //�W���b�N�{�[������~���Ă��Ȃ�
            m_IsMoving = true;
            return;
        }

        //�{�[�����擾
        GameObject[] m_balls;
        m_balls = GameObject.FindGameObjectsWithTag("Ball");
        //�{�[�����擾�ł���
        if (m_balls.Length != 0)
        {
            //��ȏ�{�[��������
            for (int num = 0; num < m_balls.Length; num++)
            {
                //�{�[���̏�Ԃ��擾
                if (m_balls[num].GetComponent<BallStateScript>().GetState() != BallState.Stop)
                {
                    //�~�܂��Ă��Ȃ��{�[��������
                    m_IsMoving = true;
                    return;
                }
            }
        }

        //�S�Ẵ{�[�����~�܂��Ă���
        m_IsMoving = false;
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
        IsStopAllBalls();
        return m_IsMoving;
    }

    ///// <summary>
    ///// �{�[���������Ă���t���O�𗧂Ă�
    ///// </summary>
    //public void SetMove(bool flag)
    //{
    //    m_IsMoving = flag;
    //}

    /// <summary>
    /// ���̃`�[����ύX
    /// </summary>
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

    public void ThrowBall()
    {
        IsThrow = true;
    }

    /// <summary>
    /// ��O�Ƀ{�[�����s�������̏���
    /// </summary>
    public void OutsideVenue()
    {

    }
}
