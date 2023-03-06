using System;
using UnityEngine;
using Screen = UnityEngine.Device.Screen;

public class MenuScript : MonoBehaviour
{
    private ControllerDevice controllerDevice;
    public GameObject Layout;
    void Start()
    {
        controllerDevice = ControllerDevice.Instance;
        controllerDevice.OnMenuPress += onMenuPress;
    }

    void onMenuPress(object sender, EventArgs args)
    {
        if(Layout!= null) {
            if (Layout.activeSelf)
                HideMenu();
            else
                Layout.SetActive(true);
        }
    }

    public void OnExit()
    {
        Application.Quit();
    }
    public void OnScreenChange()
    {
        if (Screen.fullScreenMode != FullScreenMode.FullScreenWindow)
        {
            Screen.SetResolution(3840, 2160, FullScreenMode.FullScreenWindow, 60);
        }
        else
        {
            Screen.SetResolution(640, 360, FullScreenMode.Windowed, 60);
        }
    }

    public void HideMenu()
    {
        if (Layout != null)
        {
            Layout.SetActive(false);
        }
    }

    void Update()
    {   
    }

    public void OnDestroy()
    {
        controllerDevice.OnMenuPress -= onMenuPress;
    }
}
