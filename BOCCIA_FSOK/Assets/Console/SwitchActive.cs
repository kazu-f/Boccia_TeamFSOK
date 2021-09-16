using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchActive : MonoBehaviour
{
    public bool m_isActive{ get; set; } = true;
    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.SetActive(m_isActive);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// �A�N�e�B�u��Ԃ�؂�ւ���B
    /// </summary>
    public void SwitchObjectActive()
    {
        m_isActive = true ^ m_isActive;
        this.gameObject.SetActive(m_isActive);
    }
}
