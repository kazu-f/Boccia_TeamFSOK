using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmphasisScript : MonoBehaviour
{
    [SerializeField] private Material filter;
    [SerializeField] private Vector3 forward = Vector3.zero;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Camera>().depthTextureMode |= DepthTextureMode.DepthNormals;
        forward = this.gameObject.GetComponent<Transform>().forward;
        GetComponent<Renderer>().material.SetVector("CameraForward", forward);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        Graphics.Blit(src, dest, filter);
    }
}
