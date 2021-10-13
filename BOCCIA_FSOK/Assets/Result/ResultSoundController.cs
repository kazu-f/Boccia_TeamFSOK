using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultSoundController : MonoBehaviour
{
    [SerializeField] private AudioSource drumRoll;  //ドラムロールの音。
    [SerializeField] private AudioSource cymbal;    //シンバルの音。

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
        //ドラムループをさせるか。
        if(isDrumLoop
            && drumRoll.time > DRUM_ROLL_LOOP_END)
        {
            drumRoll.time = DRUM_ROLL_LOOP_START;
        }
    }

    //ドラムロールをループさせる。
    public void SetDrumRollLoop(bool isLoop)
    {
        //ドラムをループさせるか。
        isDrumLoop = isLoop;
    }
    /// <summary>
    /// ドラムロールが終わったか。
    /// </summary>
    public bool IsEndDrummRoll()
    {
        return !drumRoll.isPlaying;
    }
    /// <summary>
    /// シンバルを鳴らす。
    /// </summary>
    public void PlayCymbal()
    {
        cymbal.Play();
    }
}
