using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SLGrid : MonoBehaviour {
    public bool displayGridGizmos;

    int gridSizeX, gridSizeY;

    Node[, ] grid;

    int penaltyMin = int.MaxValue;
    int penaltyMax = int.MinValue;

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
        BlurPenaltyMap (3);
    }

    void BlurPenaltyMap (int blurSize) {
        int kernelSize = blurSize * 2 + 1;
        int kernelExtents = (kernelSize - 1) / 2;

        int[, ] penaltiesHorizontalPass = new int[gridSizeX, gridSizeY];
        int[, ] penaltiesVerticalPass = new int[gridSizeX, gridSizeY];

        for (int y = 0; y < gridSizeY; y++) {
            for (int x = -kernelExtents; x <= kernelExtents; x++) {
                int sampleX = Mathf.Clamp (x, 0, kernelExtents);
                penaltiesHorizontalPass[0, y] += grid[sampleX, y].movementPenalty;
            }

            for (int x = 1; x < gridSizeX; x++) {
                int removeIndex = Mathf.Clamp (x - kernelExtents - 1, 0, gridSizeX);
                int addIndex = Mathf.Clamp (x + kernelExtents, 0, gridSizeX - 1);

                penaltiesHorizontalPass[x, y] = penaltiesHorizontalPass[x - 1, y] - grid[removeIndex, y].movementPenalty + grid[addIndex, y].movementPenalty;
            }
        }

        for (int x = 0; x < gridSizeX; x++) {
            for (int y = -kernelExtents; y <= kernelExtents; y++) {
                int sampleY = Mathf.Clamp (y, 0, kernelExtents);
                penaltiesVerticalPass[x, 0] += penaltiesHorizontalPass[x, sampleY];
            }

            int blurredPenalty = Mathf.RoundToInt ((float) penaltiesVerticalPass[x, 0] / (kernelSize * kernelSize));
            grid[x, 0].movementPenalty = blurredPenalty;

            for (int y = 1; y < gridSizeY; y++) {
                int removeIndex = Mathf.Clamp (y - kernelExtents - 1, 0, gridSizeY);
                int addIndex = Mathf.Clamp (y + kernelExtents, 0, gridSizeY - 1);

                penaltiesVerticalPass[x, y] = penaltiesVerticalPass[x, y - 1] - penaltiesHorizontalPass[x, removeIndex] + penaltiesHorizontalPass[x, addIndex];
                blurredPenalty = Mathf.RoundToInt ((float) penaltiesVerticalPass[x, y] / (kernelSize * kernelSize));
                grid[x, y].movementPenalty = blurredPenalty;

                if (blurredPenalty > penaltyMax) {
                    penaltyMax = blurredPenalty;
                }
                if (blurredPenalty < penaltyMin) {
                    penaltyMin = blurredPenalty;
                }
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
        int x = Mathf.RoundToInt (worldPosition.x);
        int y = Mathf.RoundToInt (worldPosition.y);
        return grid[x, y];
    }

    void OnDrawGizmos () {
        Gizmos.DrawWireCube (transform.position, new Vector3 (gridSizeX, gridSizeY, 1));
        if (grid != null && displayGridGizmos) {
            foreach (Node n in grid) {

                Gizmos.color = Color.Lerp (Color.white, Color.black, Mathf.InverseLerp (penaltyMin, penaltyMax, n.movementPenalty));
                Gizmos.color = (n.walkable) ? Gizmos.color : Color.red;
                Gizmos.DrawCube (n.worldPosition, Vector3.one);
            }
        }
    }
}