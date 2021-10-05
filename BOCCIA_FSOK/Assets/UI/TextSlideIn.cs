using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextSlideIn : MonoBehaviour
{
    public AnimationCurve animCurve = AnimationCurve.Linear(0, 0, 1, 1);
    public Vector3 inPosition;        // �X���C�h�C����̈ʒu
    public Vector3 outPosition;      // �X���C�h�A�E�g��̈ʒu
    public float duration = 1.0f;    // �X���C�h���ԁi�b�j
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    // �X���C�h�C���iPause�{�^���������ꂽ�Ƃ��ɁA������Ăԁj
    public void SlideIn()
    {
        StartCoroutine(StartSlidePanel(true));
    }

    // �X���C�h�A�E�g
    public void SlideOut()
    {
        StartCoroutine(StartSlidePanel(false));
    }

    private IEnumerator StartSlidePanel(bool isSlideIn)
    {
        float startTime = Time.time;    // �J�n����
        Vector3 startPos = transform.localPosition;  // �J�n�ʒu
        Vector3 moveDistance;            // �ړ���������ѕ���

        if (isSlideIn)
            moveDistance = (inPosition - startPos);
        else
        {
            moveDistance = (outPosition - startPos);

            while ((Time.time - startTime) < duration)
            {
                transform.localPosition = startPos + moveDistance * animCurve.Evaluate((Time.time - startTime) / duration);
                yield return 0;        // 1�t���[����A�ĊJ
            }
            transform.localPosition = startPos + moveDistance;
        }
    }
}
