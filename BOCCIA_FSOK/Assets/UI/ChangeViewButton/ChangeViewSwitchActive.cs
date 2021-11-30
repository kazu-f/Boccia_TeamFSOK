using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeViewSwitchActive : MonoBehaviour
{
    [SerializeField] private SwitchActive[] switcheObjs;      //�{�^�����������ƂŐ؂�ւ��Q�[���I�u�W�F�N�g�̔z��B
    [SerializeField] private ActiveTeamController activeTeam = null;      //�{�^�����������ƂŐ؂�ւ��Q�[���I�u�W�F�N�g�̔z��B
    private CameraChangeSprite changeSprite = null;

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
        //�I�u�W�F�N�g�̔z��̃A�N�e�B�u��؂�ւ���B
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
}
