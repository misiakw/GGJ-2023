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

    private Vector3 _defaultObstacleVelocity = new Vector3(-5, 0, 0);
    private Vector3 _stoppedObstacleVelocity = new Vector3(0, 0, 0);

    private List<string> platformImages = new List<string> { "Platform 01 Ground1", "Platform 02 Ground2" };

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(transform.position.x, yPos, 0);    
        rb.velocity = _defaultObstacleVelocity;

        floor = transform.Find("Floor").gameObject;
        //floor.transform.Find("Sprite").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("EnvironmentImages/MainPlan/" + platformImages[Random.Range(0, platformImages.Count)]);
        //floor.transform.Find("Sprite2").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("EnvironmentImages/MainPlan/" + platformImages[Random.Range(0, platformImages.Count)]);
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
