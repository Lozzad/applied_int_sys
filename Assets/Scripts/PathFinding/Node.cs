﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : IHeapItem<Node> {
    public bool walkable;
    public Vector2 worldPosition;
    public int gridX;
    public int gridY;
    public int movementPenalty;
    public float height;

    public int gCost;
    public int hCost;
    public Node parent;
    int heapIndex;

    public Node (bool _walkable, Vector2 _worldPos, int _gridX, int _gridY, int _penalty, float _height) {
        walkable = _walkable;
        worldPosition = _worldPos;
        gridX = _gridX;
        gridY = _gridY;
        movementPenalty = _penalty;
        height = _height;
    }

    public int fCost {
        get {
            return gCost + hCost;
        }
    }

    public int HeapIndex {
        get {
            return heapIndex;
        }
        set {
            heapIndex = value;
        }
    }

    public int GetSlopeFrom (Node startNode) {

        int value = (int) (1000 * Mathf.Abs (height - startNode.height));

        return value;

    }

    public int CompareTo (Node nodeToCompare) {
        int compare = fCost.CompareTo (nodeToCompare.fCost);
        if (compare == 0) {
            compare = hCost.CompareTo (nodeToCompare.hCost);
        }
        return -compare;
    }
}