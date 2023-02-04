using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MapTileController : MonoBehaviour
{
    public int height = 5;
    public float yPos = 0;
    private GameObject floor;
    public Rigidbody rb;

    private Vector3 _defaultObstacleVelocity = GlobalStore.DefaultObstacleVelocity;
    private Vector3 _stoppedObstacleVelocity = new Vector3(0, 0, 0);

    public MapTilesGenerator generator;

    private List<string> platformImages = new List<string> { "Platform 01 Ground1", "Platform 02 Ground2" };

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(transform.position.x, yPos, 0);    
        //rb.velocity = _defaultObstacleVelocity;

        floor = transform.Find("Floor").gameObject;
        //floor.transform.Find("Sprite").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("EnvironmentImages/MainPlan/" + platformImages[Random.Range(0, platformImages.Count)]);
        //floor.transform.Find("Sprite2").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("EnvironmentImages/MainPlan/" + platformImages[Random.Range(0, platformImages.Count)]);
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


        /*rb.velocity = GlobalStore.GameState == GameState.Running
            ? _defaultObstacleVelocity
            : GlobalStore.GameState == GameState.Died
            ? _stoppedObstacleVelocity
            : throw new ArgumentException("Unhandled gamestate");*/
    }

    private bool alreadyGenerated = false;
    public void GenerateNewTile()
    {
        if(!alreadyGenerated) {
            alreadyGenerated = true;
            generator.GenerateMapTile();
        }
    }
}
