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
		//ダミー表示に使うオブジェクトのリスト。
		private List<GameObject> dummySphereList = new List<GameObject>();

		private TeamFlowScript TeamFlow;
		private BallFlowScript jackFlow;
		public Material ballCol;
        [SerializeField] private Color[] ballColList = new Color[3] {
			new Color(1.0f,1.0f,1.0f,0.5f),
			new Color(1.0f,0.2f,0.2f,0.5f),
			new Color(0.2f,0.2f,1.0f,0.5f)
		};



		//シングルトン。
		static private ThrowDummyScript instance;

		//インスタンスを取得。
		static public ThrowDummyScript Instance()
        {
			return instance;
        }

		private void Awake()
		{
			if(instance != null)
            {
				Destroy(this.gameObject);
				Debug.LogWarning("ThrowDummyScriptが複数作成されている。");
				return;
			}
			instance = this;

			dummyObjParent.transform.position = transform.position;

			//弾道予測を表示するための点を生成
			for (int i = 0; i < dummyCount; i++)
			{
				var obj = (GameObject)Instantiate(dummyObjPref, dummyObjParent);
				dummySphereList.Add(obj);
			}

			var gameFlow = GameObject.FindGameObjectWithTag("GameFlow");
			if (gameFlow)
			{
				TeamFlow = gameFlow.GetComponent<TeamFlowScript>();
				jackFlow = gameFlow.GetComponent<BallFlowScript>();
			}
		}
		private void OnDestroy()
		{
			// 破棄時に、登録した実体の解除を行う
			if (this == instance) instance = null;
		}

		void Start()
		{

		}

		void Update()
		{
			offset = Mathf.Repeat(Time.time * offsetSpeed, secInterval);
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
			if(!jackFlow.IsPreparedJack())
            {
				ballCol.color = ballColList[0];
			}
            else
			{
				switch (TeamFlow.GetNowTeam())
				{
				case Team.Red:
					ballCol.color = ballColList[1];
						break;

					case Team.Blue:					
					ballCol.color = ballColList[2];
						break;
				}
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
