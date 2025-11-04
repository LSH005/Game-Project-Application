using System.Net.Http.Headers;
using UnityEngine;

public class PerlinNoise : MonoBehaviour
{
    [Header("기본 블록")]
    public GameObject dirtPrefab;
    public GameObject grassPrefab;
    public GameObject waterPrefab;

    [Header("특수 블록 (광물)")]
    public GameObject goldPrefab;
    public float goldProbability = 1;
    public GameObject coalPrefab;
    public float coalProbability = 5;


    [Header("지형 범위")]
    public int width = 20;
    public int depth = 20;
    public int maxHeight = 16;

    public int waterHeight = 5;

    [SerializeField] float noiseScale = 20f;


    void Start()
    {
        waterHeight++;

        float offsetX = Random.Range(-9999f, 9999f);
        float offsetY = Random.Range(-9999f, 9999f);

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < depth; z++)
            {
                float nx = (x + offsetX) / noiseScale;
                float nz = (z + offsetY) / noiseScale;

                float noise = Mathf.PerlinNoise(nx, nz);
                int h = Mathf.FloorToInt(noise * maxHeight);
                h = Mathf.Max(h, 2);

                for (int y = 1; y <= h; y++)
                {
                    GameObject targetBlock;
                    if (h == y) targetBlock = grassPrefab;
                    else
                    {
                        if (GetProbability(goldProbability)) targetBlock = goldPrefab;
                        else if (GetProbability(coalProbability)) targetBlock = coalPrefab;
                        else targetBlock = dirtPrefab;
                    }

                    SetBlock(x, y, z, targetBlock);
                }

                for (int y = h + 1; y < waterHeight; y++)
                {
                    SetBlock(x, y, z, waterPrefab);
                }
            }
        }
    }

    void SetBlock(int x, int y, int z, GameObject block)
    {
        var go = Instantiate(block, new Vector3(x, y, z), Quaternion.identity, transform);
        
        string blockName = "?";

        if (block == dirtPrefab) blockName = "dirt";
        else if (block == grassPrefab) blockName = "grass_block";
        else if (block == waterPrefab) blockName = "water";
        else if (block == goldPrefab) blockName = "gold";
        else if (block == coalPrefab) blockName = "coal";


        go.name = $"{blockName} : {x} // {y} // {z}";
    }

    bool GetProbability(float percent)
    {
        if (percent > 100f)
        {
            return true;
        }
        else if (percent < 0f)
        {
            return false;
        }
        else
        {
            float random100 = Random.Range(0.0f, 100.0f);
            return random100 <= percent;
        }
    }
}
