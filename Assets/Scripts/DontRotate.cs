using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontRotate : MonoBehaviour {
    public Quaternion desiredRotation = Quaternion.identity;

    void LateUpdate () {
        transform.rotation = desiredRotation;
    }
}