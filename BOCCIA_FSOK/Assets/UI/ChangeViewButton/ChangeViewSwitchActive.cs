using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeViewSwitchActive : MonoBehaviour
{
    [SerializeField] private SwitchActive[] switcheObjs;      //ボタンを押すことで切り替わるゲームオブジェクトの配列。
    [SerializeField] private ActiveTeamController activeTeam = null;      //ボタンを押すことで切り替わるゲームオブジェクトの配列。
    private CameraChangeSprite changeSprite = null;
    private bool isFixedCamera = false;

    private void Awake()
    {
        changeSprite = this.gameObject.GetComponent<CameraChangeSprite>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SwitchChangeView()
    {
        isFixedCamera = !isFixedCamera;
        //オブジェクトの配列のアクティブを切り替える。
        if (switcheObjs.Length > 0)
        {
            for (int i = 0; i < switcheObjs.Length; i++)
            {
                switcheObjs[i].SwitchObjectActive();
            }
        }
        activeTeam.SwichActiveThrow();
        changeSprite.ChangeSprite();
    }

    /// <summary>
    /// 俯瞰カメラで見ているのをリセットする。
    /// </summary>
    public void ResetFixedCamera()
    {
        if (!isFixedCamera) return;

        SwitchChangeView();
    }
}
