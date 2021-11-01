using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JackPleaseScript : MonoBehaviour
{
    enum State
    {
        start,
        stop,
        end
    }
    [SerializeField] private float MoveSpeed = 1.0f;
    [SerializeField] private float StopTime = 5.0f;
    private float late = 0.0f;      //補完率
    private RectTransform rect = null;
    private Vector3 StartPos = Vector3.zero;        //開始位置
    private Vector3 EndPos = Vector3.zero;     //終了位置
    private Vector3 NowPos = Vector3.zero;      //現在の位置
    private Vector3 NextPos = Vector3.zero;     //次の位置
    private State state = State.start;      //状態
    private void Awake()
    {
        rect = this.gameObject.GetComponent<RectTransform>();
    }
    // Start is called before the first frame update
    void Start()
    {
        //最初のポジション
        StartPos = rect.localPosition;
        //最後のポジション
        EndPos = rect.localPosition;
        EndPos.x *= -1.0f;
        //次の位置
        NextPos = StartPos;
        NextPos.x = 0.0f;

    }

    // Update is called once per frame
    void Update()
    {
        ExecuteSlide();
    }

    /// <summary>
    /// 現在の位置と次に移動する位置を計算
    /// </summary>
    public void CalucNowAndNextPos()
    {
        switch (state)
        {
            case State.start:
                NowPos = StartPos;
                NextPos.x = 0.0f;
                break;

            case State.stop:
                NowPos = StartPos;
                NowPos.x = 0.0f;
                NextPos = EndPos;
                break;

            case State.end:
                NowPos = EndPos;
                break;
        }
    }

    public void ExecuteSlide()
    {
        CalucNowAndNextPos();
        if (late < 1.0f)
        {
            late += Time.deltaTime * MoveSpeed;
            rect.localPosition = (late * NextPos) + ((1.0f - late) * NowPos);
        }
        else
        {
            switch (state)
            {
                case State.start:
                    state = State.stop;
                    break;

                case State.stop:
                    state = State.end;
                    break;

                default:
                    break;
            }
            late = 0.0f;
        }
    }
}