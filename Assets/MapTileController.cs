using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MapTileController : MonoBehaviour
{
    public int height = 5;
    public float yPos = 0;
    public float xPos = 0;
    public GameObject FloorLeft;
    public GameObject FloorRight;

    public GameObject LeftFull;
    public GameObject LeftRising;
    public GameObject LeftFalling;

    public MapTilesGenerator generator;
    private bool alreadyGenerated = false;
    private bool isRunning = GlobalStore.State.Value == GameState.Running;

    // Start is called before the first frame update
    void Start()
    {
        GlobalStore.State.Onchange += (s, v) => isRunning = v == GameState.Running;
    }

    // Update is called once per frame
    void Update()
    {
        if (GlobalStore.State.Value == GameState.Running)
        {
            if (transform.position.x <= -30)
            {
                GenerateNewTile();
                Destroy(gameObject);
                return;
            }

            transform.position += new Vector3(GlobalStore.ObstacleVelocity.Value.x * Time.deltaTime, 0, 0);
        }
    }

    public void MakeHole()
    {
        if (Random.Range(1, 2) == 1)
        {
            Destroy(FloorLeft);
        }
        else
        {
            Destroy(FloorRight);
        }
    }

    public void GenerateNewTile()
    {
        if (!alreadyGenerated)
        {
            alreadyGenerated = true;
            generator.GenerateMapTile(transform.position);
        }
    }
}
