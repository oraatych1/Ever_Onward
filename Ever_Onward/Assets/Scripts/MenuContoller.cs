using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuContoller : MonoBehaviour
{
    public void Play()
    {
        SceneManager.LoadScene("Forest");
    }

    public void Options()
    {
        SceneManager.LoadScene("Options");
    }

    public void Inventory()
    {

    }

    public void Quit()
    {
        Application.Quit();
    }
}
