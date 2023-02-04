using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public Rigidbody rb;
    public float jumpStrength = 4.5F;
    public bool canJump = false;
    public GameObject BodyGameObject;
    public StartAnimation startAnimation;

    private ControllerDevice controller = ControllerDevice.Instance;

    void Start()
    {
        controller.OnCrouchEnter += (o, e) => Shrink();
        controller.OnCrouchLeave += (o, e) => Grow();
        controller.OnJumpStart += OnJumpStart; 
    }

    void Shrink()
    {
        if (GlobalStore.GameState == GameState.Died)
        {
            return;
        }
        gameObject.transform.localScale = new Vector3(1, 0.5f, 1);
    }

    void Grow()
    {
        if (GlobalStore.GameState == GameState.Died)
        {
            return;
        }
        gameObject.transform.localScale = new Vector3(1, 1, 1);
    }

    public void OnJumpStart(object sender, EventArgs args)
    {
        if (GlobalStore.GameState == GameState.Running && canJump)
        {
            canJump = false;
            rb.AddForce(0, jumpStrength * 100, 0);
        }
        if (GlobalStore.GameState == GameState.Died)
        {
            RestartGame();
        }
    }

    private void RestartGame()
    {
        DestroyObstacles();

        SceneManager.UnloadScene("MainScene");
        controller.OnCrouchEnter = null;
        controller.OnCrouchLeave = null;
        controller.OnJumpStart = null;
        SceneManager.LoadScene("MainScene");
        startAnimation.StartGrow();
        BodyGameObject.transform.eulerAngles = new Vector3(0, 0, 0);
    }

    private static void DestroyObstacles()
    {
        var obstacles = GameObject.FindGameObjectsWithTag("Obstacle");
        foreach (var obstacle in obstacles)
        {
        Destroy(obstacle);
        }
    }

    // Update is called once per frame
    void Update()
    {
       controller.Loop();
    }

    void OnCollisionEnter(Collision collided)
    {
        if (collided.gameObject.tag == "Floor")
        {
            canJump = true;
        }
    }

    private void OnTriggerEnter(Collider collided)
    {
        if(collided.tag == "Obstacle" && GlobalStore.GameState == GameState.Running) 
        {
            GlobalStore.GameState = GameState.Died;
            BodyGameObject.transform.eulerAngles = new Vector3(0,0,-90);
        }
    }
}
