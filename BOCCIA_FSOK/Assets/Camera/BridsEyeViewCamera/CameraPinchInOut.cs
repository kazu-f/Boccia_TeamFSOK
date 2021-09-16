using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraPinchInOut : MonoBehaviour
{
    public float maxOrthographicSize = 6.0f;
    public float minOrthographicSize = 0.1f;
    public Slider slider;
    Camera camera;
    //float m_orthographicSize;
    //float oldSize = 0.0f;
    float oldDist = 0.0f;
    float currentScale = 0.5f;
    float scale = 0.0f;

    float m_time = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        camera = this.GetComponent<Camera>();
        //m_orthographicSize = camera.orthographicSize;
        currentScale = camera.orthographicSize / maxOrthographicSize;
        slider.value = currentScale;
    }

    // Update is called once per frame
    void Update()
    {
        TouchPinchInOut();
    }

    void TouchPinchInOut()
    {
        //2点タッチの時のみ処理を行う。
        if (Input.touchCount == 2)
        {
            // タッチしている２点を取得
            Touch touch1 = Input.GetTouch(0);
            Touch touch2 = Input.GetTouch(1);

            // 2点タッチ開始時の距離を記憶
            if (touch2.phase == TouchPhase.Began)
            {
                oldDist = Vector2.Distance(touch1.position, touch2.position);
            }
            else if (touch1.phase == TouchPhase.Moved && touch2.phase == TouchPhase.Moved)
            {
                // タッチ位置の移動後、長さを再測し、移動前の距離からの相対値を取る。
                float newDist = Vector2.Distance(touch1.position, touch2.position);
                scale = (newDist - oldDist) / 100.0f;
                oldDist = newDist;

                // 相対値が変更した場合、オブジェクトに相対値を反映させる
                if (scale != 0)
                {
                    currentScale += scale;
                    currentScale = Mathf.Min(1.0f, Mathf.Max(0.0f, currentScale));
                    slider.value = currentScale;
                    UpdateCamScaling(currentScale);
                }
            }
        }

    }

    //スライダーを変更したときに呼ばれる関数。
    public void SliderPinchInOut()
    {
        currentScale = slider.value;
        UpdateCamScaling(currentScale);
    }

    void UpdateCamScaling(float scale)
    {
        //線形補間でサイズを決める。
        camera.orthographicSize = Mathf.Lerp(maxOrthographicSize, minOrthographicSize, scale); 
    }
}
