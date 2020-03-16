using System.Collections;
using UnityEditor;
using UnityEngine;

[CustomEditor (typeof (MapGenerator))]
public class MapGeneratorEditor : Editor {

    // public override void OnInspectorGUI () {
    //     MapGenerator mapGen = (MapGenerator) target;

    //     // if (DrawDefaultInspector ()) {
    //     //     if (mapGen.autoUpdate) {
    //     //         mapGen.DrawMapInEditor ();
    //     //     }
    //     // }

    //     // if (GUILayout.Button ("Generate")) {
    //     //     mapGen.DrawMapInEditor ();
    //     // }

    //     // if (GUILayout.Button ("Random Seed")) {
    //     //     mapGen.seed = (int) Random.Range (0, 500);
    //     //     mapGen.DrawMapInEditor ();
    //     // }
    // }
}