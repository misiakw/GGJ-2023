using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody rb;
    public float jumpStrength = 1;
    public bool canJump = false;
    private Vector3 _defaultPlayerPosition;

    private IController controller;

    void Start()
    {
        controller = ControllerDevice.Instance;
        var pos = gameObject.transform.position;
        _defaultPlayerPosition = new Vector3(pos.x, pos.y, pos.z);
    }

    // Update is called once per frame
    void Update()
    {
        if(GlobalStore.GameState == GameState.Running)
        {
            ManageRunningInput();
        }

        if (GlobalStore.GameState == GameState.Died) 
        {
            if(controller.IsJumpDown) 
            {
                GlobalStore.GameState = GameState.Running;
                gameObject.transform.eulerAngles = new Vector3(0,0,0);
                gameObject.transform.position = _defaultPlayerPosition;
            }    
        }
    }

  private void ManageRunningInput()
  {
    if (canJump && controller.IsJumpDown)
    {
      canJump = false;
      rb.AddForce(0, jumpStrength * 100, 0);
    }
    if (controller.IsCrouchDown)
    {
      gameObject.transform.localScale = new Vector3(1, 0.5f, 1);
    }
    if (controller.IsCrouchUp)
    {
      gameObject.transform.localScale = new Vector3(1, 1, 1);
    }
  }

  void OnCollisionEnter(Collision collided)
    {
        if(collided.gameObject.tag == "Floor") 
        {
            canJump = true;
        }

        if(collided.gameObject.tag == "Obstacle" && GlobalStore.GameState == GameState.Running) 
        {
            GlobalStore.GameState = GameState.Died;
            gameObject.transform.eulerAngles = new Vector3(0,0,-90);
        }
    }
}
