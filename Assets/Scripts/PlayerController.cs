using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D rb;
    public float jumpStrength = 5F;
    public bool canJump = true;
    public GameObject BodyGameObject;
    public StartAnimation startAnimation;
    private AudioSource _jumpSound;
    private AudioSource _dieSound;
    private AudioSource _walkSound;

    private ControllerDevice controller = ControllerDevice.Instance;

    void Start()
    {

        GlobalStore.Score = 0;

        rb = GetComponent<Rigidbody2D>();

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
        if (GlobalStore.GameState == GameState.Running)
        {
            _walkSound.Stop();
            //canJump = false;
            rb.velocity += new Vector2(0, jumpStrength);
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

    void OnCollisionEnter2D(Collision2D collided)
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

    private void OnTriggerEnter2D(Collider2D collided)
    {
        if(collided.tag == "Obstacle" && GlobalStore.GameState == GameState.Running) 
        {
            _walkSound.Stop();
            _dieSound.Play();
            GlobalStore.GameState = GameState.Died;
            BodyGameObject.transform.eulerAngles = new Vector3(0,0,-90);
        }
        if(collided.tag == "Currency" && GlobalStore.GameState == GameState.Running)
        {
            GlobalStore.Score += 10;
            Destroy(collided.gameObject);
        }
    }

    public void OnDestroy()
    {
        controller.OnCrouchEnter -= Shrink;
        controller.OnCrouchLeave -= Grow;
        controller.OnJumpStart -= OnJumpStart;
    }
}
