using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchManager : MonoBehaviour
{
    bool m_isTouch = false;      //�^�b�`���Ă��邩�H
    Vector2 m_touchPos = new Vector2(0, 0);          //���W�B
    Vector2 m_oldPos = new Vector2(0, 0);          //�O�t���[���̍��W�B
    Vector2 m_deltaPos = new Vector2(0, 0);         //�O�t���[�����W����̍����B
    TouchPhase m_touchPhase = TouchPhase.Began;     //��ԁB

    /// <summary>
    /// �X�V�����B
    /// </summary>
    public void Update()
    {
        this.m_isTouch = false;

        // �G�f�B�^
        if (Application.isEditor)
        {
            // ���W�擾
            this.m_touchPos = Input.mousePosition;

            // �������u��
            if (Input.GetMouseButtonDown(0))
            {
                this.m_isTouch = true;
                this.m_touchPhase = TouchPhase.Began;
            }

            // �������u��
            else if (Input.GetMouseButtonUp(0))
            {
                this.m_isTouch = true;
                this.m_touchPhase = TouchPhase.Ended;
            }

            // �������ςȂ�
            else if (Input.GetMouseButton(0))
            {
                this.m_isTouch = true;
                if(m_oldPos == m_touchPos)
                {
                    this.m_touchPhase = TouchPhase.Stationary;
                }
                else
                {
                    this.m_touchPhase = TouchPhase.Moved;
                }
            }

            if (this.m_isTouch)
            {
                m_deltaPos = m_touchPos - m_oldPos;
                m_oldPos = m_touchPos;
            }
            else
            {
                m_touchPos = m_oldPos;
            }

            // �[��
        }
        else
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                this.m_touchPos = touch.position;
                this.m_deltaPos = touch.deltaPosition;
                this.m_touchPhase = touch.phase;
                this.m_isTouch = true;
            }
        }
    }
    /// <summary>
    /// �^�b�`�������W���擾�B
    /// </summary>
    public Vector2 GetTouchPos ()
    {
        return m_touchPos;
    }
    /// <summary>
    /// �O�t���[�����W�Ƃ̍����B
    /// </summary>
    public Vector2 GetDeltaPos()
    {
        return m_deltaPos;
    }

    /// <summary>
    /// �^�b�`��Ԃ��擾�B
    /// </summary>
    public TouchPhase GetPhase()
    {
        return m_touchPhase;
    }

    /// <summary>
    /// �^�b�`���Ă��邩�H
    /// </summary>
    public bool IsTouch()
    {
        return m_isTouch;
    }
}
