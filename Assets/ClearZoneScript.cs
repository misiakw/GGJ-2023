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

    private void OnCollisionEnter(Collision collision)
    {
        if (OnlyPlayerCollider && collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerController>().RestartGame();
            Destroy(collision.gameObject);
        }
        else if(!OnlyPlayerCollider)
        {
            var mapTileController = collision.gameObject.GetComponent<MapTileController>();
            if (mapTileController != null)
            {
                mapTileController.GenerateNewTile();
                Destroy(collision.gameObject);
            }

        }
    }
}
