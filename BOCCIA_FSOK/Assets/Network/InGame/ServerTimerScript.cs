using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using Photon.Pun;
using Photon.Realtime;

/// <summary>
/// サーバー時刻を使ってカウントを行うスクリプト。
/// </summary>
/// <remarks>
/// このスクリプトを使うにはPhotonViewを付けたゲームオブジェクトである必要がある。
/// </remarks>
public class ServerTimerScript : MonoBehaviourPun
{
    private int endTime = 0;    //カウント終了時刻
    public bool isCount { get; private set; } = false;       //カウントを行っているか。

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// カウントの終了時刻をセットするRPC。
    /// </summary>
    /// <param name="count">カウントを終了するサーバー時刻。</param>
    [PunRPC]
    void SetCount(int count)
    {
        //カウントの終了時刻をセットする。
        endTime = count;
        isCount = true;
    }

    /// <summary>
    /// カウントする時間をセットする。(単位:ミリ秒)
    /// </summary>
    /// <param name="count"></param>
    public void SetCountTime(int count)
    {
        if (!photonView.IsMine) return;
        int end = PhotonNetwork.ServerTimestamp + count;
        photonView.RPC(nameof(SetCount), RpcTarget.All, end);
    }
    /// <summary>
    /// カウントする時間をセットする。(単位:秒)
    /// </summary>
    /// <param name="count"></param>
    public void SetCountTimeSecond(float count)
    {
        if (!photonView.IsMine) return;
        int end = PhotonNetwork.ServerTimestamp + (int)(count * 1000.0f);
        photonView.RPC(nameof(SetCount), RpcTarget.All, end);
    }
    /// <summary>
    /// カウントが経過したかどうか。
    /// </summary>
    /// <returns>trueならカウントが経過した。</returns>
    public bool IsCountEnd()
    {
        //カウントしていない。
        if(!isCount)
        {
            return false;
        }
        if(unchecked(PhotonNetwork.ServerTimestamp - endTime) > 0)
        {
            //カウントが終了した。
            isCount = false;
            return true;
        }
        return false;
    }

    /// <summary>
    /// カウントの残り時間。(単位:秒)
    /// </summary>
    /// <returns></returns>
    public float CountLeft()
    {
        //カウントしていない。
        if (!isCount)
        {
            return 0.0f;
        }
        double left = unchecked(endTime - PhotonNetwork.ServerTimestamp);
        left /= 1000;
        if (left < 0.0f)
        {
            //カウントが終了した。
            isCount = false;
            left = 0.0f;
        }
        return (float)left;
    }
}
