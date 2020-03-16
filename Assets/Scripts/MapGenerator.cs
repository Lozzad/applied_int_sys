//using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO: Split it into land and water, then change the types further within that.

public class MapGenerator : MonoBehaviour {

    public static MapGenerator instance;

    public Noise.NormalizeMode normalizeMode;
    public enum DrawMode { NoiseMap, ColourMap, FalloffMap }
    public DrawMode drawMode;

    public int mapWidth = 128;
    public int mapHeight = 128;
    public float noiseScale;
    public int octaves;
    [Range (0, 1)]
    public float persistance;
    public float lacunarity;
    public int seed;
    public Vector2 offset;

    public bool useFalloff;
    public bool autoUpdate;
    public bool useSightLines;

    public Region[] regions;

    float[, ] falloffMap;

    public MapData mapData;
    public MapDisplay display;
    public Shader mapShader;

    void Awake () {
        falloffMap = FalloffGenerator.GenerateFalloffMap (mapWidth, mapHeight);

        instance = this;
        mapData = GenerateMapData (Vector2.zero);
        // DrawMapInEditor ();

    }

    void Start () {
        var texs = new List<Texture2D> ();
        foreach (var t in regions) {
            texs.Add (TextureGenerator.TextureFromColourMap (t.colourMap, mapWidth, mapHeight));
        }

        display.DrawTexture (texs);
    }

    // public void DrawMapInEditor () {
    //     mapData = GenerateMapData (Vector2.zero);
    //     display.gameObject.transform.position = new Vector2 (mapWidth / 2, mapHeight / 2);
    //     if (drawMode == DrawMode.NoiseMap) {
    //         display.DrawTexture (new List<Texture2D> (1) { TextureGenerator.TextureFromHeightMap (mapData.heightMap) });
    //     } else if (drawMode == DrawMode.ColourMap) {
    //         var texs = new List<Texture2D> ();
    //         foreach (var t in regions) {
    //             texs.Add (TextureGenerator.TextureFromColourMap (t.colourMap, mapWidth, mapHeight));
    //         }

    //         display.DrawTexture (texs);
    //     } else if (drawMode == DrawMode.FalloffMap) {
    //         display.DrawTexture (new List<Texture2D> (1) { TextureGenerator.TextureFromHeightMap (FalloffGenerator.GenerateFalloffMap (mapWidth, mapHeight)) });
    //     }
    // }

    MapData GenerateMapData (Vector2 centre) {
        //generate the noisemap
        float[, ] noiseMap = Noise.GenerateNoiseMap (mapWidth, mapHeight, seed, noiseScale, octaves, persistance, lacunarity, centre + offset, normalizeMode);

        //apply the falloff map
        if (useFalloff) {
            for (int y = 0; y < mapHeight; y++) {
                for (int x = 0; x < mapWidth; x++) {
                    noiseMap[x, y] = Mathf.Clamp01 (noiseMap[x, y] - falloffMap[x, y]);
                }
            }

        }

        //noiseMap = RandomSample (noiseMap, 10);

        //create the colourmap
        for (int i = 0; i < regions.Length; i++) {
            regions[i].colourMap = new Color[mapWidth * mapHeight];
        }

        for (int y = 0; y < mapHeight; y++) {
            for (int x = 0; x < mapWidth; x++) {
                float currentHeight = noiseMap[x, y];
                for (int i = 0; i < regions.Length; i++) {
                    if (currentHeight >= regions[i].height) {
                        regions[i].colourMap[y * mapWidth + x] = regions[i].tint;
                    } else {
                        break;
                    }
                }
            }
        }

        float[, ] trailMap = TrailMapGenerator.GenerateTrailMap (mapWidth, mapHeight);

        return new MapData (noiseMap, regions, trailMap);
    }

    float[, ] RandomSample (float[, ] map, int numSamples) {
        for (int i = 0; i < numSamples; i++) {
            var sampleX = Random.Range (0, map.GetLength (0));
            var sampleY = Random.Range (0, map.GetLength (1));

            if (map[sampleX, sampleY] == 2) {
                i--;
                Debug.Log ("Chose Same Sample");
                break;
            } else {
                Debug.LogFormat ("point chosen at {0}, {1}", sampleX, sampleY);
                map[sampleX, sampleY] = 2.0f;
            }
        }

        return map;
    }

    void OnValidate () {
        if (lacunarity < 1) {
            lacunarity = 1;
        }
        if (octaves < 0) {
            octaves = 0;
        }

        falloffMap = FalloffGenerator.GenerateFalloffMap (mapWidth, mapHeight);
    }

    public MapData GetMapData () {
        return mapData;
    }
}

[System.Serializable]
public struct Region {
    public string name;
    public float height;
    public Color tint;
    public Color[] colourMap;
}

public struct MapData {
    public readonly float[, ] heightMap;
    public readonly Region[] regions;
    public float[, ] trailMap;

    public MapData (float[, ] heightMap, Region[] regions, float[, ] trailMap) {
        this.heightMap = heightMap;
        this.regions = regions;
        this.trailMap = trailMap;
    }
}