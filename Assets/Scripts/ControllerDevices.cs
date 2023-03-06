using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public interface IController
{
    void Loop();
}

public abstract class BaseControler
{
    protected ControllerDevice _controller;

    public BaseControler(ControllerDevice controller)
    {
        _controller = controller;
    }
}
public abstract class BtnController : BaseControler, IController
{
    protected BtnController(ControllerDevice controller) : base(controller) { }
    protected bool isCrouching = false;
    protected DateTime JumpDownTime = DateTime.Now;

    protected abstract bool JumpDown();
    protected abstract bool CrouchDown();
    protected abstract bool CrouchUp();
    protected abstract bool DashDown();

    protected virtual void processJump()
    {
        if (JumpDown())
        {
            _controller.OnJumpStart?.Invoke(this, null);
        }
    }
    public virtual void Loop()
    {
        if (isCrouching && CrouchUp())
        {
            isCrouching = false;
            _controller.OnCrouchLeave?.Invoke(this, null);
        }
        else if (!isCrouching && CrouchDown())
        {
            isCrouching = true;
            _controller.OnCrouchEnter?.Invoke(this, null);
        }

        if (DashDown())
        {
            _controller.OnDashStart?.Invoke(this, null);
        }
        processJump();
    }
}
public class Keyboard : BtnController, IController
{
    private KeyCode _jump;
    private KeyCode _crouch;
    private KeyCode _dash;
    public Keyboard(KeyCode jump, KeyCode crouch, KeyCode dash, ControllerDevice controller) : base(controller)
    {
        _jump = jump;
        _crouch = crouch;
        _dash = dash;
    }

    protected override bool CrouchDown() => Input.GetKeyDown(_crouch);
    protected override bool CrouchUp() => Input.GetKeyUp(_crouch);

    protected override bool DashDown() => Input.GetKeyDown(_dash);

    protected override bool JumpDown() => Input.GetKeyDown(_jump);
}

public class GamepadBtnController : BtnController, IController
{
    public GamepadBtnController(ControllerDevice controller) : base(controller) { }

    //protected override bool CrouchDown() => Gamepad.current.aButton.isPressed;
    //protected override bool CrouchUp() => isCrouching && !Gamepad.current.aButton.isPressed;
    protected override bool CrouchDown() => Gamepad.current.leftStick.down.ReadValue() > 0.3;
    protected override bool CrouchUp() => Gamepad.current.leftStick.down.ReadValue() == 0;

    protected override bool JumpDown() => Gamepad.current.aButton.isPressed;

    private bool waitingForRelease = false;
    protected override void processJump()
    {
        if (waitingForRelease && !Gamepad.current.aButton.isPressed)
        {
            waitingForRelease = false;
        }
        if (JumpDown() && !waitingForRelease)
        {
            waitingForRelease = true;
            _controller.OnJumpStart?.Invoke(this, null);
        }
    }

    public override void Loop()
    {
        if (Gamepad.current != null)
        {
            base.Loop();
        }
    }

    protected override bool DashDown()
    {
        return false;
    }
}

public class XboxPadController : BtnController, IController
{
    public XboxPadController(ControllerDevice controller) : base(controller) { }

    protected override bool CrouchDown() => Gamepad.current.leftStick.down.ReadValue() > 0.3;
    protected override bool CrouchUp() => Gamepad.current.leftStick.down.ReadValue() == 0;
    protected override bool JumpDown() => Gamepad.current.leftStick.up.ReadValue() > 0.3;

    public override void Loop()
    {
        if (Gamepad.current != null)
        {
            base.Loop();
        }
    }

    protected override bool DashDown()
    {
        return false;
    }
}

public class ControllerDevice : IController
{
    private static ControllerDevice _instance = null;

    public static ControllerDevice Instance
    {
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
        //_devices.Add(new Keyboard(KeyCode.W, KeyCode.S, KeyCode.E, this));
        _devices.Add(new Keyboard(KeyCode.UpArrow, KeyCode.DownArrow, KeyCode.RightArrow, this));
        //_devices.Add(new Keyboard(KeyCode.Space, KeyCode.LeftControl, KeyCode.LeftShift, this));
        _devices.Add(new GamepadBtnController(this));
        //_devices.Add(new XboxPadController(this));

    }


    public EventHandler OnCrouchEnter;
    public EventHandler OnCrouchLeave;
    public EventHandler OnJumpStart;
    public EventHandler OnDashStart;
    public EventHandler OnMenuPress;

    public void Loop()
    {
        foreach (var d in _devices)
        {
            d.Loop();
        }
        if (Input.GetKey(KeyCode.Escape))
        {
            if (Input.GetKeyDown(KeyCode.M))
            {
                OnMenuPress?.Invoke(this, null);
            }
        }
        if (Input.GetKey(KeyCode.M))
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                OnMenuPress?.Invoke(this, null);
            }
        }
    }

    public void Init()
    {
        throw new NotImplementedException();
    }
}