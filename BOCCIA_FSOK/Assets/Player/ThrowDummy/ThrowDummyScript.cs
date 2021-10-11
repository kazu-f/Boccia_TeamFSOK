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
		[SerializeField] private float mass;			//質量。

		[SerializeField]private int dummyCount;			//弾道予測の点の数
		[SerializeField]private float secInterval;      //弾道を表示する間隔の秒数
		[SerializeField] private float DummyGraceDist;  //非表示距離。

		[SerializeField] private float offsetSpeed;
		private float offset = 0.0f;

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
			offset = Mathf.Repeat(Time.time * offsetSpeed, secInterval);
			////更新がなければ処理しない。
			//if (force == oldForce) return;
			//弾道予測の位置に点を移動
			for (int i = 0; i < dummyCount; i++)
			{
				var t = (i * secInterval) + offset;
				Vector3 vec = new Vector3();
				vec = (force * t) + (0.5f * Physics.gravity * Mathf.Pow(t, 2.0f));
				if (vec.magnitude < DummyGraceDist)
                {
					dummySphereList[i].SetActive(false);
				}
                else
				{
					vec = vec + transform.position;
					dummySphereList[i].SetActive(true);
					dummySphereList[i].transform.position = vec;						
				}
			}

			//oldForce = force;
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
			force = _force / mass * Time.fixedDeltaTime;
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
