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
    private Team MyTeamCol = Team.Num;

    private void Awake()
    {

    }
    // Start is called before the first frame update
    void Start()
    {
        MyTeamCol = GameObject.Find("IsNetWorkObj").GetComponent<IsUseNetwork>().GetPlayerCol();
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
                GameObject.Find("JackPlease").GetComponent<JackPleaseScript>().StartSlide();
                m_TeamFlow.SetState(TeamFlowState.Wait);
                //�^�C�}�[���X�^�[�g����
                GameObject.Find("Timer").GetComponent<TimerFillScript>().TimerStart();
                break;

            case TeamFlowState.Wait:
                //�v���C���[��������܂�
                if(GameObject.Find("Timer").GetComponent<TimerFillScript>().IsTimeUp())
                {
                    if(m_BallFlow.IsPreparedJack())
                    {
                        //�W���b�N�{�[������������Ă���Ƃ�
                        //�v���C���[�̎����������炷
                        m_TeamFlow.DecreaseBalls();
                    }
                    m_TeamFlow.SetState(TeamFlowState.Caluc);
                }
                break;

            case TeamFlowState.Throw:
                //�{�[���𓊂�����
                //�^�C�}�[���~�߂�
                GameObject.Find("Timer").GetComponent<TimerFillScript>().TimerStop();
                //�X�e�[�g��Move�ɂ���
                m_TeamFlow.SetState(TeamFlowState.Move);
                break;

            case TeamFlowState.Move:
                //�{�[�����܂������Ă���
                //�S�Ẵ{�[������~���Ă��邩���ׂ�
                if(m_TeamFlow.IsStopAllBalls())
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
                if(!m_Delay.IsDelay())
                {
                    //�x�����I������
                    m_TeamFlow.SetState(TeamFlowState.Caluc);
                    m_TeamFlow.ThrowEnd();
                }
                break;

            case TeamFlowState.Caluc:
                //���ɓ�����{�[�����v�Z����
                if(m_BallFlow.IsPreparedJack() == false)
                {
                    //�W���b�N�{�[�����p�ӂ���Ă��Ȃ��̂ŃW���b�N�{�[���𓊂���̂��~�X���Ă���
                    //�W���b�N�{�[���𓊂���`�[����ύX
                    m_TeamFlow.ChangeJackThrowTeam();
                    //�v�Z���ł��Ȃ������̂ŃX�e�[�g��End�ɂ���
                    m_TeamFlow.SetState(TeamFlowState.ThrowEnd);
                }
                else
                {
                    //�c��{�[�����̃e�L�X�g�̃����X�V
                    GameObject.Find("RemainBallText").GetComponent<RemainBallNumScript>().UpdateAlpha();
                    //�W���b�N�{�[���͗p�ӂ���Ă���̂Ŏ��ɓ�����`�[�����v�Z
                    if (m_TeamFlow.CalucNextTeam() == true)
                    {
                        //�v�Z���ł����̂ŃX�e�[�g��Caluced�ɂ���
                        m_TeamFlow.SetState(TeamFlowState.Caluced);
                    }
                    else
                    {
                        //�v�Z���ł��Ȃ������̂ŃX�e�[�g��End�ɂ���
                        m_TeamFlow.SetState(TeamFlowState.End);
                    }
                }
                break;

            case TeamFlowState.Caluced:
                //���ɓ�����{�[�����v�Z�ł�����
                //���ɓ�����`�[�����Z�b�g
                m_TeamFlow.SetNextTeamForClass();
                m_TeamFlow.SetState(TeamFlowState.ThrowEnd);
                break;

            case TeamFlowState.ThrowEnd:
                //�����I���
                //�J������Ǐ]�J��������؂�ւ���
                GameObject.Find("GameCamera").GetComponent<GameCameraScript>().SetIfFollow(false);
                m_TeamFlow.SetState(TeamFlowState.ChangeEnd);
                break;

            case TeamFlowState.ChangeEnd:
                //�`�[���ς��I�����
                //�^�C�}�[�X�^�[�g
                if (m_EndFlow.GetIsEnd() == false)
                {
                    //�G���h���I����Ă��Ȃ��Ƃ�
                    GameObject.Find("Timer").GetComponent<TimerFillScript>().TimerStart();
                }
                //�c��{�[�����̃e�L�X�g���X�V
                GameObject.Find("RemainBallText").GetComponent<RemainBallNumScript>().UpdateRemainText();
                m_TeamFlow.SetState(TeamFlowState.Wait);
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
