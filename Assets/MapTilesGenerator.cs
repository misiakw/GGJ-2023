using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.SceneManagement;
using UnityEngine;

public class MapTilesGenerator : MonoBehaviour
{
    public GameObject rootPrefab;
    public GameObject branchPrefab;
    public GameObject mapTilePrefab;

    enum ObstacleConfigurations 
    {
        None,
        Root,
        Branch,
        RootBranch,
        ThreeRoots,
        ThreeBranches,
        //NoFloor,
    }

    int possibleConfigurations = 0;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("GenerateMapTile", 0, 2);
        possibleConfigurations = Enum.GetNames(typeof(ObstacleConfigurations)).Count();
    }

    // Update is called once per frame
    void Update()
    {
    }

    void GenerateMapTile()
    {
        GameObject mapTile = Instantiate(mapTilePrefab);
        GenerateObstacles(mapTile);
    }

    void GenerateObstacles(GameObject mapTile)
    {
        GameObject go;
        switch((ObstacleConfigurations)UnityEngine.Random.Range(0, possibleConfigurations))
        {
            case ObstacleConfigurations.Root:
                CreateObject(rootPrefab, mapTile);
                break;
            case ObstacleConfigurations.Branch:
                CreateObject(branchPrefab, mapTile);
                break;
            case ObstacleConfigurations.RootBranch:
                CreateObject(branchPrefab, mapTile);
                CreateObject(rootPrefab, mapTile);
                break;
            case ObstacleConfigurations.ThreeRoots:
                CreateObject(rootPrefab, mapTile);
                go = CreateObject(rootPrefab, mapTile);
                go.transform.localPosition += new Vector3(1, 0, 0);
                go = CreateObject(rootPrefab, mapTile);
                go.transform.localPosition += new Vector3(2, 0, 0);
                break;
            case ObstacleConfigurations.ThreeBranches:
                CreateObject(branchPrefab, mapTile);
                go = CreateObject(branchPrefab, mapTile);
                go.transform.localPosition += new Vector3(1, 0, 0);
                go = CreateObject(branchPrefab, mapTile);
                go.transform.localPosition += new Vector3(2, 0, 0);
                break;
            //case ObstacleConfigurations.NoFloor:
            //    Destroy(mapTile.Find("Floor"));
            //    break;
        }
    }

    GameObject CreateObject(GameObject prefab, GameObject parent)
    {
        GameObject go = Instantiate(prefab);
        go.transform.SetParent(parent.transform, false);
        return go;
    }
}
