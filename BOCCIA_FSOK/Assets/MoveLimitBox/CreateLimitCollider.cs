using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CreateLimitCollider : MonoBehaviour
{
    private Mesh collideMesh = null;

    // Start is called before the first frame update
    void Start()
    {
        // Colliderオブジェクトの描画は不要なのでRendererを消す
        Destroy(this.gameObject.GetComponent<MeshRenderer>());
        //コライダーを全削除。
        Collider[] colliders = this.gameObject.GetComponents<Collider>();
        for (int i = 0; i < colliders.Length; i++)
            DestroyImmediate(colliders[i]);
        //メッシュの向きを反転。
        collideMesh = GetComponent<MeshFilter>().mesh;
        collideMesh.triangles = collideMesh.triangles.Reverse().ToArray();
        //メッシュコライダーを作成。
        this.gameObject.AddComponent<MeshCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
