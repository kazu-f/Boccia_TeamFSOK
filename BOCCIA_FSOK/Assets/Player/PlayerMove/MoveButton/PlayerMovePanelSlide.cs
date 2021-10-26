using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovePanelSlide : MonoBehaviour
{
    [SerializeField]private AnimationCurve animCurve;
    [SerializeField] private float duration = 1.0f;    // �X���C�h���ԁi�b�j
    private RectTransform rectTransform;    //UI��Rect�B
    private Vector2 inPivot = new Vector2(0.0f,0.5f);                //�X���C�h�C����̃s�{�b�g�ʒu
    private Vector2 outPivot = new Vector2(1.0f, 0.5f);               //�X���C�h�A�E�g��̃s�{�b�g�ʒu
    private bool isMoving = false;              //�X���C�h�����B
    private bool isInitedUI = false;            //�C�����Ă����Ԃ��H

    private void Awake()
    {
        rectTransform = this.gameObject.GetComponent<RectTransform>();
    }

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
        Vector2 startPos = rectTransform.pivot;  // �J�n�ʒu
        Vector2 moveDistance;            // �ړ���������ѕ���

        isMoving = true;
        isInitedUI = false;

        if (isSlideIn)
        {
            moveDistance = (inPivot - startPos);

            while ((Time.time - startTime) < duration)
            {
                rectTransform.pivot = startPos + moveDistance * animCurve.Evaluate((Time.time - startTime) / duration);
                yield return 0;        // 1�t���[����A�ĊJ
            }
            //�X���C�h�C�����I������B
            isInitedUI = true;
        }
        else
        {
            moveDistance = (outPivot - startPos);

            while ((Time.time - startTime) < duration)
            {
                rectTransform.pivot = startPos + moveDistance * animCurve.Evaluate((Time.time - startTime) / duration);
                yield return 0;        // 1�t���[����A�ĊJ
            }
        }
        rectTransform.pivot = startPos + moveDistance;

        isMoving = false;
    }

    /// <summary>
    /// �X���C�h�����ǂ����B
    /// </summary>
    /// <returns></returns>
    public bool IsMoving()
    {
        return isMoving;
    }
    /// <summary>
    /// �X���C�h�C��������̏�Ԃ��B
    /// </summary>
    /// <returns></returns>
    public bool IsInitedUI()
    {
        return isInitedUI;
    }
}
