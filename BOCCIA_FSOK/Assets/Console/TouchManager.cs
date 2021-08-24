using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TouchManager : MonoBehaviour
{
    public Canvas canvas;
    RectTransform canvasRect;
    bool m_isTouch = false;      //�^�b�`���Ă��邩�H
    Vector2 m_touchPos = new Vector2(0, 0);          //���W�B
    Vector2 m_touchPosInScreen = new Vector2(0, 0);          //�X�N���[����ł̍��W�B
    Vector2 m_oldPos = new Vector2(0, 0);          //�O�t���[���̍��W�B
    Vector2 m_deltaPos = new Vector2(0, 0);         //�O�t���[�����W����̍����B
    Vector2 m_deltaPosInScreen = new Vector2(0, 0);         //�X�N���[����ł̑O�t���[�����W����̍����B
    TouchPhase m_touchPhase = TouchPhase.Began;     //��ԁB

    // Start is called before the first frame update
    void Start()
    {
        canvasRect = canvas.GetComponent<RectTransform>();
    }
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
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, m_touchPos, canvas.worldCamera, out m_touchPos);

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
                //�X�N���[����ł̈ʒu�B
                this.m_touchPosInScreen.x = m_touchPos.x / Screen.width + 0.5f;
                this.m_touchPosInScreen.y = m_touchPos.y / Screen.height + 0.5f;
                //�X�N���[����ł�1�t���[���ł̈ړ��ʁB
                this.m_deltaPosInScreen.x = m_deltaPos.x / Screen.width;
                this.m_deltaPosInScreen.y = m_deltaPos.y / Screen.height;
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
                this.m_touchPos = touch.position;   //�^�b�`���W�B
                RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, m_touchPos, canvas.worldCamera, out m_touchPos);
                //�X�N���[����ł̈ʒu�B
                this.m_touchPosInScreen.x = touch.position.x / Screen.width;
                this.m_touchPosInScreen.y = touch.position.y / Screen.height;
                this.m_deltaPos = touch.deltaPosition;  //1�t���[���ł̈ړ��ʁB
                //�X�N���[����ł�1�t���[���ł̈ړ��ʁB
                this.m_deltaPosInScreen.x = touch.deltaPosition.x / Screen.width;
                this.m_deltaPosInScreen.y = touch.deltaPosition.y / Screen.height;
                this.m_touchPhase = touch.phase;    //�^�b�`��ԁB
                this.m_isTouch = true;              //�^�b�`���Ă���B
            }
        }
    }
    /// <summary>
    /// �^�b�`���Ă�����W���擾�B
    /// </summary>
    public Vector2 GetTouchPos ()
    {
        return m_touchPos;
    }
    /// <summary>
    /// �^�b�`���Ă���X�N���[����̍��W���擾�B
    /// </summary>
    public Vector2 GetTouchPosInScreen()
    {
        return m_touchPosInScreen;
    }

    /// <summary>
    /// �O�t���[�����W�Ƃ̍����B
    /// </summary>
    public Vector2 GetDeltaPos()
    {
        return m_deltaPos;
    }
    /// <summary>
    /// �O�t���[�����W�Ƃ̃X�N���[����ł̍����B
    /// </summary>
    public Vector2 GetDeltaPosInScreen()
    {
        return m_deltaPosInScreen;
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
