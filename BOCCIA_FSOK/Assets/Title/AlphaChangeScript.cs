using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlphaChangeScript : MonoBehaviour
{
    Text text;
    Sprite sprite;
    bool isText = true;

    public float ChangeTime = 2.0f;
    float time = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        //�e�L�X�g���擾�B
        text = GetComponent<Text>();
        //�e�L�X�g���擾�ł��Ȃ���΁B
        if(text == null)
        {
            sprite = GetComponent<Sprite>();
            isText = false;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
