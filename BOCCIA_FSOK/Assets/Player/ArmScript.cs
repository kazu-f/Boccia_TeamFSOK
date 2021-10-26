using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmScript : MonoBehaviour
{
    TeamFlowScript m_TeamFlow = null;
    private void Awake()
    {
        m_TeamFlow = GameObject.Find("GameFlow").GetComponent<TeamFlowScript>();
        if (m_TeamFlow == null)
        {
            //TeamFlowコンポーネントの取得に失敗
            Debug.LogError("TeamFlowScriptコンポーネントの取得に失敗しました。");
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        HoldDown();
    }

    // Update is called once per frame
    void Update()
    {
        SetPlayer();
    }

    /// <summary>
    /// 上に構える
    /// </summary>
    public void HoldUp()
    {
        this.transform.localEulerAngles = new Vector3(-45.0f, 0.0f, 0.0f);
    }

    /// <summary>
    /// 下に構える
    /// </summary>
    public void HoldDown()
    {
        this.transform.localEulerAngles = new Vector3(0.0f, 0.0f, 180.0f);
    }

    public void SetPlayer()
    {
        GameObject player = null;
        if (m_TeamFlow.m_NextTeam == Team.Red)
        {
            player = GameObject.Find("PlayerRed");
        }
        else
        {
            player = GameObject.Find("PlayerBlue");
        }
        Vector3 Pos = Vector3.zero;
        Pos.x += 0.15f;
        Pos.y -= 0.54f;
        Pos.z += 0.2f;
        this.transform.SetParent(player.transform);
        this.transform.localPosition = Pos;
    }
}
