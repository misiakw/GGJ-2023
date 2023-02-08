using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForegroundController : MonoBehaviour
{
    bool isRunning = GlobalStore.State.Value == GameState.Running;
    // Start is called before the first frame update
    void Start()
    {
        GlobalStore.State.Onchange += (s, v) => isRunning = v == GameState.Running;
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
                    child.Translate(GlobalStore.ObstacleVelocity * Time.deltaTime * 1.5f);
                    if (child.localPosition.x <= -120)
                    {
                        child.localPosition += new Vector3(240, 0, 0);
                    }
                }
                else
                {
                    child.Translate(GlobalStore.ObstacleVelocity * Time.deltaTime * 1.1f);
                    if (child.localPosition.x <= -32)
                    {
                        child.localPosition += new Vector3(57, 0, 0);
                    }
                }
            }
        }
    }
}
