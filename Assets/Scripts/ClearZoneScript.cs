using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearZoneScript : MonoBehaviour
{
    // Start is called before the first frame update
    public bool OnlyPlayerCollider = false;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (OnlyPlayerCollider && collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerController>().RestartGame();
            Destroy(collision.gameObject);
        }
    }
}
