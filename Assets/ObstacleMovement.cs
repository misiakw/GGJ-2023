using System;
using UnityEngine;

public class ObstacleMovement : MonoBehaviour
{
    public Rigidbody rb;

    private Vector3 _defaultObstacleVelocity = new Vector3(-5, 0, 0);
    private Vector3 _stoppedObstacleVelocity = new Vector3(0, 0, 0);

    // Start is called before the first frame update
    void Start()
    {
        rb.velocity = _defaultObstacleVelocity; 
        gameObject.transform.position = new Vector3(20f, UnityEngine.Random.Range(-4, 0), 0);
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
