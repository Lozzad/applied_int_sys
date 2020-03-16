using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameTiles : MonoBehaviour {
    public static GameTiles instance;
    private MapGenerator mg;
    public Tilemap Tilemap;
    public Dictionary<Vector3, WorldTile> tiles;

    void Awake () {
        if (instance == null) {
            instance = this;
        } else if (instance != this) {
            Destroy (gameObject);
        }
    }

    void Start () {
        mg = MapGenerator.instance;
        GenerateTileMap ();
        GetWorldTiles ();
    }

    private void GenerateTileMap () {
        // tiles = new Dictionary<Vector3, WorldTile> ();
        // for (int x = 0; x < mg.mapWidth; x++) {
        //     for (int y = 0; y < mg.mapHeight; y++) {
        //         var f = mg.mapData.heightMap[x, y];
        //         Tilemap.SetTile (new Vector3Int (x, y, 0), )
        //     }
        // }
    }

    private void GetWorldTiles () {
        tiles = new Dictionary<Vector3, WorldTile> ();
        foreach (Vector3Int pos in Tilemap.cellBounds.allPositionsWithin) {
            var localpos = new Vector3Int (pos.x, pos.y, pos.z);

            if (!Tilemap.HasTile (localpos)) continue;
            var tile = new WorldTile {
                LocalPosition = localpos,
                WorldPosition = Tilemap.CellToWorld (localpos),
                TileBase = Tilemap.GetTile (localpos),
                TilemapMember = Tilemap,
                Name = localpos.x + ", " + localpos.y,
                Height = 1
            };
            tiles.Add (tile.WorldPosition, tile);
        }
    }

    // Update is called once per frame
    void Update () {

    }
}