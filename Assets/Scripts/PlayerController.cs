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
        controller.OnDashStart += DashStart;

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
        gameObject.GetComponent<Rigidbody2D>().gravityScale = 10;
    }

    void Grow(object sender, EventArgs args)
    {
        if (!GlobalStore.ShouldScrollScreen())
        {
            return;
        }
        gameObject.transform.localScale = new Vector3(1, 1, 1);
        gameObject.GetComponent<Rigidbody2D>().gravityScale = 4;
    }

    void DashStart(object sender, EventArgs args)
    {
        if (!GlobalStore.ShouldScrollScreen())
        {
            return;
        }
        GlobalStore.IsDashing = true;
        Invoke("DashStop", 0.2f);
    }

    void DashStop()
    {
        GlobalStore.IsDashing = false;
    }

    public void OnJumpStart(object sender, EventArgs args)
    {
        if (GlobalStore.GameState == GameState.Loading)
        {
            GlobalStore.GameState = GameState.Running;
        }
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
        DashStop();
        Destroy(startAnimation.previousPlayer);

        var newRoot = Instantiate(startAnimation.gameObject, new Vector3(-12.57f, -3.56f, 0), new Quaternion());
        startAnimation = newRoot.GetComponent<StartAnimation>();

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
            
            if(GlobalStore.ShouldScrollScreen()) 
            {
                _walkSound.Play();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collided)
    {
        if((collided.tag == "Obstacle" || collided.tag == "DeathZone") && GlobalStore.GameState == GameState.Running) 
        {
            _walkSound.Stop();
            _dieSound.Play();
            GlobalStore.GameState = GameState.Died;
            if(GlobalStore.HighestScore < GlobalStore.Score) 
            {
                GlobalStore.HighestScore = GlobalStore.Score;
            }
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
