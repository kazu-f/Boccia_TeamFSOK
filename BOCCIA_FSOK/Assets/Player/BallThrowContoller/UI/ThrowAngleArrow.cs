using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowAngleArrow : MonoBehaviour
{
    private static ThrowAngleArrow instance = null;        //�C���X�^���X�ϐ��B
    public static ThrowAngleArrow GetInstance()
    {
        if (instance == null)
        {
            Debug.LogError("ThrowGaugeScript����������Ă��Ȃ��B");
        }
        return instance;
    }
    //�V���O���g���B
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
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
