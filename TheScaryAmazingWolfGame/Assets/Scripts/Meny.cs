using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Meny : MonoBehaviour
{
    public void OnPlayButton ()
    {
        SceneManager.LoadScene(1);
    }

    public void OnWinButton ()
    {
        SceneManager.LoadScene(0);
    }

    public void OnloseButton ()
    {
        SceneManager.LoadScene(0);
    }

    public void OnStoryButton ()
    {
        SceneManager.LoadScene(2);
    }

    public void OnQuitButton ()
    {
        Application.Quit();
    }
}
