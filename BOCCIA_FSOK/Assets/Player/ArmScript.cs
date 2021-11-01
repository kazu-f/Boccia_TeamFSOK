using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmScript : MonoBehaviour
{
    [SerializeField] private Transform throwTransform;
    private Vector3 basePos = Vector3.zero;
    private Vector3 currentPos = Vector3.zero;
    private float fluctuation = 0.0f;       //揺らぎ変数。
    const float WEIGHT = 0.02f;
    private void Awake()
    {
        basePos = this.gameObject.transform.localPosition;
    }
    // Start is called before the first frame update
    void Start()
    {
        HoldDown();
    }

    // Update is called once per frame
    void Update()
    {
        float time = Time.time;
        currentPos = basePos;
        fluctuation = Mathf.Sin(time) * WEIGHT;
        currentPos.y += fluctuation;
        this.gameObject.transform.localPosition = currentPos;
    }

    /// <summary>
    /// 上に構える
    /// </summary>
    public void HoldUp()
    {
        this.transform.localEulerAngles = new Vector3(-45.0f, 0.0f, 0.0f);
    }

    /// <summary>
    /// 下に構える
    /// </summary>
    public void HoldDown()
    {
        this.transform.localEulerAngles = new Vector3(0.0f, 0.0f, 180.0f);
    }
    /// <summary>
    /// 揺らぎを取得。
    /// </summary>
    /// <returns>揺らぎ変数。</returns>
    public float GetFluctuation()
    {
        return fluctuation;
    }
    /// <summary>
    /// 腕の座標を取得。
    /// </summary>
    /// <returns>座標</returns>
    public Vector3 GetPosition()
    {
        return throwTransform.position;
    }
}
