using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapDisplay : MonoBehaviour {

    public GameObject[] quads;
    public GameObject quadPrefab;

    public void DrawTexture (List<Texture2D> textures, int mapWidth, int mapHeight) {
        int xoffset = Mathf.RoundToInt (mapWidth / 2);
        int yoffset = Mathf.RoundToInt (mapHeight / 2);
        quads = new GameObject[textures.Count];
        for (int i = 0; i < textures.Count; i++) {
            quads[i] = Instantiate (quadPrefab, new Vector3 (xoffset, yoffset, -i), Quaternion.identity);
            quads[i].GetComponent<Renderer> ().material.mainTexture = textures[i];
            quads[i].GetComponent<Renderer> ().transform.localScale = new Vector3 (textures[i].width, textures[i].height, 1);
        }
    }
}