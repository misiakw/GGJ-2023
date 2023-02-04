using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public interface IController
{
    public bool IsJumpDown { get; }
    public bool IsCrouchDown { get; }
    public bool IsCrouchUp { get; }
}


public class WsadController : IController
{
    public bool IsJumpDown => Input.GetKeyDown("w");

    public bool IsCrouchDown => Input.GetKeyDown("s");

    public bool IsCrouchUp => Input.GetKeyUp("s");
}
public class ArrowController : IController
{
    public bool IsJumpDown => Input.GetKeyDown(KeyCode.UpArrow);

    public bool IsCrouchDown => Input.GetKeyDown(KeyCode.DownArrow);

    public bool IsCrouchUp => Input.GetKeyUp(KeyCode.DownArrow);
}

/*public class XboxPadController : IController
{
    public bool IsJumpDown => Gamepad.current.
    public bool IsCrouchDown => throw new System.NotImplementedException();

    public bool IsCrouchUp => throw new System.NotImplementedException();
}*/

public class ControllerDevice : IController
{
    private static ControllerDevice _instance = null;

    public static ControllerDevice Instance {
        get
        {
            if (_instance == null)
                _instance = new ControllerDevice();
            return _instance;
        }
    }


    private IList<IController> _devices = new List<IController>();
    private ControllerDevice()
    {
        _devices.Add(new WsadController());
        _devices.Add(new ArrowController());
    }

    public bool IsJumpDown => _devices.Any(d => d.IsJumpDown);

    public bool IsCrouchDown => _devices.Any(d => d.IsCrouchDown);
    public bool IsCrouchUp => _devices.Any(d => d.IsCrouchUp);
}