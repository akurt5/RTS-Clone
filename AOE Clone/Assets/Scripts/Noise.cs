using UnityEngine;
using System.Collections;

public static class Noise
{

    public static float[,] GenNoiseMap(int _Width, int _Height, float _Scale, int _Seed, int _Octaves, float _Persistance, float _Lacunarity, Vector2 _Offset)
    {
        float[,] noiseMap = new float[_Width, _Height];

        System.Random prng = new System.Random(_Seed);
        Vector2[] octaveOffsets = new Vector2[_Octaves];
        for (int i = 0; i < _Octaves; i++)
        {
            float offsetX = prng.Next(-100000, 100000) + _Offset.x;
            float offsetY = prng.Next(-100000, 100000) + _Offset.y;
            octaveOffsets[i] = new Vector2(offsetX, offsetY);
        }
        
        if (_Scale <= 0)
        {
            _Scale = 0.0001f;
        }

        float minNoiseHeight = float.MaxValue;
        float maxNoiseHeight = float.MinValue;

        float halfWidth = _Width / 2f;
        float halfHeight = _Height / 2f;

        for (int y = 0; y < _Height; y++)
        {
            for (int x = 0; x < _Width; x++)
            {
                float amplitude = 1;
                float frequency = 1;
                float noiseHeight = 0;

                for (int i = 0; i < _Octaves; i++)
                {
                    float sampleX = (x - halfWidth) / _Scale  * frequency + octaveOffsets[i].x;
                    float sampleY = (y - halfHeight) / _Scale  * frequency + octaveOffsets[i].y;

                    float perlinValue = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1;
                    noiseHeight += perlinValue * amplitude;

                    amplitude *= _Persistance;
                    frequency *= _Lacunarity;
                }
                if (noiseHeight > maxNoiseHeight)
                {
                    maxNoiseHeight = noiseHeight;
                }
                else if (noiseHeight < minNoiseHeight)
                {
                    minNoiseHeight = noiseHeight;
                }

                noiseMap[x, y] = noiseHeight;
            }
        }
        for (int y = 0; y < _Height; y++)
        {
            for (int x = 0; x < _Width; x++)
            {
                //Debug.Log(noiseMap[x, y]);
                noiseMap[x, y] = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, noiseMap[x, y]);
                
            }
        }
        return noiseMap;
    }
}
