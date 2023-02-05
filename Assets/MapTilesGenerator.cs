using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class MapTilesGenerator : MonoBehaviour
{
    public GameObject rootPrefab;
    public GameObject branchPrefab;
    public GameObject mapTilePrefab;
    public GameObject currencyPrefab;

    enum ObstacleConfigurations 
    {
        None,
        Root,
        Branch,
        RootBranch,
        TwoRoots,
        TwoBranches,
        NoFloor,
    }

    private ObstacleConfigurations previousObstacleConfiguration;
    private float previousMapTileYPos = 0;
    int possibleConfigurations = 0;

    public GameObject LastCreated;

    // Start is called before the first frame update
    void Start()
    {
        possibleConfigurations = Enum.GetNames(typeof(ObstacleConfigurations)).Count();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GenerateMapTile(Vector3 callerTransform)
    {
        GameObject mapTile = Instantiate(mapTilePrefab);
        previousMapTileYPos = Random.Range(-1, 2); //Comment this line to make single level tiles
        if (LastCreated.transform.position.y + previousMapTileYPos > 5)
            previousMapTileYPos = -1;

        if (LastCreated.transform.position.y - previousMapTileYPos <= -1)
            previousMapTileYPos = 1;

        var controller = mapTile.gameObject.GetComponent<MapTileController>();
        controller.generator = this;
        mapTile.transform.position = LastCreated.transform.position + new Vector3(9.9f, (float)previousMapTileYPos, 0);
        LastCreated = mapTile;
        GenerateObstacles(mapTile);
    }

    private List<string> backgroundImages = new List<string> { "bg1", "bg2", "bg3"};

    void GenerateObstacles(GameObject mapTile)
    {
        GameObject go;
        ObstacleConfigurations newObstacleConfiguration;
        do
        {
            newObstacleConfiguration = (ObstacleConfigurations)Random.Range(0, possibleConfigurations + 3);
        } while (newObstacleConfiguration == previousObstacleConfiguration);
        previousObstacleConfiguration = newObstacleConfiguration;

        switch (newObstacleConfiguration)
        {
            case ObstacleConfigurations.Root:
                CreateObject(rootPrefab, mapTile);
                break;
            case ObstacleConfigurations.Branch:
                CreateObject(branchPrefab, mapTile);
                break;
            case ObstacleConfigurations.RootBranch:
                go = CreateObject(branchPrefab, mapTile);
                go.transform.localPosition += new Vector3(0, 3, 0);
                CreateObject(rootPrefab, mapTile);
                break;
            case ObstacleConfigurations.TwoRoots:
                go = CreateObject(rootPrefab, mapTile);
                go.transform.localPosition += new Vector3(-2.5f, 0, 0);
                go = CreateObject(rootPrefab, mapTile);
                go.transform.localPosition += new Vector3(2.5f, 0, 0);
                break;
            case ObstacleConfigurations.TwoBranches:
                go = CreateObject(branchPrefab, mapTile);
                go.transform.localPosition += new Vector3(-1, 0, 0);
                go = CreateObject(branchPrefab, mapTile);
                go.transform.localPosition += new Vector3(1, 0, 0);
                break;
            case ObstacleConfigurations.NoFloor:
                mapTile.GetComponent<MapTileController>().MakeHole();
                break;
        }

        if (Random.Range(0, 100) < 80) //generation % for currency
        {
            go = CreateObject(currencyPrefab, mapTile);
        }
    }

    GameObject CreateObject(GameObject prefab, GameObject parent)
    {
        GameObject go = Instantiate(prefab);
        go.transform.SetParent(parent.transform, false);
        return go;
    }
}
