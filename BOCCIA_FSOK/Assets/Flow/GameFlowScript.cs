using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameFlowScript : MonoBehaviour
{
    private bool m_IsEnd = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// �Q�[���I���̃t���O�𗧂Ă�
    /// </summary>
    public void GameEnd()
    {
        m_IsEnd = true;
    }
}
