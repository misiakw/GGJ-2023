using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody rb;
    public float jumpStrength = 1;
    public bool canJump = false;

    // Update is called once per frame
    void Update()
    {
        if(canJump && Input.GetKeyDown("w"))
        {
            canJump = false;
            rb.AddForce(0, jumpStrength*100, 0);
        }
    }

    void OnCollisionEnter(Collision collided)
    {
        if(collided.gameObject.tag == "Floor") 
        {
            canJump = true;
        }
    }
}
