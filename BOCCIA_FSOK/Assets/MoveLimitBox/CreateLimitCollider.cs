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
        // Collider�I�u�W�F�N�g�̕`��͕s�v�Ȃ̂�Renderer������
        Destroy(this.gameObject.GetComponent<MeshRenderer>());
        //�R���C�_�[��S�폜�B
        Collider[] colliders = this.gameObject.GetComponents<Collider>();
        for (int i = 0; i < colliders.Length; i++)
            DestroyImmediate(colliders[i]);
        //���b�V���̌����𔽓]�B
        collideMesh = GetComponent<MeshFilter>().mesh;
        collideMesh.triangles = collideMesh.triangles.Reverse().ToArray();
        //���b�V���R���C�_�[���쐬�B
        this.gameObject.AddComponent<MeshCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
