using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackGroundTextScript : MonoBehaviour
{
    [SerializeField] private BackGroundScript m_BackGround;
    [SerializeField] private Text m_text;
    private void Awake()
    {
        m_BackGround = GameObject.Find("BackGround").GetComponent<BackGroundScript>();
    }
    // Start is called before the first frame update
    void Start()
    {
        m_text.text = m_BackGround.GetSprite().name;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
