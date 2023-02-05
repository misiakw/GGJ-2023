using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForegroundController : MonoBehaviour
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
                if (child.name.StartsWith("Trees"))
                {
                    child.Translate(GlobalStore.ObstacleVelocity * Time.deltaTime * 0.6f);
                    if (child.localPosition.x <= -120)
                    {
                        child.localPosition += new Vector3(240, 0, 0);
                    }
                }
                else
                {
                    child.Translate(GlobalStore.ObstacleVelocity * Time.deltaTime * 0.5f);
                    if (child.localPosition.x <= -32)
                    {
                        child.localPosition += new Vector3(57, 0, 0);
                    }
                }
            }
        }
    }
}
