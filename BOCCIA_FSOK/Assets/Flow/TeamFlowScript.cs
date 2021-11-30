using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TeamFlowState
{
    Start,
    ThrowStart,
    Wait,
    Throw,
    Move,
    Stop,
    Delay,
    Caluc,
    //Caluced,
    //ThrowEnd,
    SendInfo,
    Sync,
    SyncWait,
    //ChangeEnd,
    End,
    Num,
}
public class TeamFlowScript : MonoBehaviour
{
    public TeamFlowState m_state;
    public Team m_NextTeam = Team.Num;
    private Team m_firstTeam = Team.Red;
    private BallFlowScript m_BallFlow = null;
    private GameObject m_Jack = null;
    private BallState m_JackState = BallState.Num;
    private Vector3 m_JackPos = Vector3.zero;
    private int[] m_RemainBalls = new int[2];
    public int m_Remain { get; private set; } = 6;
    private EndFlowScript m_GameFlowScript = null;
    private bool m_IsMoving = false;
    private bool IsThrow = false;
    private int m_Frame = 0;
    private GameObject m_NextBallImage = null;

    private void Awake()
    {
        m_BallFlow = GetComponent<BallFlowScript>();
        m_GameFlowScript = GetComponent<EndFlowScript>();
        m_NextBallImage = GameObject.Find("NextBallImage");
        if(m_NextBallImage == null)
        {
            Debug.LogError("NextBallImage�̎擾�Ɏ��s���܂���");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        m_state = TeamFlowState.Start;
        //���߂ɓ�����`�[��������
        m_NextTeam = m_firstTeam;
        //�{�[���̏������𑝂₷
        m_RemainBalls[0] = 1 * m_Remain;
        m_RemainBalls[1] = 1 * m_Remain;

        ////��ԏ��߂Ȃ̂ŃW���b�N�v���[�Y�Əo��
        //GameObject.Find("JackPlease").GetComponent<JackPleaseScript>().StartSlide();
        //���O�𗬂�
        NextTeamLog();
    }

    // Update is called once per frame
    void Update()
    {
        //if (IsThrow)
        //{
        //    //�X�e�[�g�𓊂�����Ԃɂ���
        //    m_state = TeamFlowState.Throw;
        //    ////1�t���[���ڂ��Ƃ��܂������Ȃ��̂ŏ����x��������
        //    //if (m_Frame < 10)
        //    //{
        //    //    m_Frame++;
        //    //    return;
        //    //}

        //    ////�{�[���𓊂�����
        //    //IsStopAllBalls();
        //    if (m_IsMoving == false)
        //    {
        //        //if (m_BallFlow.IsPreparedJack() == false)
        //        //{
        //        //    ChangeJackThrowTeam();
        //        //    return;
        //        //}
        //        //�S�Ẵ{�[�����~�܂��Ă���Ƃ�
        //        //IsThrow = !CalucNextTeam();
        //        //if (CalucNextTeam())
        //        //{
        //        //    //���ɓ�����`�[���̃{�[���̃X�v���C�g��ύX
        //        //    m_NextBallImage.GetComponent<ChangeBallSprite>().ChangeSprite(m_NextTeam);
        //        //    //�G���h���܂��I����Ă��Ȃ��Ƃ�
        //        //    if (m_GameFlowScript.GetIsEnd() == false)
        //        //    {
        //        //        //�p�h���X�N���v�g���擾
        //        //        PaddleScript paddle = GameObject.Find("Paddle").GetComponent<PaddleScript>();
        //        //        //�p�h���̃X�v���C�g��ύX
        //        //        paddle.SetTeam(m_NextTeam);
        //        //        paddle.PaddleStart();
        //        //    }
        //        //    //�����I���
        //        //    IsThrow = false;
        //        //    //�J�����ύX
        //        //    GameObject.Find("GameCamera").GetComponent<GameCameraScript>().SwitchCamera();
        //        //    m_Frame = 0;

        //        //}
        //    }
        //}
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
            if (m_RemainBalls[0] == 0 && m_RemainBalls[1] == 0)
            {
                //�{�[�����Ȃ��Ȃ����̂Ŏ��ɓ�����`�[�����v�Z���Ȃ�
                return false;
            }

            if (m_RemainBalls[0] == m_RemainBalls[1])
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

        for (int ballnum = 0; ballnum < m_balls.Length;ballnum++)
        {
            //�`�[�����擾
            Teams[ballnum] = m_balls[ballnum].GetComponent<TeamDivisionScript>().GetTeam();
            //�{�[������W���b�N�{�[���ւ̋���
            m_JackPos = m_Jack.transform.position;
            Vector3 Dir = m_balls[ballnum].GetComponent<Rigidbody>().position - m_JackPos;
            float dist = Dir.magnitude;
            //�W���b�N�{�[���܂ł̋�������
            m_BallsDist[ballnum] = dist;

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
            m_NextTeam = Team.Blue;
        }
        else
        {
            m_NextTeam = Team.Red;
        }

        float SecondNearDist = Mathf.Max(NearDist[0], NearDist[1]);
        for (int ballnum = 0; ballnum < m_balls.Length; ballnum++)
        {
            m_balls[ballnum].gameObject.layer = 9;
            if (m_BallsDist[ballnum] < SecondNearDist)
            {
                //�����Е��̈�ԋ߂��{�[�������߂��ʒu�ɂ���̂Ń��C���[��ς���
                m_balls[ballnum].gameObject.layer = 0;
            }
        }

        if (m_RemainBalls[0] == 0 && m_RemainBalls[1] == 0)
        {
            //���������I����Ă���̂ŃQ�[���G���h
            m_GameFlowScript.GameEnd();
            m_NextBallImage.GetComponent<ChangeBallSprite>().GameEnd();
        }
        else if(m_RemainBalls[0] == 0)
        {
            //�ԃ`�[���������I����Ă���̂Ŏ��͐`�[��
            m_NextTeam = Team.Blue;
        }
        else if(m_RemainBalls[1] == 0)
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
    public bool IsStopAllBalls()
    {
        //�W���b�N�{�[�����擾
        m_Jack = m_BallFlow.GetJackBall();
        //�W���b�N�{�[���̃X�e�[�g���擾
        m_JackState = m_Jack.GetComponent<BallStateScript>().GetState();
        switch (m_JackState)
        {
            case BallState.Move:
                //�W���b�N�{�[������~���Ă��Ȃ�
                m_IsMoving = true;
                return !m_IsMoving;
            case BallState.Stop:
                //m_Jack.GetComponent<BallOperateScript>().EndThrowing();
                break;
            default:
                //�W���b�N�{�[������~���Ă��Ȃ�
                m_IsMoving = true;
                return !m_IsMoving;
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
                if (m_balls[num].GetComponent<BallStateScript>().GetState() == BallState.Stop)
                {
                    //m_balls[num].GetComponent<BallOperateScript>().EndThrowing();
                }
                else
                {
                    //�~�܂��Ă��Ȃ��{�[��������
                    m_IsMoving = true;
                    return !m_IsMoving;
                }
            }
        }

        //�S�Ẵ{�[�����~�܂��Ă���
        m_IsMoving = false;
        m_state = TeamFlowState.Stop;

        return !m_IsMoving;
    }

    /// <summary>
    /// �{�[���𓊂��I��鏈���𑖂点��
    /// </summary>
    public void ThrowEnd()
    {
        //�����I���
        m_Jack.GetComponent<BallOperateScript>().EndThrowing();
        //�{�[�����擾
        GameObject[] m_balls;
        m_balls = GameObject.FindGameObjectsWithTag("Ball");
        //�{�[�����擾�ł���
        if (m_balls.Length != 0)
        {
            for (int num = 0; num < m_balls.Length; num++)
            {
                m_balls[num].GetComponent<BallOperateScript>().EndThrowing();
            }
        }
    }

    /// <summary>
    /// �c��̋��������炷
    /// </summary>
    public void DecreaseBalls()
    {
        //�{�[�������炷
        Debug.Log("�{�[�������炵�܂�");
        switch (m_NextTeam)
        {
            case Team.Red:
                //���̃`�[�����Ԃ̏ꍇx�����炷
                m_RemainBalls[0]--;
                break;

            case Team.Blue:
                //���̃`�[�����̏ꍇy�����炷
                m_RemainBalls[1]--;
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

    public void ChangeFirstTeam()
    {
        if (m_firstTeam == Team.Red)
        {
            m_firstTeam = Team.Blue;
        }
        else
        {
            m_firstTeam = Team.Red;
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
                    Debug.Log("�c��" + m_RemainBalls[0] + "���ł�");
                    break;

                case Team.Blue:
                    Debug.Log("���ɐ`�[���������܂�");
                    Debug.Log("�c��" + m_RemainBalls[1] + "���ł�");
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
        m_IsMoving = false;
        m_NextBallImage.GetComponent<ChangeBallSprite>().ResetVar();
        //�{�[���̐���������
        m_RemainBalls[0] = 1 * m_Remain;
        m_RemainBalls[1] = 1 * m_Remain;
        m_state = TeamFlowState.Start;
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
        //�c��{�[�����̃e�L�X�g���X�V
        GameObject.Find("RemainBallText").GetComponent<RemainBallNumScript>().UpdateRemainText();
        IsThrow = true;
        //�X�e�[�g�𓊂�����Ԃɂ���
        m_state = TeamFlowState.Throw;
    }

    /// <summary>
    /// �W���b�N�{�[���𓊂���`�[����ύX����
    /// </summary>
    public void ChangeJackThrowTeam()
    {
        //�W���b�N�{�[���𓊂���`�[�����ς�����̂ŃW���b�N�v���[�Y�Əo��
        GameObject.Find("JackPlease").GetComponent<JackPleaseScript>().StartSlide();
        //�W���b�N�{�[������������Ă��Ȃ��Ƃ�
        ChangeNextTeam();
        ChangeFirstTeam();
        //�����I���
        IsThrow = false;
        //�J�����ύX
        GameObject.Find("GameCamera").GetComponent<GameCameraScript>().SetIfFollow(false);
        m_Frame = 0;
        m_IsMoving = false;
        NextTeamLog();
    }

    /// <summary>
    /// ���ɓ�����`�[���̎c��̋���
    /// </summary>
    /// <returns>�c��̋���</returns>
    public int GetRemainBall()
    {
        if(m_NextTeam == Team.Red)
        {
            return m_RemainBalls[0];
        }
        else
        {
            return m_RemainBalls[1];
        }
    }

    public int[] GetRemainBalls()
    {
        return m_RemainBalls;
    }
    /// <summary>
    /// �l�b�g���[�N�����p
    /// </summary>
    /// <param name="balls">�c��̃{�[����</param>
    public void SetRemainBalls(int[] balls)
    {
        m_RemainBalls = balls;
    }

    public TeamFlowState GetState()
    {
        return m_state;
    }

    public void SetState(TeamFlowState state)
    {
        m_state = state;
    }

    /// <summary>
    /// ���̃`�[�����Z�b�g����
    /// </summary>
    /// <param name="team">���̃`�[��</param>
    public void SetNextTeam(Team team)
    {
        m_NextTeam = team;
    }
    /// <summary>
    /// ���ɓ�����`�[������񂪕K�v�ȃN���X�ɃZ�b�g����
    /// </summary>
    public void SetNextTeamForClass()
    {
        //�G���h���܂��I����Ă��Ȃ��Ƃ�
        if (m_GameFlowScript.GetIsEnd() == false)
        {
            //���ɓ�����`�[���̃{�[���̃X�v���C�g��ύX
            m_NextBallImage.GetComponent<ChangeBallSprite>().ChangeSprite(m_NextTeam);
            //�p�h���X�N���v�g���擾
            PaddleScript paddle = GameObject.Find("Paddle").GetComponent<PaddleScript>();
            //�p�h���̃X�v���C�g��ύX
            paddle.SetTeam(m_NextTeam);
            //�p�h���̑�����n�߂�
            paddle.PaddleStart();
        }

    }
}
