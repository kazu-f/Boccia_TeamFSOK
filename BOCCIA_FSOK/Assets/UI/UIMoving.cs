using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// UIを動かすスクリプト。
/// </summary>
public class UIMoving : MonoBehaviour
{
    [SerializeField] private Vector2 MoveDirection = Vector2.one;
    [SerializeField] private float MoveDist = 10.0f;                    //UIを動かす割合。UIのサイズが基準。
    RectTransform rectTransform;
    private Vector3 UIPos = Vector3.zero;
    private Vector3 basePos = Vector3.zero;
    private Vector2 rectSize = Vector2.zero;
    // Start is called before the first frame update
    void Start()
    {
        rectTransform = this.gameObject.GetComponent<RectTransform>();
        basePos = rectTransform.localPosition;
        rectSize = rectTransform.sizeDelta;
    }

    // Update is called once per frame
    void Update()
    {
        float t = Mathf.Sin(Time.time);
        t = Mathf.Abs(t);

        UIPos.x = basePos.x + ((MoveDist / 100.0f) * rectSize.x * t * MoveDirection.x);
        UIPos.y = basePos.y + ((MoveDist / 100.0f) * rectSize.y * t * MoveDirection.y);

        rectTransform.localPosition = UIPos;
    }
}
