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
        //RootBranch,
        TwoRoots,
        TwoBranches,
        NoFloor,
        ThreeCurrencies,
        RandomCurrencies
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

    public void GenerateMapTile(GameObject mapTile)
    {
        GlobalStore.Score.Value++;
        previousMapTileYPos = Random.Range(-1, 2); //Comment this line to make single level tiles
        if (LastCreated.transform.position.y + previousMapTileYPos > 3)
            previousMapTileYPos = -1;

        if (LastCreated.transform.position.y + previousMapTileYPos <= -1)
            previousMapTileYPos = 1;

        mapTile.transform.position += new Vector3(0, LastCreated.transform.position.y - mapTile.transform.position.y + previousMapTileYPos, 0);

        GenerateObstacles(mapTile);
        LastCreated.transform.Find("FloorRight/FloorElement (2)/SpriteRight").gameObject.SetActive(ShowRightEdgeIfNecessary(LastCreated, mapTile));
        mapTile.transform.Find("FloorLeft/FloorElement (1)/SpriteLeft").gameObject.SetActive(ShowLeftEdgeIfNecessary(LastCreated, mapTile));
        LastCreated = mapTile;
    }

    bool ShowRightEdgeIfNecessary(GameObject leftObject, GameObject rightObject)
    {
        bool isDifferentYPosition = leftObject.transform.position.y != rightObject.transform.position.y;
        bool rightObjectHasLeftHole = rightObject.GetComponent<MapTileController>().HolePosition == 1;
        return isDifferentYPosition || rightObjectHasLeftHole;
    }

    bool ShowLeftEdgeIfNecessary(GameObject leftObject, GameObject rightObject)
    {
        bool isDifferentYPosition = leftObject.transform.position.y != rightObject.transform.position.y;
        bool leftObjectHasRightHole = leftObject.GetComponent<MapTileController>().HolePosition == 2;
        return isDifferentYPosition || leftObjectHasRightHole;
    }

    void GenerateObstacles(GameObject mapTile)
    {
        GameObject go;
        ObstacleConfigurations newObstacleConfiguration;
        do
        {
            newObstacleConfiguration = (ObstacleConfigurations)Random.Range(0, possibleConfigurations + 2);
        } while (newObstacleConfiguration == previousObstacleConfiguration);
        previousObstacleConfiguration = newObstacleConfiguration;
        switch (newObstacleConfiguration)
        {
            case ObstacleConfigurations.Root:
                CreateObject(rootPrefab, mapTile);
                break;
            case ObstacleConfigurations.Branch:
                go = CreateObject(branchPrefab, mapTile);
                go.transform.localPosition += new Vector3(0, Random.Range(0, 1f), 0);
                break;
            //case ObstacleConfigurations.RootBranch:
            //    go = CreateObject(branchPrefab, mapTile);
            //    go.transform.localPosition += new Vector3(0, 3, 0);
            //    CreateObject(rootPrefab, mapTile);
            //    break;
            case ObstacleConfigurations.TwoRoots:
                go = CreateObject(rootPrefab, mapTile);
                go.transform.localPosition += new Vector3(-2.5f, 0, 0);
                go = CreateObject(rootPrefab, mapTile);
                go.transform.localPosition += new Vector3(2.5f, 0, 0);
                break;
            case ObstacleConfigurations.TwoBranches:
                go = CreateObject(branchPrefab, mapTile);
                go.transform.localPosition += new Vector3(-1, Random.Range(0, 1f), 0);
                go = CreateObject(branchPrefab, mapTile);
                go.transform.localPosition += new Vector3(1, Random.Range(0, 1f), 0);
                break;
            case ObstacleConfigurations.NoFloor:
                mapTile.GetComponent<MapTileController>().MakeHole();
                break;
            case ObstacleConfigurations.ThreeCurrencies:
                go = CreateObject(currencyPrefab, mapTile);
                go.transform.localPosition += new Vector3(2, 0, 0);
                go = CreateObject(currencyPrefab, mapTile);
                go.transform.localPosition += new Vector3(5, 4, 0);
                go = CreateObject(currencyPrefab, mapTile);
                go.transform.localPosition += new Vector3(8, 0, 0);
                break;
            case ObstacleConfigurations.RandomCurrencies:
                go = CreateObject(currencyPrefab, mapTile);
                go.transform.localPosition += new Vector3(2, Random.Range(0, 5), 0);
                go = CreateObject(currencyPrefab, mapTile);
                go.transform.localPosition += new Vector3(5, Random.Range(0, 5), 0);
                go = CreateObject(currencyPrefab, mapTile);
                go.transform.localPosition += new Vector3(8, Random.Range(0, 5), 0);
                break;
        }

        if (newObstacleConfiguration != ObstacleConfigurations.ThreeCurrencies
            && newObstacleConfiguration != ObstacleConfigurations.RandomCurrencies
            && Random.Range(0, 100) < 90) //generation % for currency
        {
            go = CreateObject(currencyPrefab, mapTile);
            go.transform.localPosition += new Vector3(Random.Range(1, 9), 0, 0);
        }
    }

    GameObject CreateObject(GameObject prefab, GameObject parent)
    {
        GameObject go = Instantiate(prefab);
        go.transform.SetParent(parent.transform, false);
        return go;
    }
}
