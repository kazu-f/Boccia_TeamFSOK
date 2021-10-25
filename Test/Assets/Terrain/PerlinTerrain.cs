using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerlinTerrain : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private float SeedX, SeedZ; // マップ生成をランダムにするための変数

    [SerializeField]
    private float Width = 50; // マップの横幅
    [SerializeField]
    private float Depth = 50; // マップの奥行き

    [SerializeField]
    private bool NeedToCollider = false; // 当たり判定をつけるかどうかの変数

    [SerializeField]
    private float MaxHeight = 10; // 最大の高さ

    [SerializeField]
    private float Relief = 15f; // 起伏の激しさ

    private int PositionY; // 高さを整数に変換するための変数

    private void Awake()
    {
        SeedX = Random.value * 100f; // 同じ値にならないようにx座標をランダムにする
        SeedZ = Random.value * 100f; // 同じ値にならないようにz座標をランダムにする

        //キューブ生成
        for (int x = 0; x < Width; x++) // 指定した横幅になるまで繰り返す
        {
            for (int z = 0; z < Depth; z++) // 指定した奥行きになるまで繰り返す
            {
                GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);　// 新しいキューブ作成
                cube.transform.localPosition = new Vector3(x, 0, z); // 地面に置く
                cube.transform.SetParent(transform); // キューブを子オブジェクトにする

                if (!NeedToCollider) // もし、コライダーをつけないなら、
                {
                    Destroy(cube.GetComponent<BoxCollider>()); // コライダーを消す
                }

                SetY(cube); // 高さの設定をする
            }
        }
    }

    private void SetY(GameObject cube) // キューブのY座標を設定する関数
    {
        float y = 0; // yを0にする
        float xSample = (cube.transform.localPosition.x + SeedX) / Relief; // パーリンノイズで使う値を求める 
        float zSample = (cube.transform.localPosition.z + SeedZ) / Relief; // パーリンノイズで使う値を求める
        float noise = Mathf.PerlinNoise(xSample, zSample); // ノイズをパーリンノイズで出した値にする
        y = MaxHeight * noise; // yを最大の高さ*noiseの値にする
        PositionY = Mathf.RoundToInt(y); // PositionYをyを四捨五入した値にする
        cube.transform.localPosition = new Vector3(cube.transform.localPosition.x, PositionY, cube.transform.localPosition.z); // キューブを設置する

        Color color = Color.black;//岩盤っぽい色

        if (y > MaxHeight * 0.3f)
        {
            ColorUtility.TryParseHtmlString("#006400", out color);//草っぽい色
        }

        else if (y > MaxHeight * 0.2f)
        {
            ColorUtility.TryParseHtmlString("#4169e1", out color);//水っぽい色
        }

        else if (y > MaxHeight * 0.1f)
        {
            ColorUtility.TryParseHtmlString("#d2691e", out color);//マグマっぽい色
        }

        cube.GetComponent<MeshRenderer>().material.color = color;
    }
}
