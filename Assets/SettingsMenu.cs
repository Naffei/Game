using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    Resolution[] resolutions;
    public Dropdown resolutionDrop;

    private void Start()
    {
        resolutions = Screen.resolutions;
        resolutionDrop.ClearOptions();

        List<string> ResOptions = new List<string>();

        foreach (Resolution res in resolutions)
        {
            ResOptions.Add(res.width + " x " + res.height);
        }

        Resolution currentResolution = Screen.currentResolution;
        string currentResString = currentResolution.width + " x " + currentResolution.height;
        int currentIndex = ResOptions.IndexOf(currentResString);


        resolutionDrop.AddOptions(ResOptions);
        resolutionDrop.value = currentIndex;
    }

    public void SetResolution(int resIndex)
    {
        Resolution resolution = resolutions[resIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }

    public void back()
    {

    }


}
