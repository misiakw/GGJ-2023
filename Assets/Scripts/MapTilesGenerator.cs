using Assets.Scripts;
using Assets.Scripts.MapTileFactory;
using System;
using System.Collections.Generic;
using System.IO;
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
    int currentTile = 0;
    string[] tilesets;

    public GameObject LastCreated;

    // Start is called before the first frame update
    void Start()
    {
        tilesets = LoadLevels();
        PrefabStorage.SetPrefabs(currencyPrefab, rootPrefab, branchPrefab);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GenerateMapTile(GameObject mapTile)
    {
        GlobalStore.Score.Value++;
        //previousMapTileYPos = Random.Range(-1, 2); //Comment this line to make single level tiles
        if (LastCreated.transform.position.y + previousMapTileYPos > 3)
            previousMapTileYPos = -1;

        if (LastCreated.transform.position.y + previousMapTileYPos <= -1)
            previousMapTileYPos = 1;

        mapTile.transform.position += new Vector3(0, LastCreated.transform.position.y - mapTile.transform.position.y + previousMapTileYPos, 0);

        GenerateObstacles(mapTile);
        LastCreated = mapTile;
    }

    void GenerateObstacles(GameObject mapTile)
    {
        switch(tilesets[currentTile % tilesets.Length])
        {
            case "Jump1":
                JumpMapTileFactory.GenerateTile(mapTile, 1);
                break;
            case "Jump2":
                JumpMapTileFactory.GenerateTile(mapTile, 2);
                break;
            case "Jump3":
                JumpMapTileFactory.GenerateTile(mapTile, 3);
                break;
            case "Jump4":
                JumpMapTileFactory.GenerateTile(mapTile, 4);
                break;
            case "DoubleJump1":
                DoubleJumpMapTileFactory.GenerateTile(mapTile, 1);
                break;
            case "DoubleJump2":
                DoubleJumpMapTileFactory.GenerateTile(mapTile, 2);
                break;
            case "DoubleJump3":
                DoubleJumpMapTileFactory.GenerateTile(mapTile, 3);
                break;
            case "DoubleJump4":
                DoubleJumpMapTileFactory.GenerateTile(mapTile, 4);
                break;
            case "Duck1":
                DuckMapTileFactory.GenerateTile(mapTile, 1);
                break;
            case "Duck2":
                DuckMapTileFactory.GenerateTile(mapTile, 2);
                break;
            case "Duck3":
                DuckMapTileFactory.GenerateTile(mapTile, 3);
                break;
            case "JumpDuck1":
                JumpDuckMapTileFactory.GenerateTile(mapTile, 1);
                break;
            case "JumpDuck2":
                JumpDuckMapTileFactory.GenerateTile(mapTile, 2);
                break;
            case "JumpDuck3":
                JumpDuckMapTileFactory.GenerateTile(mapTile, 3);
                break;
            case "JumpDuck4":
                JumpDuckMapTileFactory.GenerateTile(mapTile, 4);
                break;
            case "JumpDuck5":
                JumpDuckMapTileFactory.GenerateTile(mapTile, 5);
                break;
            case "JumpDuck6":
                JumpDuckMapTileFactory.GenerateTile(mapTile, 6);
                break;
            case "Other1":
                OtherMapTileFactory.GenerateTile(mapTile, 1);
                break;
            case "Other2":
                OtherMapTileFactory.GenerateTile(mapTile, 2);
                break;
            case "Other3":
                OtherMapTileFactory.GenerateTile(mapTile, 3);
                break;
        }
        currentTile++;
    }

    private string[] LoadLevels()
    {
        var filePath = Path.Combine(Application.dataPath, "Levels.txt");
        string[] levels;
        if (File.Exists(filePath))
        {
            return File.ReadAllLines(filePath);
        }
        
        return new string[] { "Jump1", "DoubleJump1", "DoubleJump1", "Duck1", "Duck1", "JumpDuck1", "JumpDuck1", "JumpDuck1", "Duck2", "DoubleJump2", "DoubleJump", "JumpDuck2", "JumpDuck3", "JumpDuck4", "Other1", "Duck3", "Jump3" };
    }
}
