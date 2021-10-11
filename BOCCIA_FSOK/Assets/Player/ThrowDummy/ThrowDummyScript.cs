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
		private Vector3 oldForce;	//�O�t���[���̃x�N�g���B

		[SerializeField]private int dummyCount;			//�e���\���̓_�̐�

		[SerializeField]private float secInterval;      //�e����\������Ԋu�̕b��

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
			//�X�V���Ȃ���Ώ������Ȃ��B
			if (force == oldForce) return;
			//�e���\���̈ʒu�ɓ_���ړ�
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
			force = _force;
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
