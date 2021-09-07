using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamFlowScript : MonoBehaviour
{
    public Team m_NextTeam = Team.Num;
    private BallFlowScript m_BallFlow = null;
    private GameObject m_Jack = null;
    private BallState m_JackState = BallState.Num;
    private Vector3 m_JackPos = Vector3.zero;
    private Vector2Int m_RemainBalls = Vector2Int.one;
    private int m_Remain = 6;
    private GameFlowScript m_GameFlowScript = null;
    // Start is called before the first frame update
    void Start()
    {
        m_NextTeam = Team.Jack;
        m_BallFlow = GetComponent<BallFlowScript>();
        m_RemainBalls *= m_Remain;
        m_GameFlowScript = GetComponent<GameFlowScript>();
    }

    // Update is called once per frame
    void Update()
    {
        //���ɓ�����`�[���ɂ���ď�����ς���
        switch(m_NextTeam)
        {
            //�W���b�N�{�[���𓊂���Ƃ�
            case Team.Jack:
                Debug.Log("���߂ɃW���b�N�{�[���𓊂��܂�");
                if (m_BallFlow.IsPreparedJack())
                {
                    //�W���b�N�{�[�����擾
                    m_Jack = GameObject.Find("JackBall");
                    if (m_Jack == null)
                    {
                        //�C���X�^���X�̎擾�Ɏ��s�����Ƃ�
                        Debug.Log("<color=red>�G���[�F�W���b�N�{�[���̎擾�Ɏ��s���܂���</color>");
                    }
                    else
                    {
                        //�W���b�N�{�[�����������ꂽ�玟�͐ԃ`�[����������
                        m_NextTeam = Team.Red;
                    }
                }
                break;

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
        if(m_JackState == BallState.Move)
        {
            //�W���b�N�{�[�����܂������Ă���̂Ōv�Z�͎��s
            m_NextTeam = Team.Num;
            return false;
        }
        //ball�Ƃ����^�O�̂����Q�[���I�u�W�F�N�g��z��ɓ����
        GameObject[] m_balls;
        m_balls = GameObject.FindGameObjectsWithTag("Ball");
        for(int ballnum = 0; ballnum < m_balls.Length; ballnum++)
        {
            if(m_balls[ballnum].GetComponent<BallStateScript>().GetState() != BallState.Stop)
            {
                //�~�܂�؂��Ă��Ȃ��������邽�ߎ��s
                m_NextTeam = Team.Num;
                return false;
            }
        }
        //�S�Ă̋����~�܂��Ă����̂Ōv�Z�𑱂���
        int NearestBallNum = 0;
        float NearestDist = 10000;
        for(int ballnum = 0; ballnum < m_balls.Length;ballnum++)
        {
            //�{�[������W���b�N�{�[���ւ̋���
            Vector3 Dir = m_balls[ballnum].GetComponent<Rigidbody>().position - m_JackPos;
            float dist = Dir.magnitude;
            if(NearestDist > dist)
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

        return true;
    }

    /// <summary>
    /// �W���b�N�{�[���̏�Ԃ�ۑ�
    /// </summary>
    /// <param name="state">���</param>
    public void SetJackState(BallState state)
    {
        m_JackState = state;
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
}
