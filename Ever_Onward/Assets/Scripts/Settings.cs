using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
public class Settings : MonoBehaviour
{
    public Dropdown resDrop;
    Resolution[] resolutions;
    // Start is called before the first frame update
    public AudioMixer aMixer;
   
    public void Start()
    {
        int currentResolutionIndex = 4;
        resolutions = Screen.resolutions;
        resDrop.ClearOptions();
        List<string> resOptions = new List<string>();
        
        for (int i = 4; i < resolutions.Length; i++)
        {
            if (i % 3 == 0)
            {
                string resOption = resolutions[i].width + " x " + resolutions[i].height;
                resOptions.Add(resOption);

                if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
                {
                    currentResolutionIndex = i;
                }
            }
        }
        resDrop.AddOptions(resOptions);
        resDrop.value = currentResolutionIndex;
        resDrop.RefreshShownValue();
    }
    public void Resolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
    public void Volume(float volume)
    {
        aMixer.SetFloat("volume", volume);
        print(volume);
    }
    public void Quality(int qualityInt)
    {
        qualityInt = qualityInt == 0 ? 0 : qualityInt == 1 ? 3 : qualityInt == 2 ? 5: 3;
        QualitySettings.SetQualityLevel(qualityInt);
    }
    public void FullScreen(bool isFull)
    {
        Screen.fullScreen = isFull;
    }
}
