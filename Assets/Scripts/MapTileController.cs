using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    private bool isRunning = GlobalStore.State.Value == GameState.Running;
    public int HolePosition = 0; //0 - No Hole, 1 - Left Hole, 2 - Right Hole

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
            transform.Translate(new Vector3(GlobalStore.ObstacleVelocity.Value.x * Time.deltaTime, 0, 0));

            if (transform.position.x <= -30)
            {
                ResetTileState();
                generator.GenerateMapTile(gameObject);
                return;
            }
        }
    }

    private void ResetTileState()
    {
        transform.position += new Vector3(60, 0, 0);
        foreach (var child in gameObject.GetComponentsInChildren<Transform>())
        {
            if (child.tag == "Obstacle" || child.tag == "Currency")
            {
                Destroy(child.gameObject);
            }
        }
        FloorLeft.SetActive(true);
        FloorRight.SetActive(true);
        transform.Find("FloorCentre/FloorElement/SpriteLeft").gameObject.SetActive(false);
        transform.Find("FloorCentre/FloorElement (3)/SpriteRight").gameObject.SetActive(false);
        HolePosition = 0;
    }

    public void MakeHole()
    {
        if (Random.Range(0, 2) == 1)
        {
            FloorLeft.SetActive(false);
            transform.Find("FloorCentre/FloorElement/SpriteLeft").gameObject.SetActive(true);
            HolePosition = 1;
        }
        else
        {
            FloorRight.SetActive(false);
            transform.Find("FloorCentre/FloorElement (3)/SpriteRight").gameObject.SetActive(true);
            HolePosition = 2;
        }
    }
}
