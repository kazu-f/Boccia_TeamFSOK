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
    private Vector2 startPos = new Vector2(0.0f, 0.0f); //�X���C�h�J�n�n�_�B
    private Vector2 moveDistance;            // �ړ���������ѕ���
    float startTime = 0.0f;                     //�J�n���ԁB
    private bool isMoving = false;              //�X���C�h�����B
    private bool isInitedUI = false;            //�C�����Ă����Ԃ��H
    private bool isExecuteSlideIn = false;             //�X���C�h�C�������Ă��邩�H

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
        ExecuteSlide();
    }

    // �X���C�h�C�����J�n����B
    public void SlideIn()
    {
        StartSlidePanel(true);
    }

    // �X���C�h�A�E�g���J�n����B
    public void SlideOut()
    {
        StartSlidePanel(false);
    }

    private void StartSlidePanel(bool isSlideIn)
    {
        startTime = Time.time;    // �J�n����
        startPos = rectTransform.pivot;  // �J�n�ʒu

        isMoving = true;                //�X���C�h���n�߂�B
        isInitedUI = false;             //UI�������Ă���B
        isExecuteSlideIn = isSlideIn;   //�X���C�h�C�������s���Ă��邩�B

        if (isSlideIn)
        {
            moveDistance = (inPivot - startPos);
        }
        else
        {
            moveDistance = (outPivot - startPos);
        }
    }

    private void ExecuteSlide()
    {
        //�X���C�h���Ă��Ȃ��Ȃ珈�����Ȃ��B
        if (!isMoving) return;

        //�X���C�h���Ԃɓ��B���Ă��Ȃ��B�B
        if ((Time.time - startTime) < duration)
        {
            rectTransform.pivot = startPos + moveDistance * animCurve.Evaluate((Time.time - startTime) / duration);
        }
        else
        {
            rectTransform.pivot = startPos + moveDistance;

            //�X���C�h���I������B
            isMoving = false;
            //�X���C�h�C�����I��������H
            isInitedUI = isExecuteSlideIn;
        }
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

    private void OnEnable()
    {
        //�L���ɂȂ����u�Ԃɂ��X���C�h�����s���Ă����B
        ExecuteSlide();
    }
}
