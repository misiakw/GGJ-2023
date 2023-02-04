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
    public GameObject floorPrefab;

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

    private List<GameObject> mapTilesList = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("GenerateMapTile", 0, 2);
        GenerateBackgroundObject();
        possibleConfigurations = Enum.GetNames(typeof(ObstacleConfigurations)).Count();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void GenerateMapTile()
    {
        GameObject mapTile = Instantiate(mapTilePrefab);
        mapTilesList.Add(mapTile);
        previousMapTileYPos = Random.Range(previousMapTileYPos - 0.8f, previousMapTileYPos + 0.8f); //Comment this line to make single level tiles
        previousMapTileYPos = Math.Min(previousMapTileYPos, 9);
        previousMapTileYPos = Math.Max(previousMapTileYPos, -3);
        mapTile.gameObject.GetComponent<MapTileController>().yPos = previousMapTileYPos;
        GenerateObstacles(mapTile);
    }

    private List<string> backgroundImages = new List<string> { "bg1", "bg2", "bg3"};

    void GenerateBackgroundObject()
    {
        //SpriteRenderer sr = new SpriteRenderer();
        //sr.sprite = Resources.Load<Sprite>("EnvironmentImages/Background/" + backgroundImages[Random.Range(0, backgroundImages.Count)]);
        //GameObject go = sr.gameObject;
        //Instantiate(go);

    }

    void GenerateObstacles(GameObject mapTile)
    {
        GameObject go;
        ObstacleConfigurations newObstacleConfiguration;
        do
        {
            newObstacleConfiguration = (ObstacleConfigurations)Random.Range(0, possibleConfigurations);
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
                mapTile.transform.Find("Floor").localScale = new Vector3(5, 1, 1);
                break;
        }
    }

    GameObject CreateObject(GameObject prefab, GameObject parent)
    {
        GameObject go = Instantiate(prefab);
        go.transform.SetParent(parent.transform, false);
        return go;
    }
}
