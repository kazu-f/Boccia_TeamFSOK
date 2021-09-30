using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpriteMovement : MonoBehaviour
{
    RectTransform rect;
    public float MovementTime = 3.0f;
    public float MovementLength = 20.0f;
    float time = 0.0f;
    float startPosY;
    Vector3 position;

    // Start is called before the first frame update
    void Start()
    {
        //�X�v���C�g
        rect = this.transform as RectTransform;
        startPosY = rect.localPosition.y;
        position = rect.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        //���Ԍo�߁B
        time += Time.deltaTime;

        //�������B
        Movement();
    }

    void Movement()
    {
        //float t = (time / MovementTime);
        //float weight = Mathf.Abs(t - 2 * Mathf.Floor(t / 2.0f) - 1.0f);

        //Sin�g��0.0�`1.0�͈̔͂ɂ���B
        float t = Mathf.Sin(time / MovementTime) / 2.0f + 0.5f;

        float posY = startPosY + MovementLength * t;

        position.y = posY;
        rect.localPosition = position;
    }
}
