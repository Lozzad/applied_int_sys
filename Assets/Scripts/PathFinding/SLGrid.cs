using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SLGrid : MonoBehaviour {
    Node[, ] grid;
    public Vector2 gridWorldSize;
    int gridSizeX, gridSizeY;

    void Awake () {

    }

    void Start () {
        gridSizeX = MapGenerator.instance.mapWidth;
        gridSizeY = MapGenerator.instance.mapHeight;

        CreateGrid ();
    }

    public int MaxSize {
        get {
            return gridSizeX * gridSizeY;
        }
    }

    void CreateGrid () {
        grid = new Node[gridSizeX, gridSizeY];
        Vector2 worldBottomLeft = Vector2.zero; //this culd be wrong

        for (int x = 0; x < gridSizeX; x++) {
            for (int y = 0; y < gridSizeY; y++) {
                Vector2 worldPoint = worldBottomLeft + Vector2.right * (x) + Vector2.up * (y); //also this could be wrong
                bool walkable = true; //!(Physics.CheckSphere(worldPoint, 1, unwalkableMask));
                int movementPenalty = 0;
                float heightValue = MapGenerator.instance.mapData.heightMap[x, y];
                grid[x, y] = new Node (walkable, worldPoint, x, y, movementPenalty, heightValue);
            }
        }
    }

    public List<Node> GetNeighbours (Node node) {
        List<Node> neighbours = new List<Node> ();

        for (int x = -1; x <= 1; x++) {
            for (int y = -1; y <= 1; y++) {
                if (x == 0 && y == 0)
                    continue;

                int checkX = node.gridX + x;
                int checkY = node.gridY + y;

                if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY) {
                    neighbours.Add (grid[checkX, checkY]);
                }
            }
        }
        return neighbours;
    }

    public Node NodeFromWorldPoint (Vector2 worldPosition) {

        return grid[Mathf.RoundToInt (worldPosition.x), Mathf.RoundToInt (worldPosition.y)];
    }
}