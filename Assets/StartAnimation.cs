using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartAnimation : MonoBehaviour
{

    public GameObject PlayerPrefab;

    private Animator animator;
    private bool playerCreated = false;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!playerCreated && isAimationFinished())
        {
            Debug.Log("animation finished");
            playerCreated = true;
            var player = Instantiate(PlayerPrefab, new Vector3(-8, 12.5f, 0), new Quaternion());
            
            
        }
        Move();
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
