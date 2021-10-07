using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISlideIn : MonoBehaviour
{
    public AnimationCurve animCurve = AnimationCurve.Linear(0, 0, 1, 1);    //�A�j���[�V�����J�[�u�B
    [SerializeField]private Vector3 direction;           //�X���C�h�����B
    [SerializeField] private float duration = 1.0f;    // �X���C�h���ԁi�b�j
    private RectTransform rect;    // UI��Rect
    private Vector3 inPosition;        // �X���C�h�C����̈ʒu
    private Vector3 outPosition;      // �X���C�h�A�E�g��̈ʒu
    private bool isMoving = false;              //�X���C�h�����B
    private bool isInited = false;              //���W�ݒ�ς݂��H
    // Start is called before the first frame update
    void Start()
    {
        rect = this.gameObject.GetComponent<RectTransform>();
        Vector3 dir = direction.normalized;
        dir.x *= Screen.width + rect.sizeDelta.x;
        dir.z *= Screen.height + rect.sizeDelta.y;

        inPosition = transform.localPosition;           //�X���C�h�C����̈ʒu�B
        outPosition = transform.localPosition + dir;    //�X���C�h�A�E�g��̈ʒu�B
        transform.localPosition = transform.localPosition - dir;

        isInited = true;
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

        isMoving = true;

        if (isSlideIn)
        {
            moveDistance = (inPosition - startPos);

            while ((Time.time - startTime) < duration)
            {
                transform.localPosition = startPos + moveDistance * animCurve.Evaluate((Time.time - startTime) / duration);
                yield return 0;        // 1�t���[����A�ĊJ
            }
        }
        else
        {
            moveDistance = (outPosition - startPos);

            while ((Time.time - startTime) < duration)
            {
                transform.localPosition = startPos + moveDistance * animCurve.Evaluate((Time.time - startTime) / duration);
                yield return 0;        // 1�t���[����A�ĊJ
            }
        }
        transform.localPosition = startPos + moveDistance;

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
    /// ���W�ݒ�ς݂��H
    /// </summary>
    /// <returns></returns>
    public bool IsInited()
    {
        return isInited;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, direction.normalized * 10f);
        Gizmos.DrawCube(transform.position + direction.normalized * 10.0f, Vector3.one * 3.0f);
    }
}
