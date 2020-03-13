using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    Rigidbody2D body;
    MapGenerator map;
    SpriteRenderer spriteRenderer;
    public GameObject agentPrefab;

    float horizontal, vertical;

    public float movSpd = 20.0f;

    public Vector2 worldPos;

    void Awake () {
        body = GetComponent<Rigidbody2D> ();
        spriteRenderer = GetComponentInChildren<SpriteRenderer> ();
    }

    void Start () {
        map = MapGenerator.instance;

        //gameObject.transform.position = new Vector2 (map.mapWidth, map.mapHeight) * 0.5f;
        CalculateWorldPos ();
    }

    // Update is called once per frame
    void Update () {
        horizontal = Input.GetAxisRaw ("Horizontal");
        vertical = Input.GetAxisRaw ("Vertical");

        CalculateWorldPos ();

        if (Input.GetKeyDown (KeyCode.Space)) {

            Debug.Log (map.mapData.heightMap[(int) worldPos.x, (int) worldPos.y]);
            PlaceAgent ();
        }

        if (horizontal > 0) {
            spriteRenderer.flipX = false;
        } else if (horizontal < 0) {
            spriteRenderer.flipX = true;
        }

    }

    void PlaceAgent () {
        SimplePool.Spawn (agentPrefab, new Vector3 (gameObject.transform.position.x, gameObject.transform.position.y, 0), Quaternion.identity);
    }

    private void FixedUpdate () {

        body.velocity = new Vector2 (horizontal * movSpd, vertical * movSpd);

    }

    void CalculateWorldPos () {
        worldPos = new Vector2 (
            Mathf.RoundToInt (gameObject.transform.position.x),
            Mathf.RoundToInt (gameObject.transform.position.y)
        );
    }

}