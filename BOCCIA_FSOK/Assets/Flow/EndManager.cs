using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class EndManager : MonoBehaviour
{
    [SerializeField] private TeamFlowScript m_TeamFlow = null;
    [SerializeField] private EndFlowScript m_EndFlow = null;
    [SerializeField] private TeamFlowDelayScript m_Delay = null;
    [SerializeField] private BallFlowScript m_BallFlow = null;
    [SerializeField] private TimerFillScript m_Timer = null;
    private Team MyTeamCol = Team.Num;
    private bool[] SyncFlags = new bool[2];     //�S���p�̓����t���O
    //private bool SyncFlag = false;      //�S���������ŗ������ɗ��Ă�t���O
    private bool m_IsUseAI = false;
    [SerializeField] private NetworkSendManagerScript m_SendManager = null;
    private bool Client = false;
    private GameObject Failed = null;   //��O�̃I�u�W�F�N�g
    private float Limit = 5.0f;
    private float time = 0.0f;
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
                Client = false;
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

                //�����t���O��false�ɂ���
                SyncFlags[0] = false;
                SyncFlags[1] = false;
                //Wait�Ɉڍs
                if (m_SendManager.ResetSyncFlag())
                {
                    //Debug.Log("�����t���O�����Z�b�g�ł��܂���");
                    //�����n�߂�B
                    //Debug.Log("�G���h���I����Ă��Ȃ��̂Ń^�C�}�[���X�^�[�g���܂�");
                    m_Timer.SyncStartTimer();
                    m_TeamFlow.SetState(TeamFlowState.Wait);
                }
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
                    //if (m_Timer.IsTimeUpForAI())
                    //{
                        //Debug.Log("�^�C���A�b�v���Ă���̂ŃX�e�[�g��Culc�ɕύX");
                        if (m_BallFlow.IsPreparedJack())
                        {
                            //�W���b�N�{�[������������Ă���Ƃ�
                            //�v���C���[�̎����������炷
                            //Debug.Log("�^�C���A�b�v�����̂Ń{�[�������炵�܂�");
                            m_TeamFlow.DecreaseBalls();
                        }
                        m_TeamFlow.SetState(TeamFlowState.Caluc);
                    //}
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
                Debug.Log("TeamFlowState.Caluc");
                //Debug.Log("�v�Z�J�n");
                if (m_TeamFlow.GetNowTeam() == MyTeamCol|| m_IsUseAI)
                {
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
                        //if (m_TeamFlow.CalucNextTeam() == true)
                        //{
                        ////�v�Z���ł����̂ŃX�e�[�g��Caluced�ɂ���
                        //m_TeamFlow.SetState(TeamFlowState.Caluced);
                        //}
                    }
                    //�������`�[���͎��ɓ�����`�[�����v�Z�ŗ����̂�SendInfo�Ɉڍs����
                    m_TeamFlow.SetState(TeamFlowState.SendInfo);
                    break;
                }
                //�����Ă��Ȃ��`�[����Sync�Ɉڍs
                m_TeamFlow.SetState(TeamFlowState.Sync);
                break;

            //case TeamFlowState.Caluced:
            //    //���ɓ�����{�[�����v�Z�ł�����
            //    //���ɓ�����`�[�����Z�b�g
            //    //m_TeamFlow.SetNextTeamForClass();
            //    //m_TeamFlow.SetState(TeamFlowState.ThrowEnd);
            //    break;

            case TeamFlowState.SendInfo:
                Debug.Log("TeamFlowState.SendInfo");
                //Debug.Log("���𑗂�܂��B");
                //���𑗂鑤�Ȃ̂�0�Ԗڂ�true
                SyncFlags[0] = true;

                //���𑗂�
                m_SendManager.SendRemainBalls(m_TeamFlow.GetRemainBalls());
                m_SendManager.SendNextTeam((int)m_TeamFlow.GetNowTeam());
                m_SendManager.SendFirstTeam((int)m_TeamFlow.GetFirstTeam());

                //m_SendManager.SendSyncFlag(SyncFlags);
                m_SendManager.SendMasterSyncFlag(SyncFlags[0]);

                //�����X�e�[�g�Ɉڍs
                m_TeamFlow.SetState(TeamFlowState.SyncWait);
                time = 0.0f;
                break;

            case TeamFlowState.Sync:
                Debug.Log("TeamFlowState.Sync");
                Client = true;

                //if (SyncFlag == false)
                //{
                //SyncFlags = m_SendManager.ReceiveSyncFlag();
                SyncFlags[0] = m_SendManager.ReceiveMasterSyncFlag();
                //�܂��������Ƃ�Ă��Ȃ��Ƃ�
                //�܂��O�ɓ������l�̏�񂪑����Ă��邩�ǂ����𒲂ׂ�
                if (SyncFlags[0] == false)
                {
                    //�܂������Ă��Ȃ��B
                    break;
                }
                //////////////////////////////////////////////////////////
                ////////////�������牺�͏�񂪑����Ă���Ƃ�////////////
                //////////////////////////////////////////////////////////
                
                //Debug.Log("�����J�n");

                //���������ɓ�����Ƃ��O�̏��𓯊�������B
                //������������
                //�o�c��̃{�[���̌��A���ɓ�����`�[���A�p
                m_TeamFlow.SetRemainBalls(m_SendManager.ReceiveRemainBalls());
                m_TeamFlow.SetNextTeam(m_SendManager.ReceiveNextTeam());
                m_TeamFlow.SetFirstTeam(m_SendManager.ReceiveFirstTeam());
                if (m_BallFlow.IsPreparedJack() == false)
                {
                    GameObject.Find("JackPlease").GetComponent<JackPleaseScript>().StartSlide();
                }
                GameObject.Find("GameCamera").GetComponent<GameCameraScript>().SetIfFollow(false);
                //���������̂Ńt���O���Z�b�g����
                SyncFlags[1] = true;

                //�������I��������Ƃ�m�点��B
                //m_SendManager.SendSyncFlag(SyncFlags);
                m_SendManager.SendClientSyncFlag(SyncFlags[1]);

                //�������I������̂�SyncWait�X�e�[�g�Ɉڍs����
                //Debug.Log("SyncWait�X�e�[�g�Ɉڍs");
                m_TeamFlow.SetState(TeamFlowState.SyncEnd);

                //    SyncFlag = true;
                //}

                break;

            case TeamFlowState.SyncWait:
                Debug.Log("TeamFlowState.SyncWait");
                //Debug.Log("SyncWait�ɂ͂���܂���");
                time += Time.deltaTime;
                if(time > Limit)
                {
                    //Debug.Log("�p�P���X�΍�");
                    //���𑗂�
                    m_SendManager.SendRemainBalls(m_TeamFlow.GetRemainBalls());
                    m_SendManager.SendNextTeam((int)m_TeamFlow.GetNowTeam());
                    m_SendManager.SendFirstTeam((int)m_TeamFlow.GetFirstTeam());

                    //m_SendManager.SendSyncFlag(SyncFlags);
                    m_SendManager.SendMasterSyncFlag(SyncFlags[0]);
                    time = 0.0f;
                }
                //�����������ǂ������ׂ�
                //SyncFlags = m_SendManager.ReceiveSyncFlag();
                if (Client)
                {
                    SyncFlags[0] = m_SendManager.ReceiveMasterSyncFlag();
                }
                else
                {
                    SyncFlags[1] = m_SendManager.ReceiveClientSyncFlag();
                }

                if (m_IsUseAI == false)
                {
                    int i = 0;
                    //AI�킶�ᖳ���Ƃ�
                    foreach (bool flag in SyncFlags)
                    {
                        i++;
                        if (flag == false)
                        {
                            if(i == 1)
                            {
                                //Debug.Log("�܂����[�̂���");
                            }
                            else if( i == 2)
                            {
                                //Debug.Log("���炢����Ƃ̂���");
                            }
                            //�܂��������I����Ă��Ȃ�
                            //Debug.Log("�܂��������I����Ă��܂���");
                            return;
                        }
                    }
                }

                //////////////////////////////////////////////////////////
                /////////�������牺�͓������I����Ă���Ƃ��̏���/////////
                //////////////////////////////////////////////////////////


                //////////////////////////////////////////////////////////
                //////////�������牺�͂܂������I����Ă��Ȃ��Ƃ�//////////
                //////////////////////////////////////////////////////////

                //SyncEnd�ɃX�e�[�g���Z�b�g����
                m_TeamFlow.SetState(TeamFlowState.SyncEnd);

                break;

            case TeamFlowState.SyncEnd:
                Debug.Log("TeamFlowState.SyncEnd");
                ////�������������Ă���t���O�����Z�b�g����
                //if (Client == false)
                //{
                //    if (m_IsUseAI == false)
                //    {
                //        if (m_SendManager.ReceiveClientSyncFlag())
                //        {
                //            //�N���C�A���g�̓������I����Ă���
                //            Debug.Log("�z�X�g����I");
                //            Debug.Log("�����t���O��������I");
                //            SyncFlags[0] = false;
                //            SyncFlags[1] = false;
                //            m_SendManager.SendMasterSyncFlag(SyncFlags[0]);
                //            m_SendManager.SendClientSyncFlag(SyncFlags[1]);
                //        }
                //        else
                //        {
                //            //�N���C�A���g�̓������I����Ă��Ȃ�
                //            return;
                //        }
                //    }
                //}
                //SyncFlags[0] = m_SendManager.ReceiveMasterSyncFlag();
                //SyncFlags[1] = m_SendManager.ReceiveClientSyncFlag();

                //if (m_IsUseAI == false)
                //{
                //    //AI�킶�ᖳ���Ƃ�
                //    foreach (bool flag in SyncFlags)
                //    {
                //        if (flag == true)
                //        {
                //            //�܂��������I����Ă��Ȃ�
                //            Debug.Log("�܂��������I����Ă��܂���");
                //            return;
                //        }
                //    }
                //}

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

                //�t���O���Z�b�g�����������̂��m�F�����̂�
                //ThrowStart�ɃX�e�[�g���Z�b�g����
                m_TeamFlow.SetState(TeamFlowState.ThrowStart);

                break;

            //case TeamFlowState.ThrowEnd:
            //    //�����I���
            //    //�J������Ǐ]�J��������؂�ւ���
            //    GameObject.Find("GameCamera").GetComponent<GameCameraScript>().SetIfFollow(false);
            //    m_TeamFlow.SetState(TeamFlowState.ChangeEnd);
            //    break;

            //case TeamFlowState.ChangeEnd:
            //    //�`�[���ς��I�����
            //    //�c��{�[�����̃e�L�X�g���X�V
            //    GameObject.Find("RemainBallText").GetComponent<RemainBallNumScript>().UpdateRemainText();

            //    break;

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
