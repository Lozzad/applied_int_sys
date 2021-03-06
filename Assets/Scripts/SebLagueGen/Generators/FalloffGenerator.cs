using System.Collections;
using UnityEngine;

public static class FalloffGenerator {

    public static float[, ] GenerateFalloffMap (int width, int height) {
        float[, ] map = new float[width, height];

        for (int i = 0; i < width; i++) {
            for (int j = 0; j < height; j++) {
                //each cell given a value based on dist from edge
                float x = i / (float) width * 2 - 1;
                float y = j / (float) height * 2 - 1;

                //take largest value
                float value = Mathf.Max (Mathf.Abs (x), Mathf.Abs (y));
                //curve 
                map[i, j] = Evaluate (value);
            }
        }

        return map;
    }

    static float Evaluate (float value) {
        float a = 3;
        float b = 2.2f;

        return Mathf.Pow (value, a) / (Mathf.Pow (value, a) + Mathf.Pow (b - b * value, a));
    }
}