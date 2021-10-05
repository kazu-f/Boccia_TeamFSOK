using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextScaleMoving : MonoBehaviour
{
    private Text text = null;
    public float moveTime = 1.0f;       //������t���銴�o�B
    public float minScale = 0.8f;       //��ԏ��������_�ł̃X�P�[���B
    public float maxScale = 1.0f;       //��ԑ傫�����_�ł̃X�P�[���B

    private float currentTime = 0.0f;   //�o�ߎ��ԁB
    private Vector3 textScale = new Vector3();  //�e�L�X�g�̃X�P�[���B
    private float scale = 1.0f;         //�X�P�[���B

    // Start is called before the first frame update
    void Start()
    {
        text = this.gameObject.GetComponent<Text>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if(text == null)
        {
            return;
        }
        //�X�P�[�����v�Z����(0.0�`1.0�̊Ԃ̒l�����ԂŎ��A�ŏ��`�ő�Ő��`��ԁB)
        currentTime += Time.deltaTime;
        float t = (currentTime / moveTime);
        float weight = Mathf.Abs(t - 2 * Mathf.Floor(t / 2.0f) - 1.0f);
        scale = Mathf.Lerp(minScale, maxScale, weight);
        //�X�P�[����ݒ�B
        textScale.Set(scale, scale, scale);
        //�e�L�X�g�̃X�P�[����ύX�B
        text.rectTransform.localScale = textScale;
    }
}
