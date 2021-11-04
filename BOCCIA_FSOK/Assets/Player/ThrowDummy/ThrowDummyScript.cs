using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BocciaPlayer
{
	public class ThrowDummyScript : MonoBehaviour
	{
		[SerializeField]private GameObject dummyObjPref;        //�e����\�����邽�߂̓_�̃v���n�u
		[SerializeField]private Transform dummyObjParent;       //�e����\������_�̐e�I�u�W�F�N�g
		[SerializeField]private GameObject ballPrefab;          //�{�[���̃v���t�@�u�B
		[SerializeField]private PhysicMaterial courtPhysicMat;  //�R�[�g�̕����}�e���A���v���t�@�u�B
		[SerializeField] private BoxCollider courtBoxCol;		//�R�[�g�̃{�b�N�X�R���C�_�[�B

		private Vector3 force;      //�����̃x�N�g���B
		[SerializeField] private float ballMass;            //�{�[���̎��ʁB
		private float courtBounciness;                      //�R�[�g�̔����W���B
		private float courtMaxY;                            //�R�[�g�̏�ʁB
		private float ballHalfSize;							//�{�[���̃T�C�Y�B

		[SerializeField]private int dummyCount;			//�e���\���̓_�̐�
		[SerializeField]private float secInterval;      //�e����\������Ԋu�̕b��
		[SerializeField] private float DummyGraceDist;  //��\�������B

		[SerializeField] private float offsetSpeed;
		private float offset = 0.0f;
		//�_�~�[�\���Ɏg���I�u�W�F�N�g�̃��X�g�B
		private List<GameObject> dummySphereList = new List<GameObject>();

		private TeamFlowScript TeamFlow;
		private BallFlowScript jackFlow;
		public Material ballCol;
        [SerializeField] private Color[] ballColList = new Color[3] {
			new Color(1.0f,1.0f,1.0f,0.5f),
			new Color(1.0f,0.2f,0.2f,0.5f),
			new Color(0.2f,0.2f,1.0f,0.5f)
		};



		//�V���O���g���B
		static private ThrowDummyScript instance;

		//�C���X�^���X���擾�B
		static public ThrowDummyScript Instance()
        {
			return instance;
        }

		private void Awake()
		{
			if(instance != null)
            {
				Destroy(this.gameObject);
				Debug.LogWarning("ThrowDummyScript�������쐬����Ă���B");
				return;
			}
			instance = this;

			dummyObjParent.transform.position = transform.position;

			//�e���\����\�����邽�߂̓_�𐶐�
			for (int i = 0; i < dummyCount; i++)
			{
				var obj = (GameObject)Instantiate(dummyObjPref, dummyObjParent);
				dummySphereList.Add(obj);
			}

			var gameFlow = GameObject.FindGameObjectWithTag("GameFlow");
			if (gameFlow)
			{
				//�{�[���Ǘ��̃X�N���v�g�B
				TeamFlow = gameFlow.GetComponent<TeamFlowScript>();
				jackFlow = gameFlow.GetComponent<BallFlowScript>();
			}

			//�{�[���̔��a���擾�B
			ballHalfSize = dummyObjPref.transform.localScale.y / 2.0f;

			//�v���t�@�u���烊�W�b�h�{�f�B�擾�B
			var RB = ballPrefab.GetComponent<Rigidbody>();
			ballMass = RB.mass;     //���ʂ��擾�B
			
			//�v���t�@�u���畨���}�e���A���擾�B
			courtBounciness = courtPhysicMat.bounciness;

			//�n�ʂ̍������擾�B
			courtMaxY = courtBoxCol.bounds.max.y;
		}
		private void OnDestroy()
		{
			// �j�����ɁA�o�^�������̂̉������s��
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

		//�S���Ń_�~�[��\���B
		void DummyGround()
		{
			//�e���\���̈ʒu�ɓ_���ړ�
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
		//�o�E���X�����Ń_�~�[��\���B
		void DummyNoBounce()
		{
			//�e���\���̈ʒu�ɓ_���ړ�
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
		//�o�E���X����Ń_�~�[��\���B
		void DummyBounce()
		{
			Vector3 pos = transform.position;
			Vector3 oldPos = transform.position;
			Vector3 vecForce = force;
			int count = 0;
			float BounceOffset = 0.0f;

			while(count < dummyCount)
			{
				//�e���\���̈ʒu�ɓ_���ړ�
				for (int i = 0; count < dummyCount; i++)
				{
					Vector3 vec = new Vector3();
					var t = ((i + 1) * secInterval) + offset + BounceOffset;
					vec = (vecForce * t) + (0.5f * Physics.gravity * Mathf.Pow(t, 2.0f));
					if (vec.magnitude < DummyGraceDist
						&& pos == transform.position)
					{
						dummySphereList[count].SetActive(false);
						oldPos = vec + pos;       //�L�^���Ă����B
					}
					else
					{
						dummySphereList[count].SetActive(true);
						Vector3 resPos = pos + vec;
						//�R�[�g�̏�ɂ���B
						if (resPos.y > courtMaxY)
						{
							oldPos = resPos;       //�L�^���Ă����B
							resPos.y += ballHalfSize;
							dummySphereList[count].transform.position = resPos;
						}
						//�R�[�g�̉��ɂ���B
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
							oldPos = resPos;       //�L�^���Ă����B
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
		/// �n�_���Z�b�g�B
		/// </summary>
		public void SetPosition(Vector3 _pos)
        {
			transform.position = _pos;
		}
		/// <summary>
		/// �͂��Z�b�g�B
		/// </summary>
		public void SetForce(Vector3 _force)
        {
			force = _force / ballMass * Time.fixedDeltaTime;
        }

        public void OnEnable()
        {
			//�I�u�W�F�N�g���L���ɂȂ����B
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
			//�I�u�W�F�N�g���L���ɂȂ����B
			for (int i = 0; i < dummyCount; i++)
			{
				dummySphereList[i].SetActive(false);
			}
		}
    }
}
