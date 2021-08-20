using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchManager : MonoBehaviour
{
    bool m_isTouch = false;      //タッチしているか？
    Vector2 m_touchPos = new Vector2(0, 0);          //座標。
    Vector2 m_oldPos = new Vector2(0, 0);          //前フレームの座標。
    Vector2 m_deltaPos = new Vector2(0, 0);         //前フレーム座標からの差分。
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
            // 座標取得
            this.m_touchPos = Input.mousePosition;

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
                this.m_touchPos = touch.position;
                this.m_deltaPos = touch.deltaPosition;
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
    /// 前フレーム座標との差分。
    /// </summary>
    public Vector2 GetDeltaPos()
    {
        return m_deltaPos;
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
