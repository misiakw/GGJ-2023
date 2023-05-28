using System;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D rb;
    private MoveConsts moveConsts;
    public float jumpStrength;
    public StartAnimation startAnimation;
    private AudioSource _jumpSound;
    private AudioSource _dieSound;
    private AudioSource _walkSound;
    private AudioSource _pickupSound;
    private Animator animator;

    private GameState currentState = GlobalStore.State.Value;
    private bool isRunning => currentState == GameState.Running;
    private bool canDash = false;
    private bool isDashing = false;
    private bool canDoubleJump = true;
    private float elapsedTimeFromBirth = 0;
    private const int timeBeforeSpeedIncrease = 20;

    private ControllerDevice controller = ControllerDevice.Instance;

    void Start()
    {
        moveConsts = MoveConsts.instance;
        jumpStrength = moveConsts.jumpStrength;

        //state changes
        GlobalStore.State.Onchange += onStateChange;
        GlobalStore.Score.Value = 0;
        GlobalStore.ObstacleVelocity.Value = GlobalStore.PLAYER_STARTUP_SPEED;
        GlobalStore.State.Value = GameState.Loading;
        elapsedTimeFromBirth = 0;

        rb = GetComponent<Rigidbody2D>();

        controller.OnCrouchEnter += onShrink;
        controller.OnCrouchLeave += onGrow;
        controller.OnJumpStart += onJumpStart;
        controller.OnDashStart += onDashStart;

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
                if (animator != null)
                    animator.SetTrigger("StartRunning");
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
        animator.SetBool("Crouching", true);
        gameObject.GetComponent<Rigidbody2D>().gravityScale = moveConsts.squashedGravity;
    }

    void onGrow(object sender, EventArgs args)
    {
        if (!isRunning)
        {
            return;
        }
        animator.SetBool("Crouching", false);
        gameObject.GetComponent<Rigidbody2D>().gravityScale = moveConsts.gravity;
    }

    void onDashStart(object sender, EventArgs args)
    {
        if (!moveConsts.enableDashing || !isRunning || !canDash || isDashing)
        {
            return;
        }
        canDash = false;
        isDashing = true;
        rb.velocity = new Vector2(rb.velocity.x, 0);
        gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
        GlobalStore.ObstacleVelocity.Value = new Vector3(GlobalStore.ObstacleVelocity.Value.x - moveConsts.dashSpeed, 0, 0);
        Invoke("DashStop", moveConsts.dashTime);
    }

    void DashStop()
    {
        gameObject.GetComponent<Rigidbody2D>().gravityScale = moveConsts.gravity;
        GlobalStore.ObstacleVelocity.Value = new Vector3(GlobalStore.ObstacleVelocity.Value.x + moveConsts.dashSpeed, 0, 0);
        isDashing = false;
    }

    public void onJumpStart(object sender, EventArgs args)
    {
        if (currentState == GameState.Loading)
        {
            var playerScoreWindow = GameObject.FindGameObjectsWithTag("PlayerScoreWindow");
            if (!playerScoreWindow.Any() || playerScoreWindow[0].activeSelf == false)
            {
                GlobalStore.ObstacleVelocity.Value = GlobalStore.PLAYER_STARTUP_SPEED;
                elapsedTimeFromBirth = 0;
                GlobalStore.State.Value = GameState.Running;
            }
            return;
        }
        if (isRunning && canDoubleJump)
        {
            animator.SetBool("Jumping", true);
            _walkSound.Stop();
            if (floorsTouching == 0)
            {
                canDoubleJump = false;
            }
            rb.velocity = new Vector2(rb.velocity.x, jumpStrength);
            _jumpSound.Play();
        }


    }
    #endregion

    public void RestartGame(bool forceStateChange = false)
    {
        DestroyGameObjects();
        SceneManager.LoadScene("MainScene");
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
        if (currentState == GameState.Running)
        {
            ManageGameSpeed();
        }
    }

    void ManageGameSpeed()
    {
        elapsedTimeFromBirth += Time.deltaTime;
        if (elapsedTimeFromBirth >= timeBeforeSpeedIncrease)
        {
            elapsedTimeFromBirth = 0;
            GlobalStore.ObstacleVelocity.Value = new Vector3(
                GlobalStore.ObstacleVelocity.Value.x * moveConsts.speedIncreaseMultiplier,
                GlobalStore.ObstacleVelocity.Value.y,
                0);
        }
    }

    private int floorsTouching = 0;

    void OnCollisionEnter2D(Collision2D collided)
    {
        if (collided.otherCollider is BoxCollider2D && collided.gameObject.tag == "Floor")
        {
            floorsTouching++;
            canDoubleJump = true;
            canDash = true;
            animator.SetBool("Jumping", false);
            if (isRunning)
            {
                _walkSound.Play();
            }
        }
    }

    void OnCollisionExit2D(Collision2D collided)
    {
        if (collided.otherCollider is BoxCollider2D && collided.gameObject.tag == "Floor")
        {
            floorsTouching--;
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
        controller.OnDashStart -= onDashStart;
    }
}
