using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraMove2D : MonoBehaviour
{
    public Collider collider;

    public TouchManager touchManager;   //�^�b�`�}�l�[�W���[�B
    public Slider slider;           //�J�����̊g�嗦�̃X���C�_�[�B

    public float moveSpeed = 2.0f;    //�J�����̃X�s�[�h�B
    Camera camera;                  //�J�����B
    Vector3 position;               //���W�B
    Vector3 posMax;                 //�R�[�g�̍ő���W�B
    Vector3 posMin;                 //�R�[�g�̍ŏ����W�B
    Vector3 boxSize;
    float ScreenAspect;             //�A�X�y�N�g��B
    bool isMove = false;            //���������ǂ����B

    // Start is called before the first frame update
    void Start()
    {
        camera = GetComponent<Camera>();
        position = camera.transform.position;

        posMax = collider.bounds.max;
        posMin = collider.bounds.min;
        boxSize = posMax - posMin;
        ScreenAspect = (float)Screen.height / Screen.width;
    }

    // Update is called once per frame
    void Update()
    {
        CameraMove();       //�J�����̈ړ��B
        CameraPosLimit();   //�J�����̈ʒu�𐧌��B
    }

    void CameraMove()
    {
        isMove = false;
        if (touchManager.IsTouch())
        {
            if(touchManager.GetPhase() == TouchPhase.Moved)
            {
                float camSpeed = camera.orthographicSize;

                position.x += touchManager.GetDeltaPosInScreen().x * -moveSpeed * camSpeed;
                position.z += touchManager.GetDeltaPosInScreen().y * -moveSpeed * camSpeed;

                isMove = true;
            }
        }

    }

    void CameraPosLimit()
    {
        float cameraCenterY = camera.orthographicSize;
        float cameraCenterX = cameraCenterY / ScreenAspect;
        if (boxSize.x > cameraCenterX * 2.0f
            && boxSize.z > cameraCenterY * 2.0f)
        {
            position.x = Mathf.Clamp(position.x, posMin.x + cameraCenterX, posMax.x - cameraCenterX);
            position.z = Mathf.Clamp(position.z, posMin.z + cameraCenterY, posMax.z - cameraCenterY);
        }
        else
        {
            position.x = collider.transform.position.x;
            position.z = collider.transform.position.z;
        }

        camera.transform.position = position;
    }

    public bool IsMoved()
    {
        return isMove;
    }
}
