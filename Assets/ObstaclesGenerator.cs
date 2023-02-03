using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObstaclesGenerator : MonoBehaviour
{
    public GameObject obstaclePrefab;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("GenerateObstacle", 0, 1);
    }

    // Update is called once per frame
    void Update()
    {
    }

    void GenerateObstacle()
    {
        GameObject newObstacle = Instantiate(obstaclePrefab);
    }
}
