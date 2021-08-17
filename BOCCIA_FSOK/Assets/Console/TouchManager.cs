using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchManager
{
    bool m_isTouch = false;      //タッチしているか？
    Vector2 m_touchPos = new Vector2(0, 0);          //座標。
    TouchPhase m_touchPhase = TouchPhase.Began;     //状態。

    /// <summary>
    /// 更新処理。
    /// </summary>
    public void Update()
    {
        this.m_isTouch = false;

        // エディタ
        if (Application.isEditor)
        {
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
                this.m_touchPhase = TouchPhase.Moved;
            }

            // 座標取得
            if (this.m_isTouch) this.m_touchPos = Input.mousePosition;

            // 端末
        }
        else
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                this.m_touchPos = touch.position;
                this.m_touchPhase = touch.phase;
                this.m_isTouch = true;
            }
        }
    }
    /// <summary>
    /// タッチした座標を取得。
    /// </summary>
    public Vector2 GetTouchPos ()
    {
        return m_touchPos;
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
