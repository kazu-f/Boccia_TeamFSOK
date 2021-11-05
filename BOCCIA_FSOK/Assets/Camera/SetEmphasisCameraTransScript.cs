using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetEmphasisCameraTransScript : MonoBehaviour
{
    private GameObject EmphasisCamera = null;
    private bool IsOrtho;
    private void Awake()
    {
        EmphasisCamera = GameObject.Find("EmphasisCamera");
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }

    private void LateUpdate()
    {
        EmphasisCamera.gameObject.transform.position = this.gameObject.transform.position;
        EmphasisCamera.gameObject.transform.rotation = this.gameObject.transform.rotation;
        if (IsOrtho)
        {
            //�����ϊ��̎�
            EmphasisCamera.GetComponent<Camera>().orthographicSize = this.gameObject.GetComponent<Camera>().orthographicSize;
        }
        else
        {
            //���s���e�̎�
            EmphasisCamera.GetComponent<Camera>().fieldOfView = this.gameObject.GetComponent<Camera>().fieldOfView;
        }

    }
    private void OnEnable()
    {
        IsOrtho = this.gameObject.GetComponent<Camera>().orthographic;
        //�����ϊ����ǂ���
        EmphasisCamera.GetComponent<Camera>().orthographic = IsOrtho;
    }
}
