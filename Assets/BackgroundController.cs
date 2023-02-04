using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    public GameObject TreesPrefab;
    public GameObject BacktreesPrefab;
    public GameObject BackgroundFXPrefab;

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
                float speed = child.name.StartsWith("Trees") ? 0.01f
                    : child.name.StartsWith("Backtrees") ? 0.002f
                    : 0.001f;
                child.Translate(Vector3.left * speed);
                if (child.localPosition.x <= -19.2)
                {
                    child.localPosition += new Vector3(57.6f, 0, 0);
                }
            }
        }
    }
}
