using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class GenerateLevel : MonoBehaviour
{
    public int mapChunkSize;
    [Range(0, 6)]
    public int levelOfDetail;
    public float noiseScale;
    [Range(0, 10)]
    public int octaves;
    [Range(0, 1)]
    public float persistance;
    public float lacunarity;

    public int seed;
    public Vector2 offset;

    public bool autoUpdate;

    public TerrainType[] regions;

    public Renderer texRenderer;

    public void Start()
    {
        if (autoUpdate)
        {
            GenMap();
        }
    }

    public void GenMap()
    {
        mapChunkSize = (int)texRenderer.gameObject.transform.localScale.x * 10;

        float[,] noiseMap = Noise.GenNoiseMap(mapChunkSize, mapChunkSize, noiseScale, seed, octaves, persistance, lacunarity, offset);

        Color[] colourMap = new Color[mapChunkSize * mapChunkSize];

        Unit.InitializeWalkableMap(mapChunkSize);

        for (int z = 0; z < mapChunkSize; z++)
        {
            for (int x = 0; x < mapChunkSize; x++)
            {
                float currentHeight = noiseMap[x, z];
                for (int i = 0; i < regions.Length; i++)
                {
                    if (currentHeight <= regions[i].height)
                    {
                        colourMap[z * mapChunkSize + x] = regions[i].colour;
                        Unit.walkableMap[x, z] = i;
                        break;
                    }
                }
            }
        }

        Texture2D tex = new Texture2D(mapChunkSize, mapChunkSize);
        tex.filterMode = FilterMode.Point;
        tex.wrapMode = TextureWrapMode.Clamp;
        tex.SetPixels(colourMap);
        tex.Apply();

        texRenderer.sharedMaterial.mainTexture = tex;

    }

    private void OnValidate()
    {
        if (lacunarity < 1) { lacunarity = 1; }
        if (octaves < 0) { octaves = 0; }
    }
    [System.Serializable]
    public struct TerrainType
    {
        public string name;
        public float height;
        public Color colour;
    }
}