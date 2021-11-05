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
    private float NowTime = 0.0f;
    private float late = 0.0f;      //補完率
    private RectTransform rect = null;
    [SerializeField]private Vector3 StartPos = Vector3.zero;        //開始位置
    private Vector3 EndPos = Vector3.zero;     //終了位置
    private Vector3 NowPos = Vector3.zero;      //現在の位置
    private Vector3 NextPos = Vector3.zero;     //次の位置
    private State state = State.start;      //状態
    private bool SlideStart = false;        //移動スタートフラグ
    private bool IsMove = true;        //動くかどうかのフラグ
    private void Awake()
    {
        rect = this.gameObject.GetComponent<RectTransform>();
    }
    // Start is called before the first frame update
    void Start()
    {
        //最初のポジション
        //StartPos = rect.localPosition;
        StartPos.x = rect.rect.width+50;
        //最後のポジション
        EndPos = StartPos;
        EndPos.x = -(rect.rect.width+50);
        //次の位置
        NextPos = StartPos;
        NextPos.x = 0.0f;

    }

    // Update is called once per frame
    void Update()
    {
        if (SlideStart)
        {
            SwichActiveGameObjects.GetInstance().SwitchGameObject(false);
            //投げれないようにする
            var player = GameObject.Find("Players");
            if (player != null)
            {
                player.GetComponent<ActiveTeamController>().StopThrow();
            }
            ExecuteSlide();
        }
    }

    /// <summary>
    /// 現在の位置と次に移動する位置を計算
    /// </summary>
    private void CalucNowAndNextPos()
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

    private void ExecuteSlide()
    {
        CalucNowAndNextPos();
        if(!IsMove)
        {
            if (TouchManager.GetInstance().IsTouch())
            {
                IsMove = true;
                NowTime = 0.0f;
                return;
            }
            NowTime += Time.deltaTime;
            if(NowTime > StopTime)
            {
                IsMove = true;
                NowTime = 0.0f;
            }
            return;
        }    
        if (late < 1.0f)
        {
            late += Time.deltaTime * MoveSpeed;
            late = Mathf.Min(late, 1.0f);
            rect.localPosition = (late * NextPos) + ((1.0f - late) * NowPos);
        }
        else
        {
            //補完率が1を超えた時
            //つまり次のステートに移行するとき
            switch (state)
            {
                case State.start:
                    state = State.stop;
                    IsMove = false;
                    break;

                case State.stop:
                    state = State.end;
                    break;

                default:
                    //投げれるようにする
                    GameObject.Find("Players").GetComponent<ActiveTeamController>().ChangeActivePlayer();
                    SwichActiveGameObjects.GetInstance().SwitchGameObject(true);
                    SlideStart = false;
                    return;
            }
            late = 0.0f;
        }
    }

    public void StartSlide()
    {
        SlideStart = true;
        rect.localPosition = StartPos;
        state = State.start;
        late = 0.0f;
    }
}