using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//UI���񂵑����邾���̃X�N���v�g�B
public class UIRotation : MonoBehaviour
{
    private RectTransform rectTransform = null;     //���̃I�u�W�F�N�g��Rect�B
    private Vector3 currentRotEuler = new Vector3(0.0f,0.0f,0.0f);
    private Vector3 rotVector = Vector3.zero;       //��]�����鎲�B

    [Tooltip("UI����]�����鑬�x�B(�b�� X��)")]
    [SerializeField] private float rotationSpeed = 90.0f;
    [SerializeField] private bool isUseAxisX = false;        //X���ɉ񂷂��B
    [SerializeField] private bool isUseAxisY = false;        //Y���ɉ񂷂��B
    [SerializeField] private bool isUseAxisZ = true;         //Z���ɉ񂷂��B


    // Start is called before the first frame update
    void Start()
    {
        //���̃I�u�W�F�N�g��Rect���擾�B
        rectTransform = this.gameObject.GetComponent<RectTransform>();    
        //Rect���擾�ł������H
        if(rectTransform == null)
        {
            Debug.LogError("RectTransform�����݂��Ă��܂���B");
            //Rect���擾�ł��Ȃ��ꍇ���̃X�N���v�g�𖳌�������B
            this.enabled = false;
        }
        else
        {
            currentRotEuler = rectTransform.localEulerAngles;
        }

        //��]�����߂�B
        if(isUseAxisX)
        {
            rotVector.x = 1.0f;
        }
        if(isUseAxisY)
        {
            rotVector.y = 1.0f;
        }
        if(isUseAxisZ)
        {
            rotVector.z = 1.0f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //��]���|����B
        currentRotEuler += rotVector * rotationSpeed * Time.deltaTime;
        //��]�𔽉f�B
        rectTransform.localEulerAngles = currentRotEuler;
    }
}
