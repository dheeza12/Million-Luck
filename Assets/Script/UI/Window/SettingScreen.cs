using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class SettingScreen : MonoBehaviour
{
    public TMP_Dropdown resolutionDropdown;
    Resolution[] resolutions;

    public GameObject PauseSetting;

    private void Start()
    {
        resolutions = Screen.resolutions;

        resolutionDropdown.ClearOptions();
        int currentResolutionIndex = 0;
        List<string> options = new List<string>();
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);
            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ShowSettingScreen();
        }
    }

    public void ShowSettingScreen()
    {
        if (PauseSetting.activeSelf)
        {
            PauseSetting.LeanMoveLocalY(Screen.height, 0.22f);
            PauseSetting.LeanScale(Vector3.zero, 0.22f).setOnComplete(() => DisableScreen(PauseSetting));
        }
        else
        {
            PauseSetting.SetActive(true);
            PauseSetting.transform.localPosition = new Vector2(0f, Screen.height);
            PauseSetting.transform.localScale = Vector3.zero;
            PauseSetting.LeanMoveLocalY(0, 0.22f);
            PauseSetting.LeanScale(Vector3.one, 0.22f);
        }

    }

    private void DisableScreen(GameObject obj)
    {
        obj.SetActive(false);
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }
}
