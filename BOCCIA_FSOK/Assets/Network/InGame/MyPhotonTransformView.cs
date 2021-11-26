using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Photon.Pun
{
    /// <summary>
    /// 補間のないTransformの同期を行うスクリプト。
    /// </summary>
    public class MyPhotonTransformView : MonoBehaviourPun, IPunObservable
    {
        [Tooltip("Positionの同期を行うかどうか。ルーム内のプレイヤーで同じ設定でないと上手くいかない。")]
        public bool m_SynchronizePosition = true;
        [Tooltip("Rotationの同期を行うかどうか。ルーム内のプレイヤーで同じ設定でないと上手くいかない。")]
        public bool m_SynchronizeRotation = true;

        Vector3 oldPos = Vector3.zero;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            var tr = this.transform;
            var v = Vector3.zero;
            v.y = 1.0f;
            v.z = 1.0f;

            if (v == tr.position)
            {
                if(photonView.IsMine)
                {
                    Debug.Log("IsMine == true" + tr.position);
                }
                else
                {
                    Debug.Log("IsMine == false" + tr.position);
                }
            }

        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            var tr = this.transform;
            if (stream.IsWriting)
            {
                if (this.m_SynchronizePosition)
                {
                    stream.SendNext(tr.position);
                    //Debug.Log("SendPosition" + tr.position);
                    var v = Vector3.zero;
                    v.y = 1.0f;
                    v.z = 1.0f;

                    if (v == tr.position)
                    {
                        Debug.Log("SendPosition" + tr.position);
                    }
                    oldPos = tr.position;
                }
                if (this.m_SynchronizeRotation)
                {
                    stream.SendNext(tr.rotation);
                    //Debug.Log("SendRotation" + tr.rotation);
                }
            }
            else
            {
                if (this.m_SynchronizePosition)
                {
                    tr.position = (Vector3)stream.ReceiveNext();
                    //Debug.Log("ReceivePosition" + tr.position);
                    var v = Vector3.zero;
                    v.y = 1.0f;
                    v.z = 1.0f;

                    if (v == tr.position)
                    {
                        Debug.Log("ReceivePosition" + tr.position);
                    }
                    oldPos = tr.position;
                }
                if (this.m_SynchronizeRotation)
                {
                    tr.rotation = (Quaternion)stream.ReceiveNext();
                    //Debug.Log("ReceiveRotation" + tr.rotation);
                }
            }
        }
    }
}
