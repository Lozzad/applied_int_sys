using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TrailMapGenerator {
    public static float[, ] GenerateTrailMap (int width, int height) {
        //initialise at 0
        float[, ] trailmap = new float[width, height];
        return trailmap;
    }
}