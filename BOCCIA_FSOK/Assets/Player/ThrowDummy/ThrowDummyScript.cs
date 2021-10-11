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

		private List<GameObject> dummySphereList = new List<GameObject>();

        private void Awake()
		{
			dummyObjParent.transform.position = transform.position;

			//�e���\����\�����邽�߂̓_�𐶐�
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
			////�X�V���Ȃ���Ώ������Ȃ��B
			//if (force == oldForce) return;
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

			//oldForce = force;
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
