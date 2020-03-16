using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour {
    public Transform target;
    float speed = 4;
    Vector2[] path;
    int targetIndex;

    void Awake () {
        target = FindObjectOfType<DontRotate> ().gameObject.transform;
    }

    void Start () {
        PathRequestManager.RequestPath (transform.position, target.position, OnPathFound);
    }

    public void OnPathFound (Vector2[] newPath, bool pathSuccessful) {
        if (pathSuccessful) {
            path = newPath;
            targetIndex = 0;
            StopCoroutine ("FollowPath");
            StartCoroutine ("FollowPath");
        }
    }

    IEnumerator FollowPath () {
        Vector2 currentWaypoint = path[0];

        while (true) {
            if ((Vector2) transform.position == currentWaypoint) {
                targetIndex++;
                if (targetIndex >= path.Length) {
                    yield break;
                }
                currentWaypoint = path[targetIndex];
            }

            transform.position = Vector2.MoveTowards (transform.position, currentWaypoint, speed * Time.deltaTime);
            yield return null;
        }
    }

    public void OnDrawGizmos () {
        if (path != null) {
            for (int i = targetIndex; i < path.Length; i++) {
                Gizmos.color = Color.black;
                Gizmos.DrawCube (path[i], Vector3.one);

                if (i == targetIndex) {
                    Gizmos.DrawLine (transform.position, path[i]);
                } else {
                    Gizmos.DrawLine (path[i - 1], path[i]);
                }
            }
        }
    }

}