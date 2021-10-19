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
    /// プレイヤーの名前を設定して、PlayerPrefsに保存します
    /// </summary>
    /// <param name="value">プレイヤーの名前</param>
    public void SetPayerName(string value)
    {
        if(string.IsNullOrEmpty(value))
        {
            //プレイヤーの名前が何も入ってないとき
            Debug.LogError("プレイヤーの名前がnullもしくは空です");
            return;
        }
        PhotonNetwork.NickName = value;
        //ネットワーク上でプレイヤー名を設定
        PlayerPrefs.SetString(playerNamePrefKey,value);
    }

    #endregion

    // Update is called once per frame
    void Update()
    {
        
    }
}
