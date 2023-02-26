using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StartAnimation : MonoBehaviour
{

    public GameObject PlayerPrefab;

    private Animation animator;
    private bool playerCreated = false;
    public GameObject previousPlayer;

    private bool ShouldStartGrowing = true;
    private GameState currentState;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animation>();

        GlobalStore.State.Onchange += (s, v) => currentState = v;
    }

    // Update is called once per frame
    void Update()
    {
        /*if (!ShouldStartGrowing)
        {
            var hit = Physics2D.Raycast(new Vector2(-8, -5f), Vector2.up);

            if (hit.collider == null)
            {
                GlobalStore.State.Value = GameState.Running;
            }
            else
            {
                GlobalStore.State.Value = GameState.Loading;
                ShouldStartGrowing = true;
            }

            animator.Stop();
        }*/

        DropPlayer();
        Move();
    }

    private void DropPlayer()
    {
        if (!playerCreated && ShouldStartGrowing && isAimationFinished())
        {
            Debug.Log("animation finished");
            playerCreated = true;
            previousPlayer = Instantiate(PlayerPrefab, new Vector3(-8, 6.5f, 0), new Quaternion());
            previousPlayer.GetComponent<PlayerController>().startAnimation = this;
        }
    }

    public void StartGrow()
    {
        return;
        GlobalStore.State.Value = GameState.Loading;
        transform.position = new Vector3(-12.57f, -3.57f, 0f);
        playerCreated = false;
        if (previousPlayer != null && !previousPlayer.IsDestroyed())
        {
            Destroy(previousPlayer);
            previousPlayer = null;
        }
        transform.position = new Vector3(-12.57f, -3.56f, 0);
        animator.Play();
        GetComponent<AudioSource>().Play();
    }

    private void Move()
    {
        if (currentState == GameState.Running)
        {
            var speed = new Vector3(GlobalStore.ObstacleVelocity.Value.x * Time.deltaTime, 0, 0);
            transform.position += speed;
        }
    }

    private bool isAimationFinished()
    {
        return true;
        var state = GetComponent<Animator>().GetCurrentAnimatorStateInfo(0);
        return state.length / 2 <= state.normalizedTime;
    }
}
