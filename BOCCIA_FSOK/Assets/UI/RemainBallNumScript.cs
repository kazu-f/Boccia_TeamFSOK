using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RemainBallNumScript : MonoBehaviour
{
    private GameObject m_GameFlow = null;
    private Text m_Text = null;
    private void Awake()
    {
        m_GameFlow = GameObject.Find("GameFlow");
        if(m_GameFlow == null)
        {
            //�Q�[���t���[�I�u�W�F�N�g�̎擾�Ɏ��s
            Debug.LogError("GameFlow�I�u�W�F�N�g�̎擾�Ɏ��s���܂����B");
        }
        m_Text = this.gameObject.GetComponent<Text>();
        if(m_Text == null)
        {
            //Text�R���|�[�l���g�̎擾�Ɏ��s
            Debug.LogError("Text�R���|�[�l���g�̎擾�Ɏ��s���܂����B");
        }
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// �c��̃{�[������\�����Ă���e�L�X�g���X�V����
    /// </summary>
    public void UpdateRemainText()
    {
        m_Text.text = ("x" + m_GameFlow.GetComponent<TeamFlowScript>().GetRemainBall());
    }

    /// <summary>
    /// �e�L�X�g�̃����X�V����
    /// </summary>
    public void UpdateAlpha()
    {
        if (m_GameFlow.GetComponent<BallFlowScript>().IsPreparedJack())
        {
            Color color = new Color(0.0f, 0.0f, 0.0f, 1.0f);
            m_Text.color = color;
        }
        else
        {
            Color color = new Color(0.0f, 0.0f, 0.0f, 0.0f);
            m_Text.color = color;
        }
    }
}
