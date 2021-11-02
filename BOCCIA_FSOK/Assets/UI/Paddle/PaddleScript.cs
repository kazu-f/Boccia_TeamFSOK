using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PaddleScript : MonoBehaviour
{
    private Image image = null;
    private RectTransform rect = null;
    [SerializeField] private Sprite RedPaddleSprite;
    [SerializeField] private Sprite BluePaddleSprite;

    private void Awake()
    {
        image = this.gameObject.GetComponent<Image>();
        rect = this.gameObject.GetComponent<RectTransform>();
    }
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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
    }
}
