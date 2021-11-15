using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FailedMoveScript : MonoBehaviour
{
    private Vector3 pos= new Vector3(0.0f, 60.0f, 0.0f );
    private RectTransform rect = null;

    private float Alpha = 0.0f;
    public float SubAlpha;

    private GameObject spriteObject;
    // Start is called before the first frame update
    void Start()
    {
        rect = this.gameObject.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        //�X�v���C�g�̈ړ�//////
        pos.y -= 1.0f;
        pos.y = Mathf.Max(pos.y, 20.0f);
        this.transform.position.Set(0.0f,pos.y,0.0f);
        rect.localPosition = pos;
        ///////////////////////

        ///�X�v���C�g�̓����x//////
        Alpha -= Time.deltaTime * SubAlpha;
        if (GameObject.Find("FailedSprite") != null)
        {
            spriteObject = GameObject.Find("FailedSprite");
        }
        Alpha = Mathf.Max(Alpha, 0.0f);
        spriteObject.GetComponent<SpriteRenderer>().color = new Color(0.0f, 0.0f, 0.0f, Alpha);

        
    }
}
