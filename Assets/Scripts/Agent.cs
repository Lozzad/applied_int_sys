using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour {
    SpriteRenderer head;
    SpriteRenderer body;

    Vector2 worldPos;
    Vector2 previousWorldPos;
    [Range (1, 3)]
    public float speed;

    void Awake () {
        CalculateWorldPos ();
    }

    void Start () {
        speed = Random.Range (0f, 3f);
        StartCoroutine (MoveCoroutine (0.2f * speed));
    }

    void Update () {

    }

    void CalculateWorldPos () {
        worldPos = new Vector2 (
            Mathf.RoundToInt (gameObject.transform.position.x),
            Mathf.RoundToInt (gameObject.transform.position.y)
        );
    }

    void NextMove () {

        var movedir = CalculateHighestNeighbour ();
        gameObject.transform.position += movedir;
    }

    private IEnumerator MoveCoroutine (float waitTime) {
        while (true) {

            NextMove ();

            yield return new WaitForSeconds (waitTime);
        }
    }

    Vector3 CalculateLowestNeighbour () {
        CalculateWorldPos ();
        float cheapest = float.MaxValue;
        Vector2 coord = Vector2.zero;
        for (int x = -1; x < 2; x++) {
            for (int y = -1; y < 2; y++) {
                if (Mathf.Abs (x + y) != 1) {
                    continue;
                }
                if (MapGenerator.instance.mapData.heightMap[(int) worldPos.x + x, (int) worldPos.y + y] < cheapest) {
                    cheapest = MapGenerator.instance.mapData.heightMap[(int) worldPos.x + x, (int) worldPos.y + y];
                    coord.x = x;
                    coord.y = y;
                }
            }
        }

        return coord;
    }

    Vector3 CalculateHighestNeighbour () {
        CalculateWorldPos ();
        float cheapest = float.MinValue;
        Vector2 coord = Vector2.zero;
        for (int x = -1; x < 2; x++) {
            for (int y = -1; y < 2; y++) {
                if (Mathf.Abs (x + y) != 1) {
                    continue;
                }
                if (MapGenerator.instance.mapData.heightMap[(int) worldPos.x + x, (int) worldPos.y + y] > cheapest) {
                    cheapest = MapGenerator.instance.mapData.heightMap[(int) worldPos.x + x, (int) worldPos.y + y];
                    coord.x = x;
                    coord.y = y;
                }
            }
        }

        return coord;
    }
}