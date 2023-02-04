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

    private Vector3 _defaultObstacleVelocity = GlobalStore.DefaultObstacleVelocity;
    private Vector3 _stoppedObstacleVelocity = new Vector3(0, 0, 0);

    public MapTilesGenerator generator;

    private List<string> platformImages = new List<string> { "Platform 01 Ground1", "Platform 02 Ground2" };

    // Start is called before the first frame update
    void Start()
    {
        //transform.position = new Vector3(xPos, yPos, 0);    
    }

    // Update is called once per frame
    void Update()
    {
        if(GlobalStore.GameState == GameState.Running)
        {
            if (transform.position.x <= -30)
            {
                GenerateNewTile();
                Destroy(gameObject);
                return;
            }

            transform.position += new Vector3(GlobalStore.DefaultObstacleVelocity.x * Time.deltaTime, 0, 0);
        }
    }

    private bool alreadyGenerated = false;

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
        if(!alreadyGenerated) {
            alreadyGenerated = true;
            generator.GenerateMapTile(transform.position);
        }
    }
}
