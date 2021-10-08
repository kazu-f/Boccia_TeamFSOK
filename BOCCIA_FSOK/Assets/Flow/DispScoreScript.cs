using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DispScoreScript : MonoBehaviour
{
    public Text m_ScoreResult;

    private void Awake()
    {

    }
    // Start is called before the first frame update
    void Start()
    {
        m_ScoreResult.color = new Color(1.0f, 0.0f, 0.0f, 0.0f);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void DispResult(int score)
    {
        m_ScoreResult.text = score + "�_!";
    }

    /// <summary>
    /// �e�L�X�g���`�[���ɂ���ĕύX����
    /// </summary>
    /// <param name="team">���_�����`�[��</param>
    public void SetTextTeam(Team team)
    {
        //���_�����`�[���ɂ���ĐF��ύX
        switch(team)
        {
            case Team.Red:
                //�Ԃɕς���
                m_ScoreResult.color = Color.red;
                //m_ScoreResult.text = "�ԃ`�[����";
                break;
            case Team.Blue:
                //�ɕς���
                m_ScoreResult.color = Color.blue;
                //m_ScoreResult.text = "�`�[����";
                break;
            default:
                return;
        }
    }

    //���Z�b�g�֐�
    public void ResetVar()
    {
        m_ScoreResult.color = Vector4.zero;
    }
}
