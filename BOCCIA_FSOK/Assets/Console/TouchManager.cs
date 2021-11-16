using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// タッチ情報。タッチの状態を表す。
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
    Vector2 m_touchPos = new Vector2(0, 0);          //座標。
    Vector2 m_touchPosInScreen = new Vector2(0, 0);          //スクリーン上での座標。
    Vector2 m_oldPos = new Vector2(0, 0);          //前フレームの座標。
    Vector2 m_deltaPos = new Vector2(0, 0);         //前フレーム座標からの差分。
    Vector2 m_deltaPosInScreen = new Vector2(0, 0);         //スクリーン上での前フレーム座標からの差分。
    TouchInfo m_touchPhase = TouchInfo.Began;     //状態。
    bool m_isTouch = false;      //タッチしているか？
    bool m_isOnUI = false;       //UIを触っているか？

    private static TouchManager instance = null;        //インスタンス変数。
    public static TouchManager GetInstance()
    {
        if(instance == null)
        {
            Debug.LogError("TouchManagerが生成されていない。");
        }
        return instance;
    }

    private void Awake()
    {
        // もしインスタンスが存在するなら、自らを破棄する
        if (instance != null)
        {
            Destroy(this.gameObject);
            return;
        }

        instance = this;
    }

    private void OnDestroy()
    {
        // 破棄時に、登録した実体の解除を行う
        if (this == instance) instance = null;
    }

    // Start is called before the first frame update
    void Start()
    {

    }
    /// <summary>
    /// 更新処理。
    /// </summary>
    public void Update()
    {
        if(IsOnUI() && !m_isTouch)
        {
            //UIを触っている。
            m_isOnUI = true;
        }
        this.m_isTouch = false;

        // エディタ
#if (UNITY_EDITOR || UNITY_STANDALONE_WIN)
        // 座標取得
        this.m_touchPos = Input.mousePosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, m_touchPos, canvas.worldCamera, out m_touchPos);

        // 押した瞬間
        if (Input.GetMouseButtonDown(0))
        {
            this.m_isTouch = true;
            this.m_touchPhase = TouchInfo.Began;
        }
        // 離した瞬間
        else if (Input.GetMouseButtonUp(0))
        {
            this.m_isTouch = true;
            this.m_touchPhase = TouchInfo.Ended;
        }
        // 押しっぱなし
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
            //スクリーン上での位置。
            this.m_touchPosInScreen.x = m_touchPos.x / canvasRect.sizeDelta.x + 0.5f;
            this.m_touchPosInScreen.y = m_touchPos.y / canvasRect.sizeDelta.y + 0.5f;
            //スクリーン上での1フレームでの移動量。
            this.m_deltaPosInScreen.x = m_deltaPos.x / Screen.width;
            this.m_deltaPosInScreen.y = m_deltaPos.y / Screen.height;
        }
        else
        {
            m_touchPos = m_oldPos;
        }
#else
        // 端末
        if (Input.touchCount == 1 && !m_isOnUI)
        {
            Touch touch = Input.GetTouch(0);
            this.m_touchPhase = (TouchInfo)touch.phase;    //タッチ状態。
            {
                this.m_touchPos = touch.position;   //タッチ座標。
                RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, m_touchPos, canvas.worldCamera, out m_touchPos);
                //スクリーン上での位置。
                this.m_touchPosInScreen.x = m_touchPos.x / canvasRect.sizeDelta.x + 0.5f;
                this.m_touchPosInScreen.y = m_touchPos.y / canvasRect.sizeDelta.y + 0.5f;
                this.m_deltaPos = touch.deltaPosition;  //1フレームでの移動量。
                                                        //スクリーン上での1フレームでの移動量。
                this.m_deltaPosInScreen.x = touch.deltaPosition.x / Screen.width;
                this.m_deltaPosInScreen.y = touch.deltaPosition.y / Screen.height;
                this.m_isTouch = true;              //タッチしている。
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
        //タッチが終わればUIを触っていないことになる。
        if(m_touchPhase == TouchInfo.None)
        {
            m_isOnUI = false;
        }
    }
    /// <summary>
    /// タッチしている座標を取得。
    /// </summary>
    public Vector2 GetTouchPos ()
    {
        return m_touchPos;
    }
    /// <summary>
    /// タッチしているスクリーン上の座標を取得。
    /// </summary>
    public Vector2 GetTouchPosInScreen()
    {
        return m_touchPosInScreen;
    }

    /// <summary>
    /// 前フレーム座標との差分。
    /// </summary>
    public Vector2 GetDeltaPos()
    {
        return m_deltaPos;
    }
    /// <summary>
    /// 前フレーム座標とのスクリーン上での差分。
    /// </summary>
    public Vector2 GetDeltaPosInScreen()
    {
        return m_deltaPosInScreen;
    }

    /// <summary>
    /// タッチ状態を取得。
    /// </summary>
    public TouchInfo GetPhase()
    {
        return m_touchPhase;
    }

    /// <summary>
    /// タッチしているか？
    /// </summary>
    public bool IsTouch()
    {
        return m_isTouch;
    }

    /// <summary>
    /// UIの上をタッチしているか？。
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
        //当たったuiがuGUIかどうかタグで判定。
        foreach(RaycastResult target in result)
        {
            if (target.gameObject.tag == "uGUI")
            {
                result_uGUI++;
            }
        }

        return result_uGUI > 0;
    }

    //キャンバスを取得。
    public Canvas GetCanvas()
    {
        return canvas;
    }

    //キャンバスサイズ等取得。
    public RectTransform GetCavasRect()
    {
        return canvasRect;
    }
}
