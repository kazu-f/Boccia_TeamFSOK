using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PaddleScript : MonoBehaviour
{
    private Image image = null;
    private RectTransform rect = null;
    //�X�v���C�g
    [SerializeField] private Sprite RedPaddleSprite;
    [SerializeField] private Sprite BluePaddleSprite;
    private Vector3 DefaultScale = Vector3.zero;       //�n�߂̕��ƍ���
    [SerializeField] private Vector3 LastScale;        //�⊮��̕��ƍ���
    private float late = 0.0f;      //�⊮��
    [SerializeField] private float LerpSpeed = 1.0f;        //�⊮���x
    [SerializeField] private float StopTime = 1.0f;     //��~���Ă��鎞��
    private float NowTime = 0.0f;       //���݂̎���
    private bool IsMove = false;        //�������ǂ����̃t���O
    private void Awake()
    {
        image = this.gameObject.GetComponent<Image>();
        rect = this.gameObject.GetComponent<RectTransform>();
        DefaultScale = rect.localScale;
    }
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        LerpPos();
    }

    /// <summary>
    /// ���ɓ�����`�[�����Z�b�g���ăX�v���C�g��؂�ւ�
    /// </summary>
    /// <param name="team">���ɓ�����`�[��</param>
    public void SetTeam(Team team)
    {
        switch(team)
        {
            case Team.Red:
                image.sprite = RedPaddleSprite;
                break;
            case Team.Blue:
                image.sprite = BluePaddleSprite;
                break;
        }
        SetDefault();
    }

    /// <summary>
    /// �f�t�H���g�ɃZ�b�g
    /// </summary>
    private void SetDefault()
    {
        rect.localScale = DefaultScale;
        late = 0.0f;
        IsMove = false;
    }

    private void LerpPos()
    {
        if (!IsMove)
        {
            NowTime += Time.deltaTime;
            if (NowTime > StopTime)
            {
                IsMove = true;
                NowTime = 0.0f;
            }
            return;
        }
        if (late < 1.0f)
        {
            //�⊮�������Z
            late += Time.deltaTime * LerpSpeed;
            late = Mathf.Min(late, 1.0f);
            rect.localScale = (late * LastScale) + ((1.0f - late) * DefaultScale);
        }
    }
}
