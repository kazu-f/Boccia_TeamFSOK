using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�}�b�`���O��T���n�߂�B
public class FindMatching : MonoBehaviour
{
    private NetworkLauncherScript launcher = null;
    private float startTime = 0.0f;
    [Tooltip("�}�b�`���O�̂��߂ɑҋ@���鎞�Ԃ̃��~�b�g�B")]
    [SerializeField] private float LIMIT_WAIT_TIME = 30.0f;
    // Start is called before the first frame update
    void Start()
    {
        launcher = this.gameObject.GetComponent<NetworkLauncherScript>();
        if(launcher != null)
        {
            //�ڑ����J�n����B
            launcher.Connect();
        }
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time - startTime > LIMIT_WAIT_TIME)
        {
            //�}�b�`���O�Ɏ��Ԃ��|���肷���Ă��邽��AI�ΐ���s���B
            launcher.UseAI();
            Debug.Log("AI�ΐ�ɐ؂�ւ���B");
        }        
    }
}
