using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapDisplay : MonoBehaviour {

    public GameObject quad;

    public void DrawTexture (Texture2D texture) {
        int texWidth = texture.width;
        int xoffset = Mathf.RoundToInt (texWidth / 2);
        int yoffset = Mathf.RoundToInt (texWidth / 2);
        quad.GetComponent<Renderer> ().sharedMaterial.mainTexture = texture;
        quad.GetComponent<Renderer> ().transform.localScale = new Vector3 (texture.width, texture.height, 1);

    }
}