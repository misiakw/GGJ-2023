using System;
using System.Linq;
using UnityEngine;

public class ObstaclesGenerator : MonoBehaviour
{
    public GameObject rootPrefab;
    public GameObject branchPrefab;

    enum ObstacleConfigurations 
    {
        None,
        Root,
        Branch,
        RootBranch,
        //ThreeRoots,
        //ThreeBranches,
    }

    int possibleConfigurations = 0;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("GenerateObstacle", 0, 1);
        possibleConfigurations = Enum.GetNames(typeof(ObstacleConfigurations)).Count();
    }

    // Update is called once per frame
    void Update()
    {
    }

    void GenerateObstacle()
    {
        switch((ObstacleConfigurations)UnityEngine.Random.Range(0, possibleConfigurations + 1))
        {
            case ObstacleConfigurations.Root:
                Instantiate(rootPrefab);
                break;
            case ObstacleConfigurations.Branch:
                Instantiate(branchPrefab);
                break;
            case ObstacleConfigurations.RootBranch:
                Instantiate(rootPrefab);
                Instantiate(rootPrefab);
                break;
            case ObstacleConfigurations.None:
                //Instantiate(rootPrefab);
                break;
        }
    }
}
