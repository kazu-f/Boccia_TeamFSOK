using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultSoundController : MonoBehaviour
{
    [SerializeField] private AudioSource drumRoll;  //�h�������[���̉��B
    [SerializeField] private AudioSource cymbal;    //�V���o���̉��B

    [SerializeField] private float DRUM_ROLL_LOOP_START = 3.0f;
    [SerializeField] private float DRUM_ROLL_LOOP_END = 4.0f;
    private bool isDrumLoop = true;

    // Start is called before the first frame update
    void Start()
    {
        drumRoll.PlayDelayed(0.1f);
    }

    // Update is called once per frame
    void Update()
    {
        //�h�������[�v�������邩�B
        if(isDrumLoop
            && drumRoll.time > DRUM_ROLL_LOOP_END)
        {
            drumRoll.time = DRUM_ROLL_LOOP_START;
        }
    }

    //�h�������[�������[�v������B
    public void SetDrumRollLoop(bool isLoop)
    {
        //�h���������[�v�����邩�B
        isDrumLoop = isLoop;
    }
    /// <summary>
    /// �h�������[�����I��������B
    /// </summary>
    public bool IsEndDrummRoll()
    {
        return !drumRoll.isPlaying;
    }
    /// <summary>
    /// �V���o����炷�B
    /// </summary>
    public void PlayCymbal()
    {
        cymbal.Play();
    }
}
