using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetCameraScript : MonoBehaviour
{
    [SerializeField] GameObject maincamera;
    private Transform cameratrans;

    private void Awake()
    {
        cameratrans = maincamera.GetComponent<Transform>();
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        this.gameObject.transform.position = cameratrans.position;
        this.gameObject.transform.rotation = cameratrans.rotation;
    }

    /// <summary>
    /// �J�����̃Z�b�g
    /// </summary>
    /// <param name="camera">�g�����X�t�H�[��</param>
    public void SetTrans(Transform trans)
    {
        cameratrans = trans;
    }
}
