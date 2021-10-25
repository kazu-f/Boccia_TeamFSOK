using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BocciaPlayer
{
	public class ThrowDummyScript : MonoBehaviour
	{
		[SerializeField]private GameObject dummyObjPref;        //�e����\�����邽�߂̓_�̃v���n�u
		[SerializeField]private Transform dummyObjParent;       //�e����\������_�̐e�I�u�W�F�N�g

		private Vector3 force;      //�����̃x�N�g���B
		[SerializeField] private float mass;			//���ʁB

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
				TeamFlow = gameFlow.GetComponent<TeamFlowScript>();
				jackFlow = gameFlow.GetComponent<BallFlowScript>();
			}
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
					dummySphereList[i].transform.position = vec;						
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
			force = _force / mass * Time.fixedDeltaTime;
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
