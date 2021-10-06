using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetScoreTextScript : MonoBehaviour
{
    [SerializeField] private Text redTeamScore;     //�ԃ`�[���̃X�R�A�B
    [SerializeField] private Text blueTeamScore;    //�`�[���̃X�R�A�B

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    /// <summary>
    /// �G���h���U���g���Z�b�g����B
    /// </summary>
    /// <param name="result">���U���g�B</param>
    public void SetEndResult(GameScore.EndResult result)
    {
        redTeamScore.text = result.redTeamScore.ToString();
        blueTeamScore.text = result.blueTeamScore.ToString();
    }
}
