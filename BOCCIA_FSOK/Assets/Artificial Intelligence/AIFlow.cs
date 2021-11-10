using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BocciaPlayer;

public class AIFlow : IPlayerController
{
    private TouchInfo info;
    private Transform ThrowTrance;
    private Vector2 ThrowPower = Vector2.zero;
    private GameObject JackBoll = null;
    private Rigidbody JackBody = null;
    private Vector3 JackPos = Vector3.zero;
    /// <summary>
    /// プレイヤーリセット
    /// PlayerControllerと一緒
    /// </summary>
    override public void ResetPlayer()
    {
        ballHolderController.ResetBall();
    }
    public override void SwitchPlayer(bool isEnable)
    {
        //開始時点のトランスフォームへ移動。
        if (isEnable == true)
        {
            //プレイヤーが切り替わる時にカメラの位置を合わせる。
            throwAngleController.ChangeCamPos();
        }
    }

    private void Awake()
    {
        JackBoll = GameObject.Find("JackBoll");
        if (JackBoll == null)
        {
            Debug.LogError("JackBollが見つかりませんでした。");
        }
        JackBody = JackBoll.GetComponent<Rigidbody>();
        if (JackBody == null)
        {
            Debug.LogError("JackBollのリジットボディが見つかりませんでした。");
        }
        ThrowTrance = GameObject.Find("Players").transform;
        
    }
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        //ジャックボールの位置
        JackPos = JackBody.position;
        //投げたい位置を計算
        Vector3 TargetPos = JackPos - ThrowTrance.position;
        //正規化して投げる方向だけにする
        Vector3 TargetNorm = TargetPos.normalized;
        //投げるプレイヤーの前方向
        Vector3 ThrowForward = ThrowTrance.forward;
        //プレイヤーをジャックボールの方向に向ける
        ThrowTrance.rotation.SetFromToRotation(ThrowForward, TargetNorm);

        Vector2 throwPow = Vector2.zero;
        throwPow.y = 1.0f;
        throwBallControler.SetThrowPow(throwPow);
  }
    public TouchInfo GetPath()
    {
        return info;
    }

    public float FllowPower()
    {
        //投げる値はテキトー
        return 1.0f;
    } 
}
