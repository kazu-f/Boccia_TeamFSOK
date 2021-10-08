using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeBallSprite : MonoBehaviour
{
    public Sprite m_JackBallSprite;
    public Sprite m_RedBallSprite;
    public Sprite m_BlueBallSprite;
    private Image m_Image = null;
    // Start is called before the first frame update
    void Start()
    {
        m_Image = this.gameObject.GetComponent<Image>();
        m_Image.sprite = m_JackBallSprite;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeSprite(Team NextTeam)
    {
        switch (NextTeam)
        {
            case Team.Red:
                m_Image.sprite = m_RedBallSprite;
                break;

            case Team.Blue:
                m_Image.sprite = m_BlueBallSprite;
                break;

            default:
                Debug.LogError("���̃`�[���������ɓ���Ă�������");
                return;
        }
    }

    public void GameEnd()
    {
        var c = m_Image.color;
        //����0�ɂ���
        m_Image.color = new Color(c.r, c.g, c.b, 0.0f);
    }
    /// <summary>
    /// ���Z�b�g����
    /// </summary>
    public void ResetVar()
    {
        //����߂�
        m_Image.sprite = m_JackBallSprite;
        var c = m_Image.color;
        //����0�ɂ���
        m_Image.color = new Color(c.r, c.g, c.b, 1.0f);

    }
}
