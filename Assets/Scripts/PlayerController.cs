using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public Rigidbody rb;
    public float jumpStrength = 4.5F;
    public bool canJump = false;
    public GameObject BodyGameObject;
    public StartAnimation startAnimation;
    private AudioSource _jumpSound;
    private AudioSource _dieSound;
    private AudioSource _walkSound;

    private ControllerDevice controller = ControllerDevice.Instance;

    void Start()
    {

        GlobalStore.Score = 0;
        controller.OnCrouchEnter += Shrink;
        controller.OnCrouchLeave += Grow;
        controller.OnJumpStart += OnJumpStart;

        var audioSources = gameObject.GetComponentsInChildren<AudioSource>();

        foreach (AudioSource source in audioSources)
        {
            switch (source.name)
            {
                case "JumpSound":
                    _jumpSound = source; break;
                case "DieSound":
                    _dieSound = source; break;
                case "WalkSound":
                    _walkSound = source; break;
            }
        }

    }

    void Shrink(object sender, EventArgs args)
    {
        if (GlobalStore.GameState == GameState.Died)
        {
            return;
        }
        gameObject.transform.localScale = new Vector3(1, 0.5f, 1);
    }

    void Grow(object sender, EventArgs args)
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
            _walkSound.Stop();
            canJump = false;
            rb.AddForce(0, jumpStrength * 100, 0);
            _jumpSound.Play();
        }
        if (GlobalStore.GameState == GameState.Died)
        {
            RestartGame();
        }
    }

    public void RestartGame()
    {
        DestroyObstacles();
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
            if(GlobalStore.GameState == GameState.Running) 
            {
                _walkSound.Play();
            }
        }
    }

    private void OnTriggerEnter(Collider collided)
    {
        if(collided.tag == "Obstacle" && GlobalStore.GameState == GameState.Running) 
        {
            _walkSound.Stop();
            _dieSound.Play();
            GlobalStore.GameState = GameState.Died;
            BodyGameObject.transform.eulerAngles = new Vector3(0,0,-90);
        }
    }

    public void OnDestroy()
    {
        controller.OnCrouchEnter -= Shrink;
        controller.OnCrouchLeave -= Grow;
        controller.OnJumpStart -= OnJumpStart;
    }
}
