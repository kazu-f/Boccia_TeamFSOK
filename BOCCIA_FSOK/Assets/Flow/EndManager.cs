using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndManager : MonoBehaviour
{
    [SerializeField] TeamFlowScript m_TeamFlow;
    [SerializeField] EndFlowScript m_EndFlow;
    [SerializeField] TeamFlowDelayScript m_Delay;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        switch (m_TeamFlow.GetState())
        {
            case TeamFlowState.Move:

                break;
            case TeamFlowState.Stop:

                break;
            case TeamFlowState.delay:

                break;
            case TeamFlowState.Caluced:

                break;
            default:
                return;
        }
    }
}
