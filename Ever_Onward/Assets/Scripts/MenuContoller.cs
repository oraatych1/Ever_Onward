using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuContoller : MonoBehaviour
{
    GameObject optionsMenu;
    GameObject playButton;
    GameObject optionsButton;
    GameObject quitButton;
    public void Start()
    {
        optionsMenu = GameObject.Find("Settings");
        playButton = GameObject.Find("Play");
        optionsButton = GameObject.Find("Options");
        quitButton = GameObject.Find("Quit");
        optionsMenu.SetActive(!optionsMenu.activeSelf);
    }
    public void Play()
    {
        SceneManager.LoadScene("Forest");
    }

    public void Options()
    {
        optionsMenu.SetActive(!optionsMenu.activeSelf);
        playButton.SetActive(!playButton.activeSelf);
        optionsButton.SetActive(!optionsButton.activeSelf);
        quitButton.SetActive(!quitButton.activeSelf);
    }
    public void Inventory()
    {

    }

    public void Quit()
    {
        Application.Quit();
    }
}
