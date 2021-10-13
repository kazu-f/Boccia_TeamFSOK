using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwichActiveGameObjects : MonoBehaviour
{
    public GameObject[] gameObjects;    //�{�[���������Ă���Ԏ~�߂�I�u�W�F�N�g�����B

    private static SwichActiveGameObjects instance = null;        //�C���X�^���X�ϐ��B
    public static SwichActiveGameObjects GetInstance()
    {
        if (instance == null)
        {
            Debug.LogError("SwichActiveGameObjects����������Ă��Ȃ��B");
        }
        return instance;
    }

    private void Awake()
    {
        // �����C���X�^���X�����݂���Ȃ�A�����j������
        if (instance != null)
        {
            Destroy(this.gameObject);
            return;
        }

        instance = this;
    }

    private void OnDestroy()
    {
        // �j�����ɁA�o�^�������̂̉������s��
        if (this == instance) instance = null;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    /// <summary>
    /// �Q�[���I�u�W�F�N�g�̃A�N�e�B�u��ύX�B
    /// </summary>
    public void SwitchGameObject(bool isActive)
    {
        if (!(gameObjects.Length > 0))
        {
            Debug.Log("�Q�[���I�u�W�F�N�g���o�^����Ă��Ȃ��B");
            return;
        }

        //if(isActive)
        //{
        //    Debug.Log("�A�N�e�B�u�ɂ���B");
        //}
        //else
        //{
        //    Debug.Log("�A�N�e�B�u�����B");
        //}


        foreach (var obj in gameObjects)
        {
            obj.gameObject.SetActive(isActive);
        }
    }
}
