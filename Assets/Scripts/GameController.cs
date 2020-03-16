using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {
    MapGenerator mapGenerator;
    MapData mapData;
    private static GameController instance;
    public static GameController Instance {
        get { return instance; }
    }

    void Awake () {
        instance = this;
        mapData = FindObjectOfType<MapGenerator> ().GetMapData ();
    }

    void Start () {
        //mapGenerator = MapGenerator.instance;

    }

    void Update () {
        if (Input.GetMouseButtonDown (0)) {
            Vector2 loc = Camera.main.ScreenToWorldPoint ((Vector2) Input.mousePosition);
            Debug.Log ("clicked " + loc);
            //FlockController.instance.SpawnBoid (loc);
            PlaceTown (loc);
        }
    }

    void PlaceTown (Vector2 loc) {
        var x = Mathf.RoundToInt (loc.x);
        var y = Mathf.RoundToInt (loc.y);

        Debug.LogFormat ("Placed town at {0}, {1}, cell value {2}", x, y, mapGenerator.mapData.heightMap[x, y]);

    }
}