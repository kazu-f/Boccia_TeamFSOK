using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmScript : MonoBehaviour
{
    private void Awake()
    {

    }
    // Start is called before the first frame update
    void Start()
    {
        HoldDown();
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// è„Ç…ç\Ç¶ÇÈ
    /// </summary>
    public void HoldUp()
    {
        this.transform.localEulerAngles = new Vector3(-45.0f, 0.0f, 0.0f);
    }

    /// <summary>
    /// â∫Ç…ç\Ç¶ÇÈ
    /// </summary>
    public void HoldDown()
    {
        this.transform.localEulerAngles = new Vector3(0.0f, 0.0f, 180.0f);
    }
}
