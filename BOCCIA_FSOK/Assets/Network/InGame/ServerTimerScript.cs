using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using Photon.Pun;
using Photon.Realtime;

/// <summary>
/// �T�[�o�[�������g���ăJ�E���g���s���X�N���v�g�B
/// </summary>
/// <remarks>
/// ���̃X�N���v�g���g���ɂ�PhotonView��t�����Q�[���I�u�W�F�N�g�ł���K�v������B
/// </remarks>
public class ServerTimerScript : MonoBehaviourPun
{
    private int endTime = 0;    //�J�E���g�I������
    public bool isCount { get; private set; } = false;       //�J�E���g���s���Ă��邩�B

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// �J�E���g�̏I���������Z�b�g����RPC�B
    /// </summary>
    /// <param name="count">�J�E���g���I������T�[�o�[�����B</param>
    [PunRPC]
    void SetCount(int count)
    {
        //�J�E���g�̏I���������Z�b�g����B
        endTime = count;
        isCount = true;
    }

    /// <summary>
    /// �J�E���g���鎞�Ԃ��Z�b�g����B(�P��:�~���b)
    /// </summary>
    /// <param name="count"></param>
    public void SetCountTime(int count)
    {
        if (!photonView.IsMine) return;
        int end = PhotonNetwork.ServerTimestamp + count;
        photonView.RPC(nameof(SetCount), RpcTarget.All, end);
    }
    /// <summary>
    /// �J�E���g���鎞�Ԃ��Z�b�g����B(�P��:�b)
    /// </summary>
    /// <param name="count"></param>
    public void SetCountTimeSecond(float count)
    {
        if (!photonView.IsMine) return;
        int end = PhotonNetwork.ServerTimestamp + (int)(count * 1000.0f);
        photonView.RPC(nameof(SetCount), RpcTarget.All, end);
    }
    /// <summary>
    /// �J�E���g���o�߂������ǂ����B
    /// </summary>
    /// <returns>true�Ȃ�J�E���g���o�߂����B</returns>
    public bool IsCountEnd()
    {
        //�J�E���g���Ă��Ȃ��B
        if(!isCount)
        {
            return false;
        }
        if(unchecked(PhotonNetwork.ServerTimestamp - endTime) > 0)
        {
            //�J�E���g���I�������B
            isCount = false;
            return true;
        }
        return false;
    }

    /// <summary>
    /// �J�E���g�̎c�莞�ԁB(�P��:�b)
    /// </summary>
    /// <returns></returns>
    public float CountLeft()
    {
        //�J�E���g���Ă��Ȃ��B
        if (!isCount)
        {
            return 0.0f;
        }
        double left = unchecked(endTime - PhotonNetwork.ServerTimestamp);
        left /= 1000;
        if (left < 0.0f)
        {
            //�J�E���g���I�������B
            isCount = false;
            left = 0.0f;
        }
        return (float)left;
    }
}
