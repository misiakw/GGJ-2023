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

    public MapTilesGenerator generator;
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
        foreach (var child in gameObject.GetComponentsInChildren<Transform>(true))
        {
            if (child.tag == "Obstacle" || child.tag == "Currency")
            {
                Destroy(child.gameObject);
            }
            else if(child.tag == "Floor")
            {
                child.gameObject.SetActive(true);
            }
        }
    }
}
