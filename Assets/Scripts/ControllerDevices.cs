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
    private bool isCrouching = false;

    protected abstract bool JumpDown();
    protected abstract bool CrouchDown();
    protected  abstract bool CrouchUp();
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

        if (JumpDown())
        {
            _controller.OnJumpStart?.Invoke(this, null);
        }
    }
}
public class Keyboard: BtnController, IController
{
    private KeyCode _jump;
    private KeyCode _crouch;
    public Keyboard(KeyCode jump, KeyCode crouch, ControllerDevice controller): base(controller)
    {
        _jump = jump;
        _crouch = crouch;
    }

    protected override bool CrouchDown() => Input.GetKeyDown(_crouch);
    protected override bool CrouchUp() => Input.GetKeyUp(_crouch);
    protected override bool JumpDown() => Input.GetKeyDown(_jump);
}

public class GamepadBtnController : BtnController, IController
{
    public GamepadBtnController(ControllerDevice controller) : base(controller) { }

    protected override bool CrouchDown() => Gamepad.current.bButton.wasPressedThisFrame;
    protected override bool CrouchUp() => Gamepad.current.bButton.wasReleasedThisFrame;
    protected override bool JumpDown() => Gamepad.current.aButton.isPressed;

    public override void Loop()
    {
        if (Gamepad.current != null)
        {
            base.Loop();
        }
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
}

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
        _devices.Add(new Keyboard(KeyCode.W, KeyCode.S, this));
        _devices.Add(new Keyboard(KeyCode.UpArrow, KeyCode.DownArrow, this));
        _devices.Add(new Keyboard(KeyCode.Space, KeyCode.LeftControl, this));
        _devices.Add(new GamepadBtnController(this));
        _devices.Add(new XboxPadController(this));
    }


    public EventHandler OnCrouchEnter;
    public EventHandler OnCrouchLeave;
    public EventHandler OnJumpStart;
    public void Loop()
    {
        foreach(var d in _devices)
        {
            d.Loop();
        }
    }
}