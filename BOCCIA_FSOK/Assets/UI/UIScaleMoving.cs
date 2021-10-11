using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIScaleMoving : MonoBehaviour
{
    private RectTransform rect = null;
    public float moveTime = 1.0f;       //������t���銴�o�B
    public float minScale = 0.8f;       //��ԏ��������_�ł̃X�P�[���B
    public float maxScale = 1.0f;       //��ԑ傫�����_�ł̃X�P�[���B

    private float currentTime = 0.0f;   //�o�ߎ��ԁB
    private Vector3 vScale = new Vector3();  //�X�P�[���B
    private float scale = 1.0f;         //�X�P�[���B

    // Start is called before the first frame update
    void Start()
    {
        rect = this.gameObject.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if(rect == null)
        {
            return;
        }
        //�X�P�[�����v�Z����(0.0�`1.0�̊Ԃ̒l�����ԂŎ��A�ŏ��`�ő�Ő��`��ԁB)
        currentTime += Time.deltaTime;
        float t = (currentTime / moveTime);
        float weight = Mathf.Abs(t - 2 * Mathf.Floor(t / 2.0f) - 1.0f);
        scale = Mathf.Lerp(minScale, maxScale, weight);
        //�X�P�[����ݒ�B
        vScale.Set(scale, scale, scale);
        //�e�L�X�g�̃X�P�[����ύX�B
        rect.localScale = vScale;
    }
}
