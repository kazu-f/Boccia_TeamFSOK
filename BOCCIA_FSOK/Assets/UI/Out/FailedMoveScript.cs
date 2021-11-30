using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FailedMoveScript : MonoBehaviour
{
    private Vector3 m_Pos = new Vector3(0.0f, 60.0f, 0.0f);
    private Vector3 m_DefaultPos ;
    private RectTransform m_Rect = null;

    private float m_Alpha = 0.0f;
    public float m_SubAlpha;

    [SerializeField] private Image m_Image;

    bool Direct = false;
    // Start is called before the first frame update
    void Start()
    {
        m_DefaultPos = m_Pos;
        FontAlphaZero();
        m_Rect = this.gameObject.GetComponent<RectTransform>();
    }


    // Update is called once per frame
    void Update()
    {
        if (Direct)
        {
            FailedOut();
        }
        var n = m_Image.color;
        m_Image.color = new Color(n.r, n.g, n.b, m_Alpha);
    }
    private void SetAlpha()
    {
        var n = m_Image.color;
        m_Image.color = new Color(n.r, n.g, n.b, m_Alpha);
    }
    public void SetDirect()
    {
        Debug.LogError("場外");
        Direct = true;
    }
    public void FailedOut()
    {
        //スプライトの移動//////
        m_Pos.y -= 1.0f;
        m_Pos.y = Mathf.Max(m_Pos.y, 20.0f);
        this.transform.position.Set(0.0f, m_Pos.y, 0.0f);
        m_Rect.localPosition = m_Pos;
        ///////////////////////

        ///スプライトの透明度//////
        m_Alpha += Time.deltaTime * m_SubAlpha;
        m_Alpha = Mathf.Min(m_Alpha, 1.0f);

        SetAlpha();
    }

    public void FontAlphaZero()
    {
        Debug.LogError("消した");
        m_Pos = m_DefaultPos;
        m_Alpha = 0.0f;
        Direct = false;
    }
}
