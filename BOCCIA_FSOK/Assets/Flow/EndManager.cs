using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndManager : MonoBehaviour
{
    [SerializeField] private TeamFlowScript m_TeamFlow = null;
    [SerializeField] private EndFlowScript m_EndFlow = null;
    [SerializeField] private TeamFlowDelayScript m_Delay = null;
    [SerializeField] private BallFlowScript m_BallFlow = null;
    // Start is called before the first frame update
    void Start()
    {
        
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
                break;

            case TeamFlowState.Wait:
                //�v���C���[��������܂�
                break;

            case TeamFlowState.Throw:
                //�{�[���𓊂�����
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
                }
                break;

            case TeamFlowState.Caluc:
                m_TeamFlow.ThrowEnd();
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
                m_TeamFlow.SetNextTeam();
                //�J������Ǐ]�J��������؂�ւ���
                GameObject.Find("GameCamera").GetComponent<GameCameraScript>().SwitchCamera();
                m_TeamFlow.SetState(TeamFlowState.ThrowEnd);
                break;

            case TeamFlowState.ThrowEnd:

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
