using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TouchManager : MonoBehaviour
{
    public Canvas canvas;
    public RectTransform canvasRect;
    Vector2 m_touchPos = new Vector2(0, 0);          //���W�B
    Vector2 m_touchPosInScreen = new Vector2(0, 0);          //�X�N���[����ł̍��W�B
    Vector2 m_oldPos = new Vector2(0, 0);          //�O�t���[���̍��W�B
    Vector2 m_deltaPos = new Vector2(0, 0);         //�O�t���[�����W����̍����B
    Vector2 m_deltaPosInScreen = new Vector2(0, 0);         //�X�N���[����ł̑O�t���[�����W����̍����B
    TouchPhase m_touchPhase = TouchPhase.Began;     //��ԁB
    bool m_isTouch = false;      //�^�b�`���Ă��邩�H
    bool m_isOnUI = false;       //UI��G���Ă��邩�H

    // Start is called before the first frame update
    void Start()
    {

    }
    /// <summary>
    /// �X�V�����B
    /// </summary>
    public void Update()
    {
        if(IsOnUI() && !m_isTouch)
        {
            //UI��G���Ă���B
            m_isOnUI = true;
        }
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

            if (this.m_isTouch && !m_isOnUI)
            {
                m_deltaPos = m_touchPos - m_oldPos;
                m_oldPos = m_touchPos;
                //�X�N���[����ł̈ʒu�B
                this.m_touchPosInScreen.x = m_touchPos.x / canvasRect.sizeDelta.x + 0.5f;
                this.m_touchPosInScreen.y = m_touchPos.y / canvasRect.sizeDelta.y + 0.5f;
                //�X�N���[����ł�1�t���[���ł̈ړ��ʁB
                this.m_deltaPosInScreen.x = m_deltaPos.x / Screen.width;
                this.m_deltaPosInScreen.y = m_deltaPos.y / Screen.height;
            }
            else
            {
                m_touchPos = m_oldPos;
            }

        }
        else
        {
            // �[��
            if (Input.touchCount == 1)
            {
                Touch touch = Input.GetTouch(0);
                this.m_touchPhase = touch.phase;    //�^�b�`��ԁB
                {
                    this.m_touchPos = touch.position;   //�^�b�`���W�B
                    RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, m_touchPos, canvas.worldCamera, out m_touchPos);
                    //�X�N���[����ł̈ʒu�B
                    this.m_touchPosInScreen.x = m_touchPos.x / canvasRect.sizeDelta.x + 0.5f;
                    this.m_touchPosInScreen.y = m_touchPos.y / canvasRect.sizeDelta.y + 0.5f;
                    this.m_deltaPos = touch.deltaPosition;  //1�t���[���ł̈ړ��ʁB
                                                            //�X�N���[����ł�1�t���[���ł̈ړ��ʁB
                    this.m_deltaPosInScreen.x = touch.deltaPosition.x / Screen.width;
                    this.m_deltaPosInScreen.y = touch.deltaPosition.y / Screen.height;
                    this.m_isTouch = true;              //�^�b�`���Ă���B
                }
            }
            else
            {
                m_touchPhase = TouchPhase.Ended;
            }
        }
        if(m_isOnUI)
        {
            m_isTouch = false;
        }
        //�^�b�`���I����UI��G���Ă��Ȃ����ƂɂȂ�B
        if(m_touchPhase == TouchPhase.Ended)
        {
            m_isOnUI = false;
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

    /// <summary>
    /// UI�̏���^�b�`���Ă��邩�H�B
    /// </summary>
    public static bool IsOnUI()
    {
        PointerEventData pointer = new PointerEventData(EventSystem.current);
#if UNITY_EDITOR
        pointer.position = Input.mousePosition;
#else
        pointer.position = Input.GetTouch(0).position;
#endif
        List<RaycastResult> result = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointer, result);
        int result_uGUI = 0;
        //��������ui��uGUI���ǂ����^�O�Ŕ���B
        foreach(RaycastResult target in result)
        {
            if (target.gameObject.tag == "uGUI")
            {
                result_uGUI++;
            }
        }

        return result_uGUI > 0;
    }

    //�L�����o�X���擾�B
    public Canvas GetCanvas()
    {
        return canvas;
    }

    //�L�����o�X�T�C�Y���擾�B
    public RectTransform GetCavasRect()
    {
        return canvasRect;
    }
}
