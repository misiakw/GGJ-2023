using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapTileController : MonoBehaviour
{
    public int height = 5;
    public float yPos = 0;
    //private GameObject ceiling;
    public Rigidbody rb;

    private Vector3 _defaultObstacleVelocity = new Vector3(-5, 0, 0);
    private Vector3 _stoppedObstacleVelocity = new Vector3(0, 0, 0);

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(transform.position.x, yPos, 0);    
        rb.velocity = _defaultObstacleVelocity;
        //ceiling = transform.Find("Ceiling").gameObject;
        //ceiling.transform.localPosition = new Vector3(0, height, 0);
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = GlobalStore.GameState == GameState.Running
            ? _defaultObstacleVelocity
            : GlobalStore.GameState == GameState.Died
            ? _stoppedObstacleVelocity
            : throw new ArgumentException("Unhandled gamestate");
    }
}
