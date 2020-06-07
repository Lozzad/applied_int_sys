using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFindingUnit : MonoBehaviour {
    const float minPathUpdateTime = .2f;
    const float pathUpdateMoveThreshold = .5f;

    public Transform target;
    float speed = 5;
    public float turnSpeed = 3;
    public float turnDst = 5;
    public float stoppingDst = 5;

    Path path;

    void Awake () {
        target = FindObjectOfType<DontRotate> ().gameObject.transform;
    }

    void Start () {
        StartCoroutine (UpdatePath ());
    }

    public void OnPathFound (Vector2[] waypoints, bool pathSuccessful) {
        if (pathSuccessful) {
            path = new Path (waypoints, transform.position, turnDst, stoppingDst);

            StopCoroutine ("FollowPath");
            StartCoroutine ("FollowPath");
        }
    }

    IEnumerator UpdatePath () {
        if (Time.timeSinceLevelLoad < .3f) {
            yield return new WaitForSeconds (.3f);
        }
        PathRequestManager.RequestPath (new PathRequest (transform.position, target.position, OnPathFound));

        float sqrMoveThreshold = pathUpdateMoveThreshold * pathUpdateMoveThreshold;
        Vector2 targetPosOld = target.position;

        while (true) {
            yield return new WaitForSeconds (minPathUpdateTime);
            print ((((Vector2) target.position - targetPosOld).sqrMagnitude) + " " + sqrMoveThreshold);
            if (((Vector2) target.position - targetPosOld).sqrMagnitude > sqrMoveThreshold) {
                PathRequestManager.RequestPath (new PathRequest (transform.position, target.position, OnPathFound));
                targetPosOld = target.position;
            }
        }
    }

    IEnumerator FollowPath () {
        bool followingPath = true;
        int pathIndex = 0;
        transform.LookAt (path.lookPoints[0]);

        float speedPercent = 1;

        while (followingPath) {
            Vector2 pos = transform.position;
            while (path.turnBoundaries[pathIndex].HasCrossedLine (pos)) {
                if (pathIndex == path.finishLineIndex) {
                    followingPath = false;
                    break;
                } else {
                    pathIndex++;
                }
            }

            if (followingPath) {
                if (pathIndex >= path.slowDownIndex && stoppingDst > 0) {
                    speedPercent = Mathf.Clamp01 (path.turnBoundaries[path.finishLineIndex].DistanceFromPoint (pos) / stoppingDst);
                    if (speedPercent < 0.01f) {
                        followingPath = false;
                    }
                }
                //TODO LOOOK UP QUATS IN 2D
                // Quaternion targetRotation = Vector2.LookRotation ((Vector3) path.lookPoints[pathIndex] - transform.position);
                // transform.rotation = Quaternion.Lerp (transform.rotation, targetRotation, Time.deltaTime * turnSpeed);
                transform.right = Vector3.Lerp (transform.right, ((Vector3) path.lookPoints[pathIndex] - transform.position), Time.deltaTime * turnSpeed);
                transform.Translate (Vector2.up * Time.deltaTime * speed * speedPercent, Space.Self);
            }

            yield return null;
        }
    }

    public void OnDrawGizmos () {
        if (path != null) {
            path.DrawWithGizmos ();
        }
    }

}