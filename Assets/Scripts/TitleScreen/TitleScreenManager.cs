using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreenManager : MonoBehaviour
{
    [SerializeField] private GameObject aboutPanel;

    [SerializeField] private GameObject continueButton;


    void Start()
    {
        continueButton.SetActive(System.IO.File.Exists(FileManager.savPath + "/save.sav"));
    }


    public void OpenAboutPanel(bool value)
    {
        aboutPanel.SetActive(value);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void LaunchGame(bool debug)
    {
        if (!debug)
        {
            GameManager.ChangeLevel("Introduction");
        }
        else
        {
            GameManager.instance.LoadGame();
        }
    }


    public void ChangeLocalization(string newLoc)
    {
        GameManager.ChangeLocalization(newLoc);
    }
}
