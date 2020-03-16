using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WorldTile {
    public Vector3Int LocalPosition { get; set; }
    public Vector3 WorldPosition { get; set; }
    public TileBase TileBase { get; set; }
    public Tilemap TilemapMember { get; set; }
    public string Name { get; set; }
    public bool IsExplored { get; set; }
    public WorldTile ExploredFrom { get; set; }
    public int Height { get; set; }
}