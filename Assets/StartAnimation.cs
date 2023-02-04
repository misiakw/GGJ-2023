using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StartAnimation : MonoBehaviour
{

    public GameObject PlayerPrefab;

    private Animator animator;
    private bool playerCreated = false;
    private GameObject previousPlayer;
    private AudioSource _walkSound;


    // Start is called before the first frame update
    void Start()
    {
        var walkSoundObject = GameObject.Find("WalkSound");
        _walkSound = walkSoundObject.GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!playerCreated && isAimationFinished())
        {
            Debug.Log("animation finished");
            playerCreated = true;
            previousPlayer = Instantiate(PlayerPrefab, new Vector3(-8, 12.5f, 0), new Quaternion());
            previousPlayer.GetComponent<PlayerController>().startAnimation = this;
            GlobalStore.GameState = GameState.Running;
            _walkSound.Play();
        }
        Move();
    }

    public void StartGrow()
    {
        transform.position = new Vector3(-12.57f, -3.57f, 0f);
        playerCreated = false;
        if(previousPlayer!= null && !previousPlayer.IsDestroyed()) {
            Destroy(previousPlayer);
            previousPlayer = null;
        }
        animator.Play("RootAnimation");
        GetComponent<AudioSource>().Play();
    }

    private void Move()
    {
        if (GlobalStore.GameState == GameState.Running)
        {
            var speed = new Vector3(GlobalStore.DefaultObstacleVelocity.x * Time.deltaTime, 0, 0);
            transform.position += speed;
        }
    }

    private bool isAimationFinished()
    {
        var state = animator.GetCurrentAnimatorStateInfo(0);
        return state.length/2 <= state.normalizedTime;
    }
}
