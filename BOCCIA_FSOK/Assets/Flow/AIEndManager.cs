using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class AIEndManager : MonoBehaviour
{
    [SerializeField] private TeamFlowScript m_TeamFlow = null;
    [SerializeField] private EndFlowScript m_EndFlow = null;
    [SerializeField] private TeamFlowDelayScript m_Delay = null;
    [SerializeField] private BallFlowScript m_BallFlow = null;
    [SerializeField] private TimerFillScript m_Timer = null;
    private Team MyTeamCol = Team.Num;
    
    //private bool SyncFlag = false;      //�S���������ŗ������ɗ��Ă�t���O
    private bool m_IsUseAI = false;
    private GameObject Failed = null;   //��O�̃I�u�W�F�N�g
    int m_turnNo = 0;
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
                Debug.Log("TeamFlowState.Start");
                GameObject.Find("GameCamera").GetComponent<GameCameraScript>().SetIfFollow(false);
                //�{�[���𓊂��n�߂�Ƃ�
                //��ԏ��߂Ȃ̂ŃW���b�N�v���[�Y�Əo��
                GameObject.Find("JackPlease").GetComponent<JackPleaseScript>().StartSlide();
                m_TeamFlow.SetState(TeamFlowState.Wait);
                //�^�C�}�[���X�^�[�g����
             //   Debug.Log("���߂Ƀ^�C�}�[���X�^�[�g���܂�");
                m_Timer.SyncStartTimer();
                break;

            case TeamFlowState.ThrowStart:

                Debug.Log("TeamFlowState.ThrowStart");
                if (m_BallFlow.IsPreparedJack() == true)
                {
                   // Debug.Log("�e������Z�b�g���܂��B");
                    //�c��{�[�����̃e�L�X�g�̃����X�V
                    GameObject.Find("RemainBallText").GetComponent<RemainBallNumScript>().UpdateAlpha();
                    //�J������Ǐ]�J��������؂�ւ���
                    GameObject.Find("GameCamera").GetComponent<GameCameraScript>().SetIfFollow(false);
                    //���ɓ�����`�[�����Z�b�g
                    m_TeamFlow.SetNextTeamForClass();
                    //�c��{�[�����̃e�L�X�g���X�V
                    GameObject.Find("RemainBallText").GetComponent<RemainBallNumScript>().UpdateRemainText();
                }


                //Wait�Ɉڍs
                //�����n�߂�B
                //Debug.Log("�G���h���I����Ă��Ȃ��̂Ń^�C�}�[���X�^�[�g���܂�");
                m_Timer.SyncStartTimer();
                m_TeamFlow.SetState(TeamFlowState.Wait);
                
                break;

            case TeamFlowState.Wait:
                Debug.Log("TeamFlowState.Wait");

                //�v���C���[��������܂�
                if (m_Timer.IsTimerStart() == false)
                {
                    //�^�C�}�[���܂��X�^�[�g���Ă��Ȃ�
                    //Debug.Log("�^�C�}�[���܂��X�^�[�g���Ă��܂���B");
                    return;
                }

                if (m_Timer.IsTimeUp()/* || m_IsUseAI*/)
                {
                    //Debug.Log("�^�C���A�b�v���Ă���̂ŃX�e�[�g��Culc�ɕύX");
                    if (m_BallFlow.IsPreparedJack())
                    {
                        //�W���b�N�{�[������������Ă���Ƃ�
                        //�v���C���[�̎����������炷
                        //Debug.Log("�^�C���A�b�v�����̂Ń{�[�������炵�܂�");
                        m_TeamFlow.DecreaseBalls();
                    }
                    m_TeamFlow.SetState(TeamFlowState.Caluc);
                }
                break;

            case TeamFlowState.Throw:
                Debug.Log("TeamFlowState.Throw");
                //�{�[���𓊂�����
                //�^�C�}�[���~�߂�
                m_Timer.TimerStop();
                //�X�e�[�g��Move�ɂ���
                m_TeamFlow.SetState(TeamFlowState.Move);
                break;

            case TeamFlowState.Move:
                Debug.Log("TeamFlowState.Move");
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
                Debug.Log("TeamFlowState.Stop");
                //�S�Ẵ{�[������~���Ă���
                //�x�����J�n������
                m_Delay.DelayStart();
                m_TeamFlow.SetState(TeamFlowState.Delay);
                break;

            case TeamFlowState.Delay:
                Debug.Log("TeamFlowState.Delay");
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
                // ���̃^�[���ցB
                m_turnNo++;
                Debug.Log("TeamFlowState.Caluc");
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
                    //�c��{�[�����̃e�L�X�g�̃����X�V
                    //GameObject.Find("RemainBallText").GetComponent<RemainBallNumScript>().UpdateAlpha();
                    //�W���b�N�{�[���͗p�ӂ���Ă���̂Ŏ��ɓ�����`�[�����v�Z
                    m_TeamFlow.CalucNextTeam();

                }

                //�������`�[���͓����f�[�^���M�Ɉڍs
                m_TeamFlow.SetState(TeamFlowState.SyncEnd);
                break;

            case TeamFlowState.SyncEnd:
                Debug.Log("TeamFlowState.SyncEnd");

                bool endflag = true;
                //�ǂ���Ƃ������I���Ă���Ƃ��G���h�I���Ɉڍs����
                foreach (int i in m_TeamFlow.GetRemainBalls())
                {
                    if (i != 0)
                    {
                        //�܂���������̂Ŕ�����
                        //Debug.Log("�܂���������̂ŃQ�[���𑱂��܂��B");
                        endflag = false;
                        break;
                    }
                }
               
                if (endflag == true)
                {
                    //�����I���Ă���̂ŃX�e�[�g��End�ɂ���
                    //Debug.Log("�����{�[�����Ȃ���ŃG���h���I�����܂�");
                    m_TeamFlow.SetState(TeamFlowState.End);
                    break;
                }

                //ThrowStart�ɃX�e�[�g���Z�b�g����
                if (m_BallFlow.IsPreparedJack() == false)
                {
                    m_TeamFlow.SetState(TeamFlowState.Start);
                }
                else
                {
                    m_TeamFlow.SetState(TeamFlowState.ThrowStart);
                }

                break;

            case TeamFlowState.End:
                Debug.Log("TeamFlowState.End");
                //�G���h���I�������
                //�G���h���I������t���O�𗧂Ă�
                m_EndFlow.GameEnd();
                break;

            default:
                return;
        }
    }
}
