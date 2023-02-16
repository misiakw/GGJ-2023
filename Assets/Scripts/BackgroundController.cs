using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    bool isRunning = GlobalStore.State.Value == GameState.Running;
    Vector3 velocity = GlobalStore.ObstacleVelocity.Value;

    void Start()
    {
        GlobalStore.State.Onchange += (s, v) => isRunning = v == GameState.Running;
        GlobalStore.ObstacleVelocity.Onchange += (s, v) => velocity = v;
    }

    // Update is called once per frame
    void Update()
    {
        if (isRunning)
        {
            foreach (Transform child in transform.GetComponentInChildren<Transform>())
            {
                float speed = child.name.StartsWith("Trees") ? 0.3f
                    : child.name.StartsWith("Backtrees") ? 0.2f
                    : 0.1f;
                child.Translate(velocity * Time.deltaTime * speed);
                if (child.localPosition.x <= -19.2)
                {
                    child.localPosition += new Vector3(57.6f, 0, 0);
                }
            }
        }
    }
}
