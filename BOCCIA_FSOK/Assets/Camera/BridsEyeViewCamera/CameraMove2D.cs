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
    bool isMove = false;            //���������ǂ����B

    // Start is called before the first frame update
    void Start()
    {
        camera = GetComponent<Camera>();
        position = camera.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        CameraMove();
    }

    void CameraMove()
    {
        isMove = false;
        if (touchManager.IsTouch())
        {
            if(touchManager.GetPhase() == TouchPhase.Moved)
            {
                position.x += touchManager.GetDeltaPosInScreen().x * -moveSpeed;
                position.z += touchManager.GetDeltaPosInScreen().y * -moveSpeed;

                camera.transform.position = position;


                isMove = true;
            }
        }

    }
}
