using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BocciaPlayer
{
	public class ThrowDummyScript : MonoBehaviour
	{
		[SerializeField]private GameObject dummyObjPref;        //弾道を表示するための点のプレハブ
		[SerializeField]private Transform dummyObjParent;       //弾道を表示する点の親オブジェクト

		private Vector3 force;      //初速のベクトル。
		private Vector3 oldForce;	//前フレームのベクトル。

		[SerializeField]private int dummyCount;			//弾道予測の点の数

		[SerializeField]private float secInterval;      //弾道を表示する間隔の秒数

		private List<GameObject> dummySphereList = new List<GameObject>();

        private void Awake()
		{
			dummyObjParent.transform.position = transform.position;

			//弾道予測を表示するための点を生成
			for (int i = 0; i < dummyCount; i++)
			{
				var obj = (GameObject)Instantiate(dummyObjPref, dummyObjParent);
				dummySphereList.Add(obj);
			}
		}

        void Start()
		{

		}

		void Update()
		{
			//更新がなければ処理しない。
			if (force == oldForce) return;
			//弾道予測の位置に点を移動
			for (int i = 0; i < dummyCount; i++)
			{
				var t = i * secInterval;
				float x = t * force.x + transform.position.x;
				float z = t * force.z + transform.position.z;
				float y = (force.y * t) - 0.5f * (-Physics.gravity.y) * Mathf.Pow(t, 2.0f) + transform.position.y;
				dummySphereList[i].transform.position = new Vector3(x, y, z);
			}

			oldForce = force;
		}

		/// <summary>
		/// 始点をセット。
		/// </summary>
		public void SetPosition(Vector3 _pos)
        {
			transform.position = _pos;
		}
		/// <summary>
		/// 力をセット。
		/// </summary>
		public void SetForce(Vector3 _force)
        {
			force = _force;
        }

        public void OnEnable()
        {
			//オブジェクトが有効になった。
			for (int i = 0; i < dummyCount; i++)
			{
				dummySphereList[i].SetActive(true);
			}
        }

        public void OnDisable()
		{
			//オブジェクトが有効になった。
			for (int i = 0; i < dummyCount; i++)
			{
				dummySphereList[i].SetActive(false);
			}
		}
    }
}
