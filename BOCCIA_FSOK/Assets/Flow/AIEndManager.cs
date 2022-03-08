using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class AIEndManager : MonoBehaviour
{
    [SerializeField] private ChangeViewSwitchActive m_changeViewSwitch = null;
    [SerializeField] private TeamFlowScript m_TeamFlow = null;
    [SerializeField] private EndFlowScript m_EndFlow = null;
    [SerializeField] private TeamFlowDelayScript m_Delay = null;
    [SerializeField] private BallFlowScript m_BallFlow = null;
    [SerializeField] private TimerFillScript m_Timer = null;
    private Team MyTeamCol = Team.Num;
    private bool m_IsUseAI = false;
    private GameObject Failed = null;   //��O�̃I�u�W�F�N�g
    private void Awake()
    {

    }
    // Start is called before the first frame update
    void Start()
    {
        MyTeamCol = GameObject.Find("IsNetWorkObj").GetComponent<IsUseNetwork>().GetPlayerCol();
        m_IsUseAI = GameObject.Find("IsNetWorkObj").GetComponent<IsUseNetwork>().IsUseAI();
        if (MyTeamCol != Team.Red && MyTeamCol != Team.Blue)
        {
            Debug.LogError("�`�[���̃J���[���擾�ł��܂���ł����B�l�b�g���[�N�I�u�W�F������������Ă��Ȃ��\��������܂�");
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch (m_TeamFlow.GetState())
        {
            case TeamFlowState.Start:
                //�{�[���𓊂��n�߂�Ƃ�
                //��ԏ��߂Ȃ̂ŃW���b�N�v���[�Y�Əo��
                //�J������Ǐ]�J��������؂�ւ���
                GameObject.Find("GameCamera").GetComponent<GameCameraScript>().SetIfFollow(false);
                GameObject.Find("JackPlease").GetComponent<JackPleaseScript>().StartSlide();
                m_TeamFlow.SetState(TeamFlowState.Wait);
                //�^�C�}�[���X�^�[�g����
                Debug.Log("���߂Ƀ^�C�}�[���X�^�[�g���܂�");
                m_Timer.SyncStartTimer(true);
                break;

            case TeamFlowState.ThrowStart:
                if (m_BallFlow.IsPreparedJack() == true)
                {
                    Debug.Log("�e������Z�b�g���܂��B");
                    //�c��{�[�����̃e�L�X�g�̃����X�V
                    GameObject.Find("RemainBallText").GetComponent<RemainBallNumScript>().UpdateAlpha();
                    //���ɓ�����`�[�����Z�b�g
                    m_TeamFlow.SetNextTeamForClass();
                    //�c��{�[�����̃e�L�X�g���X�V
                    GameObject.Find("RemainBallText").GetComponent<RemainBallNumScript>().UpdateRemainText();
                }
                //�J������Ǐ]�J��������؂�ւ���
                GameObject.Find("GameCamera").GetComponent<GameCameraScript>().SetIfFollow(false);

                Debug.Log("�G���h���I����Ă��Ȃ��̂Ń^�C�}�[���X�^�[�g���܂�");
                m_Timer.SyncStartTimer(true);
                m_TeamFlow.SetState(TeamFlowState.Wait);
                break;

            case TeamFlowState.Wait:
                //�v���C���[��������܂�
                if (m_Timer.IsTimerStart() == false)
                {
                    //�^�C�}�[���܂��X�^�[�g���Ă��Ȃ�
                    Debug.Log("�^�C�}�[���܂��X�^�[�g���Ă��܂���B");
                    return;
                }

                if (m_Timer.IsTimeUp()/* || m_IsUseAI*/)
                {
                    m_changeViewSwitch.ResetFixedCamera();
                    //if (m_Timer.IsTimeUpForAI())
                    //{
                    Debug.Log("�^�C���A�b�v���Ă���̂ŃX�e�[�g��Culc�ɕύX");
                    if (m_BallFlow.IsPreparedJack())
                    {
                        //�W���b�N�{�[������������Ă���Ƃ�
                        //�v���C���[�̎����������炷
                        Debug.Log("�^�C���A�b�v�����̂Ń{�[�������炵�܂�");
                        m_TeamFlow.DecreaseBalls();
                    }
                    m_TeamFlow.SetState(TeamFlowState.Caluc);
                    //}
                }
                break;

            case TeamFlowState.Throw:
                //�{�[���𓊂�����
                //�^�C�}�[���~�߂�
                m_Timer.TimerStop();
                //�X�e�[�g��Move�ɂ���
                m_TeamFlow.SetState(TeamFlowState.Move);
                break;

            case TeamFlowState.Move:
                //�{�[�����܂������Ă���
                //�S�Ẵ{�[������~���Ă��邩���ׂ�
                if (m_TeamFlow.IsStopAllBalls())
                {
                    //�S�Ẵ{�[������~���Ă���Ƃ�
                    //�X�e�[�g���~�ɂ���
                    m_TeamFlow.SetState(TeamFlowState.Stop);
                }
                break;

            case TeamFlowState.Stop:
                //�S�Ẵ{�[������~���Ă���
                //�x�����J�n������
                m_Delay.DelayStart();
                m_TeamFlow.SetState(TeamFlowState.Delay);
                break;

            case TeamFlowState.Delay:
                //�x����                
                if (!m_Delay.IsDelay())
                {
                    Failed = GameObject.Find("Failed");
                    Failed.GetComponent<FailedMoveScript>().FontAlphaZero();
                    //�x�����I������
                    m_TeamFlow.SetState(TeamFlowState.Caluc);
                    m_TeamFlow.ThrowEnd();
                }
                break;

            case TeamFlowState.Caluc:
                Debug.Log("�v�Z�J�n");
                //�������`�[����AI��̎��݂̂��v�Z������
                //���ɓ�����{�[�����v�Z����
                if (m_BallFlow.IsPreparedJack() == false)
                {
                    //�W���b�N�{�[�����p�ӂ���Ă��Ȃ��̂ŃW���b�N�{�[���𓊂���̂��~�X���Ă���
                    //�W���b�N�{�[���𓊂���`�[����ύX
                    m_TeamFlow.ChangeJackThrowTeam();
                    //�v�Z���ł��Ȃ������̂ŃX�e�[�g��ThrowEnd�ɂ���
                    //m_TeamFlow.SetState(TeamFlowState.ThrowEnd);
                }
                else
                {
                    //�W���b�N�{�[���͗p�ӂ���Ă���̂Ŏ��ɓ�����`�[�����v�Z
                    m_TeamFlow.CalucNextTeam();

                }
                //�������`�[���͎��ɓ�����`�[�����v�Z�ŗ����̂�SendInfo�Ɉڍs����
                m_TeamFlow.SetState(TeamFlowState.SyncEnd);
                break;

            case TeamFlowState.SyncEnd:

                bool endflag = true;
                //�ǂ���Ƃ������I���Ă���Ƃ��G���h�I���Ɉڍs����
                foreach (int i in m_TeamFlow.GetRemainBalls())
                {
                    if (i != 0)
                    {
                        //�܂���������̂Ŕ�����
                        Debug.Log("�܂���������̂ŃQ�[���𑱂��܂��B");
                        endflag = false;
                        break;
                    }
                }
                if (endflag == true)
                {
                    //�����I���Ă���̂ŃX�e�[�g��End�ɂ���
                    Debug.Log("�����{�[�����Ȃ���ŃG���h���I�����܂�");
                    m_TeamFlow.SetState(TeamFlowState.End);
                    break;
                }

                //�t���O���Z�b�g�����������̂��m�F�����̂�
                //ThrowStart�ɃX�e�[�g���Z�b�g����
                m_TeamFlow.SetState(TeamFlowState.ThrowStart);

                break;

            case TeamFlowState.End:
                //�G���h���I�������
                //�G���h���I������t���O�𗧂Ă�
                m_EndFlow.GameEnd();
                break;

            default:
                return;
        }
    }
}
