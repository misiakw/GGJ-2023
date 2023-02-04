using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForegroundController : MonoBehaviour
{
    public GameObject TreesPrefab;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (GlobalStore.GameState == GameState.Running)
        {
            foreach (Transform child in transform.GetComponentInChildren<Transform>())
            {
                float speed = child.name.StartsWith("Trees") ? 0.04f
                    : 0.001f;
                child.Translate(Vector3.left * speed);
                if (child.localPosition.x <= -120)
                {
                    child.localPosition += new Vector3(240, 0, 0);
                }
            }
        }
    }
}
