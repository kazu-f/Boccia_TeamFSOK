using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultParticleScript : MonoBehaviour
{
    [SerializeField]private GameObject[] particleObjects;
    private Material[] materials;
    private ParticleSystem[] particles;
    private ParticleSystemRenderer[] particleRenderers;
    private int objLength = 0;

    private Color colorRed = Color.red;
    private Color colorBlue = Color.blue;


    // Start is called before the first frame update
    void Start()
    {
        objLength = particleObjects.Length;
        materials = new Material[objLength];
        particles = new ParticleSystem[objLength];
        particleRenderers = new ParticleSystemRenderer[objLength];

        //マテリアルの配列作成。
        for (int i = 0;i< objLength; i++)
        {
            particles[i] = particleObjects[i].GetComponent<ParticleSystem>();
            particleRenderers[i] = particleObjects[i].GetComponent<ParticleSystemRenderer>();
            materials[i] = particleRenderers[i].material;
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    /// <summary>
    /// パーティクルの色をチームカラーに変更する。
    /// </summary>
    public void PlayWinnerParticle(Team team)
    {
        Color col;
        if(team == Team.Red)
        {
            col = colorRed;
        }
        else if(team == Team.Blue)
        {
            col = colorBlue;
        }
        else
        {
            return;
        }

        for(int i = 0;i < objLength; i++)
        {
            materials[i].color = col;
            particles[i].Play();
        }
    }
}
