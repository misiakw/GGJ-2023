using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D rb;
    public float jumpStrength = 7F;
    public bool canJump = false;
    public GameObject BodyGameObject;
    public StartAnimation startAnimation;
    private AudioSource _jumpSound;
    private AudioSource _dieSound;
    private AudioSource _walkSound;
    private AudioSource _pickupSound;

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
                case "PickupSound":
                    _pickupSound = source; break;
            }
        }

    }

    void Shrink(object sender, EventArgs args)
    {
        if (!GlobalStore.ShouldScrollScreen())
        {
            return;
        }
        gameObject.transform.localScale = new Vector3(1, 0.5f, 1);
    }

    void Grow(object sender, EventArgs args)
    {
        if (!GlobalStore.ShouldScrollScreen())
        {
            return;
        }
        gameObject.transform.localScale = new Vector3(1, 1, 1);
    }

    public void OnJumpStart(object sender, EventArgs args)
    {
        if (GlobalStore.ShouldScrollScreen() && canJump)
        {
            _walkSound.Stop();
            canJump = false;
            rb.velocity += new Vector2(0, jumpStrength);
            _jumpSound.Play();
        }
    }

    public void RestartGame(bool forceStateChange = false)
    {

        DestroyGameObjects();
        startAnimation.StartGrow();
        BodyGameObject.transform.eulerAngles = new Vector3(0, 0, 0);
    }

    private static void DestroyGameObjects()
    {
        DestroyCurrencies();
        DestroyObstacles();
    }

    private static void DestroyCurrencies()
    {
        var currencies = GameObject.FindGameObjectsWithTag("Currency");
        foreach (var currency in currencies)
        {
            Destroy(currency);
        }
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
            if (GlobalStore.GameState == GameState.Loading)
            {
                GlobalStore.GameState = GameState.Running;
            }
            
            if(GlobalStore.ShouldScrollScreen()) 
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
            RestartGame();
        }
        if(collided.tag == "Currency" && GlobalStore.GameState == GameState.Running)
        {
            _pickupSound.Play();
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
