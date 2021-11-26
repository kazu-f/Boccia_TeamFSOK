using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackGroundScript : MonoBehaviour
{
    [SerializeField] private Sprite[] m_sprite;
    [SerializeField] private Image m_image;
    int num = 0;

    private void Awake()
    {
        num = Random.Range(0, m_sprite.Length);
        m_image.sprite = m_sprite[num];
    }
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Sprite GetSprite()
    {
        return m_sprite[num];
    }
}
