using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// �^�b�`���B�^�b�`�̏�Ԃ�\���B
/// </summary>
public enum TouchInfo
{ 
    Began = TouchPhase.Began,
    Moved = TouchPhase.Moved,
    Stationary = TouchPhase.Stationary,
    Ended = TouchPhase.Ended,
    Canceled = TouchPhase.Canceled,
    None
}


public class TouchManager : MonoBehaviour
{
    public Canvas canvas;
    public RectTransform canvasRect;
    Vector2 m_touchPos = new Vector2(0, 0);          //���W�B
    Vector2 m_touchPosInScreen = new Vector2(0, 0);          //�X�N���[����ł̍��W�B
    Vector2 m_oldPos = new Vector2(0, 0);          //�O�t���[���̍��W�B
    Vector2 m_deltaPos = new Vector2(0, 0);         //�O�t���[�����W����̍����B
    Vector2 m_deltaPosInScreen = new Vector2(0, 0);         //�X�N���[����ł̑O�t���[�����W����̍����B
    TouchInfo m_touchPhase = TouchInfo.Began;     //��ԁB
    bool m_isTouch = false;      //�^�b�`���Ă��邩�H
    bool m_isOnUI = false;       //UI��G���Ă��邩�H

    private static TouchManager instance = null;        //�C���X�^���X�ϐ��B
    public static TouchManager GetInstance()
    {
        if(instance == null)
        {
            Debug.LogError("TouchManager����������Ă��Ȃ��B");
        }
        return instance;
    }

    private void Awake()
    {
        // �����C���X�^���X�����݂���Ȃ�A�����j������
        if (instance != null)
        {
            Destroy(this.gameObject);
            return;
        }

        instance = this;
    }

    private void OnDestroy()
    {
        // �j�����ɁA�o�^�������̂̉������s��
        if (this == instance) instance = null;
    }

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
#if (UNITY_EDITOR || UNITY_STANDALONE_WIN)
        // ���W�擾
        this.m_touchPos = Input.mousePosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, m_touchPos, canvas.worldCamera, out m_touchPos);

        // �������u��
        if (Input.GetMouseButtonDown(0))
        {
            this.m_isTouch = true;
            this.m_touchPhase = TouchInfo.Began;
        }
        // �������u��
        else if (Input.GetMouseButtonUp(0))
        {
            this.m_isTouch = true;
            this.m_touchPhase = TouchInfo.Ended;
        }
        // �������ςȂ�
        else if (Input.GetMouseButton(0))
        {
            this.m_isTouch = true;
            if (m_oldPos == m_touchPos)
            {
                this.m_touchPhase = TouchInfo.Stationary;
            }
            else
            {
                this.m_touchPhase = TouchInfo.Moved;
            }
        }
        else
        {
            this.m_touchPhase = TouchInfo.None;
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
#else
        // �[��
        if (Input.touchCount == 1 && !m_isOnUI)
        {
            Touch touch = Input.GetTouch(0);
            this.m_touchPhase = (TouchInfo)touch.phase;    //�^�b�`��ԁB
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
            m_touchPhase = TouchInfo.None;
        }
#endif
        if (m_isOnUI)
        {
            m_isTouch = false;
        }
        //�^�b�`���I����UI��G���Ă��Ȃ����ƂɂȂ�B
        if(m_touchPhase == TouchInfo.None)
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
    public TouchInfo GetPhase()
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
#if (UNITY_EDITOR || UNITY_STANDALONE_WIN)
        if (!Input.GetMouseButton(0))
        {
            return false;
        }
#else
        if(Input.touchCount == 0)
        {
            return false;
        }
#endif

        PointerEventData pointer = new PointerEventData(EventSystem.current);

#if (UNITY_EDITOR || UNITY_STANDALONE_WIN)
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
