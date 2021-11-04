using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BocciaPlayer
{
	public class ThrowDummyScript : MonoBehaviour
	{
		[SerializeField]private GameObject dummyObjPref;        //弾道を表示するための点のプレハブ
		[SerializeField]private Transform dummyObjParent;       //弾道を表示する点の親オブジェクト
		[SerializeField]private GameObject ballPrefab;          //ボールのプレファブ。
		[SerializeField]private PhysicMaterial courtPhysicMat;  //コートの物理マテリアルプレファブ。
		[SerializeField] private BoxCollider courtBoxCol;		//コートのボックスコライダー。

		private Vector3 force;      //初速のベクトル。
		[SerializeField] private float ballMass;            //ボールの質量。
		private float courtBounciness;                      //コートの反発係数。
		private float courtMaxY;                            //コートの上面。
		private float ballHalfSize;							//ボールのサイズ。

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
				//ボール管理のスクリプト。
				TeamFlow = gameFlow.GetComponent<TeamFlowScript>();
				jackFlow = gameFlow.GetComponent<BallFlowScript>();
			}

			//ボールの半径を取得。
			ballHalfSize = dummyObjPref.transform.localScale.y / 2.0f;

			//プレファブからリジッドボディ取得。
			var RB = ballPrefab.GetComponent<Rigidbody>();
			ballMass = RB.mass;     //質量を取得。
			
			//プレファブから物理マテリアル取得。
			courtBounciness = courtPhysicMat.bounciness;

			//地面の高さを取得。
			courtMaxY = courtBoxCol.bounds.max.y;
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
			//DummyGround();
			//DummyNoBounce();
			DummyBounce();
		}

		//ゴロでダミーを表示。
		void DummyGround()
		{
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
					Vector3 resPos = vec + transform.position;
					dummySphereList[i].SetActive(true);
					if(resPos.y < courtMaxY)
                    {
						resPos.y = courtMaxY;
					}
					resPos.y += ballHalfSize;
					dummySphereList[i].transform.position = resPos;
				}
			}
		}
		//バウンス無しでダミーを表示。
		void DummyNoBounce()
		{
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
					vec.y += ballHalfSize;
					dummySphereList[i].transform.position = vec;
				}
			}
		}
		//バウンスありでダミーを表示。
		void DummyBounce()
		{
			Vector3 pos = transform.position;
			Vector3 oldPos = transform.position;
			Vector3 vecForce = force;
			int count = 0;
			float BounceOffset = 0.0f;

			while(count < dummyCount)
			{
				//弾道予測の位置に点を移動
				for (int i = 0; count < dummyCount; i++)
				{
					Vector3 vec = new Vector3();
					var t = ((i + 1) * secInterval) + offset + BounceOffset;
					vec = (vecForce * t) + (0.5f * Physics.gravity * Mathf.Pow(t, 2.0f));
					if (vec.magnitude < DummyGraceDist
						&& pos == transform.position)
					{
						dummySphereList[count].SetActive(false);
						oldPos = vec + pos;       //記録しておく。
					}
					else
					{
						dummySphereList[count].SetActive(true);
						Vector3 resPos = pos + vec;
						//コートの上にある。
						if (resPos.y > courtMaxY)
						{
							oldPos = resPos;       //記録しておく。
							resPos.y += ballHalfSize;
							dummySphereList[count].transform.position = resPos;
						}
						//コートの下にある。
                        else
                        {
							Vector3 vDif = resPos - oldPos;
							float sum = (Mathf.Abs(resPos.y - courtMaxY) + (oldPos.y - courtMaxY));
							float weight = 1.0f;
							if (sum > 0.0f)
							{
								weight = Mathf.Abs(resPos.y - courtMaxY) / sum;
								weight = Mathf.Clamp(weight, 0.0f, 1.0f);
								resPos -= vDif * weight;
							}
							pos = resPos;
							BounceOffset = secInterval * weight;
							vecForce += Physics.gravity * (t - BounceOffset);
							vecForce *= courtBounciness;
							vecForce.y *= -1.0f;
							vec = (vecForce * BounceOffset) + (0.5f * Physics.gravity * Mathf.Pow(BounceOffset, 2.0f));
							resPos = pos + vec;
							oldPos = resPos;       //記録しておく。
							resPos.y += ballHalfSize;
							dummySphereList[count].transform.position = resPos;
							count++;
							break;
						}
					}
					count++;
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
			force = _force / ballMass * Time.fixedDeltaTime;
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
