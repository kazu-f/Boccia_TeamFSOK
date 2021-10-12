using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThrowAngleScript : MonoBehaviour
{
    private Material m_gaugeImageMat;

    private void Awake()
    {
        var image = this.gameObject.GetComponent<Image>();
        m_gaugeImageMat = image.material;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
