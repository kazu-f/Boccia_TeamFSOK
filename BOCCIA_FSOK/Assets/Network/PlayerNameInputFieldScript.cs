using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

[RequireComponent(typeof(InputField))]
public class PlayerNameInputFieldScript : MonoBehaviour
{
    #region Private Constants

    const string playerNamePrefKey = "PlyerName";

    #endregion

    #region MonoBehaviour CallBacks

    // Start is called before the first frame update
    void Start()
    {
        string defaultName = string.Empty;
        InputField _inputField = this.GetComponent<InputField>();
        if(_inputField != null)
        {
            if(PlayerPrefs.HasKey(playerNamePrefKey))
            {
                defaultName = PlayerPrefs.GetString(playerNamePrefKey);
                _inputField.text = defaultName;
            }
        }
        PhotonNetwork.NickName = defaultName;
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// �v���C���[�̖��O��ݒ肵�āAPlayerPrefs�ɕۑ����܂�
    /// </summary>
    /// <param name="value">�v���C���[�̖��O</param>
    public void SetPayerName(string value)
    {
        if(string.IsNullOrEmpty(value))
        {
            //�v���C���[�̖��O�����������ĂȂ��Ƃ�
            Debug.LogError("�v���C���[�̖��O��null�������͋�ł�");
            return;
        }
        PhotonNetwork.NickName = value;
        //�l�b�g���[�N��Ńv���C���[����ݒ�
        PlayerPrefs.SetString(playerNamePrefKey,value);
    }

    #endregion

    // Update is called once per frame
    void Update()
    {
        
    }
}
