using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D rb;
    public float jumpStrength = 7F;
    public StartAnimation startAnimation;
    private AudioSource _jumpSound;
    private AudioSource _dieSound;
    private AudioSource _walkSound;
    private AudioSource _pickupSound;
    private Animator animator;

    private GameState currentState = GlobalStore.State.Value;
    private bool isRunning => currentState == GameState.Running;
    private bool isDashing = false;
    private short jumpCounter = 0;
    private float elapsedTimeFromBirth = 0;
    private const int timeBeforeSpeedIncrease = 20;

    private ControllerDevice controller = ControllerDevice.Instance;

    void Start()
    {

        //state changes
        GlobalStore.State.Onchange += onStateChange;
        GlobalStore.Score.Value = 0;

        rb = GetComponent<Rigidbody2D>();

        controller.OnCrouchEnter += onShrink;
        controller.OnCrouchLeave += onGrow;
        controller.OnJumpStart += onJumpStart;
        //controller.OnDashStart += onDashStart;

        animator = GetComponent<Animator>();

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
    #region movement event listeners

    void onStateChange(object sender, GameState state)
    {
        currentState = state;
        switch (state)
        {
            case GameState.Running:
                if(animator != null)
                    animator.SetBool("Running", true);
                break;
            case GameState.Loading:
            case GameState.Died:
                break;
        }
    }

    void onShrink(object sender, EventArgs args)
    {
        if (!isRunning)
        {
            return;
        }
        gameObject.transform.localScale = new Vector3(1, 0.5f, 1);
        animator.SetBool("Crouching", true);
        gameObject.GetComponent<Rigidbody2D>().gravityScale = 10;
    }

    void onGrow(object sender, EventArgs args)
    {
        if (!isRunning)
        {
            return;
        }
        gameObject.transform.localScale = new Vector3(1, 1, 1);
        animator.SetBool("Crouching", false);
        gameObject.GetComponent<Rigidbody2D>().gravityScale = 4;
    }

    /*void onDashStart(object sender, EventArgs args)
    {
        if (!isRunning)
        {
            return;
        }
        isDashing = true;
        Invoke("DashStop", 0.2f);
    }

    void DashStop()
    {
        isDashing = false;
    }*/

    public void onJumpStart(object sender, EventArgs args)
    {
        Debug.Log("jump");
        if (currentState == GameState.Loading)
        {
            GlobalStore.State.Value = GameState.Running;
            return;
        }
        if (isRunning && jumpCounter < 2)
        {
            if (jumpCounter == 0)
            {
                animator.SetBool("Jumping", true);
                rb.velocity = new Vector2(rb.velocity.x, 0);
            }

            _walkSound.Stop();
            jumpCounter++;
            rb.velocity += new Vector2(0, jumpStrength / jumpCounter);
            if (rb.velocity.y > 2*jumpStrength)
                rb.velocity = new Vector2(rb.velocity.x, 2*jumpStrength);
            _jumpSound.Play();
        }


    }
    #endregion

    public void RestartGame(bool forceStateChange = false)
    {
        DestroyGameObjects();
        SceneManager.LoadScene("MainScene");
        //DashStop();
        //Destroy(startAnimation.previousPlayer);
        //GlobalStore.ObstacleVelocity.Value = new Vector3(-6f, 0, 0);

        //var newRoot = Instantiate(startAnimation.gameObject, new Vector3(-12.57f, -3.56f, 0), new Quaternion());
       //startAnimation = newRoot.GetComponent<StartAnimation>();
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
    void FixedUpdate()
    {
        controller.Loop();
        ManageGameSpeed();
    }

    void ManageGameSpeed()
    {
        elapsedTimeFromBirth += Time.deltaTime;
        if (elapsedTimeFromBirth >= timeBeforeSpeedIncrease)
        {
            elapsedTimeFromBirth = 0;
            GlobalStore.ObstacleVelocity.Value = new Vector3(GlobalStore.ObstacleVelocity.Value.x * 1.4f, GlobalStore.ObstacleVelocity.Value.y, 0);
        }
    }

    void OnCollisionEnter2D(Collision2D collided)
    {
        if (collided.gameObject.tag == "Floor")
        {
            if(jumpCounter > 0)
                animator.SetBool("Jumping", false);
            jumpCounter = 0;

            if (isRunning)
            {
                _walkSound.Play();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collided)
    {
        if (currentState == GameState.Running)
        {
            switch (collided.tag)
            {
                case "Obstacle":
                case "DeathZone":
                    _walkSound.Stop();
                    _dieSound.Play();
                    GlobalStore.State.Value = GameState.Died;
                    RestartGame();
                    break;
                case "Currency":
                    _pickupSound.Play();
                    GlobalStore.Score.Value++;
                    Destroy(collided.gameObject);
                    break;
            }
        }
    }

    public void OnDestroy()
    {
        controller.OnCrouchEnter -= onShrink;
        controller.OnCrouchLeave -= onGrow;
        controller.OnJumpStart -= onJumpStart;
        //controller.OnDashStart -= onDashStart;
    }
}
