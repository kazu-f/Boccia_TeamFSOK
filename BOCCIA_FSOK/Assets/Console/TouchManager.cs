using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TouchManager : MonoBehaviour
{
    public Canvas canvas;
    RectTransform canvasRect;
    bool m_isTouch = false;      //タッチしているか？
    Vector2 m_touchPos = new Vector2(0, 0);          //座標。
    Vector2 m_touchPosInScreen = new Vector2(0, 0);          //スクリーン上での座標。
    Vector2 m_oldPos = new Vector2(0, 0);          //前フレームの座標。
    Vector2 m_deltaPos = new Vector2(0, 0);         //前フレーム座標からの差分。
    Vector2 m_deltaPosInScreen = new Vector2(0, 0);         //スクリーン上での前フレーム座標からの差分。
    TouchPhase m_touchPhase = TouchPhase.Began;     //状態。

    // Start is called before the first frame update
    void Start()
    {
        canvasRect = canvas.GetComponent<RectTransform>();
    }
    /// <summary>
    /// 更新処理。
    /// </summary>
    public void Update()
    {
        this.m_isTouch = false;

        // エディタ
        if (Application.isEditor)
        {
            // 座標取得
            this.m_touchPos = Input.mousePosition;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, m_touchPos, canvas.worldCamera, out m_touchPos);

            // 押した瞬間
            if (Input.GetMouseButtonDown(0))
            {
                this.m_isTouch = true;
                this.m_touchPhase = TouchPhase.Began;
            }

            // 離した瞬間
            else if (Input.GetMouseButtonUp(0))
            {
                this.m_isTouch = true;
                this.m_touchPhase = TouchPhase.Ended;
            }

            // 押しっぱなし
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
                //スクリーン上での位置。
                this.m_touchPosInScreen.x = m_touchPos.x / Screen.width + 0.5f;
                this.m_touchPosInScreen.y = m_touchPos.y / Screen.height + 0.5f;
                //スクリーン上での1フレームでの移動量。
                this.m_deltaPosInScreen.x = m_deltaPos.x / Screen.width;
                this.m_deltaPosInScreen.y = m_deltaPos.y / Screen.height;
            }
            else
            {
                m_touchPos = m_oldPos;
            }

            // 端末
        }
        else
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                this.m_touchPos = touch.position;   //タッチ座標。
                RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, m_touchPos, canvas.worldCamera, out m_touchPos);
                //スクリーン上での位置。
                this.m_touchPosInScreen.x = touch.position.x / Screen.width;
                this.m_touchPosInScreen.y = touch.position.y / Screen.height;
                this.m_deltaPos = touch.deltaPosition;  //1フレームでの移動量。
                //スクリーン上での1フレームでの移動量。
                this.m_deltaPosInScreen.x = touch.deltaPosition.x / Screen.width;
                this.m_deltaPosInScreen.y = touch.deltaPosition.y / Screen.height;
                this.m_touchPhase = touch.phase;    //タッチ状態。
                this.m_isTouch = true;              //タッチしている。
            }
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
    public TouchPhase GetPhase()
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
}
