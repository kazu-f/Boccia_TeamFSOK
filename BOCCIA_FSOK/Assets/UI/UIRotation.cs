using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//UIを回し続けるだけのスクリプト。
public class UIRotation : MonoBehaviour
{
    private RectTransform rectTransform = null;     //このオブジェクトのRect。
    private Vector3 currentRotEuler = new Vector3(0.0f,0.0f,0.0f);
    private Vector3 rotVector = Vector3.zero;       //回転させる軸。

    [Tooltip("UIを回転させる速度。(秒間 X°)")]
    [SerializeField] private float rotationSpeed = 90.0f;
    [SerializeField] private bool isUseAxisX = false;        //X軸に回すか。
    [SerializeField] private bool isUseAxisY = false;        //Y軸に回すか。
    [SerializeField] private bool isUseAxisZ = true;         //Z軸に回すか。


    // Start is called before the first frame update
    void Start()
    {
        //このオブジェクトのRectを取得。
        rectTransform = this.gameObject.GetComponent<RectTransform>();    
        //Rectが取得できたか？
        if(rectTransform == null)
        {
            Debug.LogError("RectTransformが存在していません。");
            //Rectが取得できない場合このスクリプトを無効化する。
            this.enabled = false;
        }
        else
        {
            currentRotEuler = rectTransform.localEulerAngles;
        }

        //回転軸を定める。
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
        //回転を掛ける。
        currentRotEuler += rotVector * rotationSpeed * Time.deltaTime;
        //回転を反映。
        rectTransform.localEulerAngles = currentRotEuler;
    }
}
