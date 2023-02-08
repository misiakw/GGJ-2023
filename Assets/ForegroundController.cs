using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForegroundController : MonoBehaviour
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
                if (child.name.StartsWith("Trees"))
                {
                    child.Translate(velocity * Time.deltaTime * 1.5f);
                    if (child.localPosition.x <= -120)
                    {
                        child.localPosition += new Vector3(240, 0, 0);
                    }
                }
                else
                {
                    child.Translate(velocity * Time.deltaTime * 1.1f);
                    if (child.localPosition.x <= -32)
                    {
                        child.localPosition += new Vector3(57, 0, 0);
                    }
                }
            }
        }
    }
}
