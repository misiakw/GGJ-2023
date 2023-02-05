using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GlobalStore.ShouldScrollScreen())
        {
            foreach (Transform child in transform.GetComponentInChildren<Transform>())
            {
                float speed = child.name.StartsWith("Trees") ? 0.3f
                    : child.name.StartsWith("Backtrees") ? 0.2f
                    : 0.1f;
                child.Translate(GlobalStore.ObstacleVelocity * Time.deltaTime * speed);
                if (child.localPosition.x <= -19.2)
                {
                    child.localPosition += new Vector3(57.6f, 0, 0);
                }
            }
        }
    }
}
