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
                float speed = 0f;
                switch(true)
                {
                    case bool b when child.name.StartsWith("Rocks"):
                        speed = 0.2f;
                        break;
                    case bool b when child.name.StartsWith("Cloud"):
                        speed = 0.1f;
                        break;
                    case bool b when child.name.StartsWith("Ground"):
                        speed = 0.3f;
                        break;
                }
                child.Translate(velocity * Time.deltaTime * speed);
                if (child.localPosition.x <= -26)
                {
                    child.localPosition += new Vector3(52f, 0, 0);
                }
            }
        }
    }
}
