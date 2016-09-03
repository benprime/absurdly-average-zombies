﻿using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_TurretSideBar : MonoBehaviour
{

    private GameManager gm;

    // Use this for initialization
    void Start()
    {
        gm = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SelectObjectToPlace(GameObject choice)
    {
        gm.selectedObjToBuild = choice;
    }

    private bool sb_isOpen = false;
    public void ToggleSideBar()
    {
        if (!sb_isOpen) transform.Translate(-2, 0, 0); //TODO: 126 pixels or whatever size of sidebar is
        else transform.Translate(2, 0, 0);
        sb_isOpen = !sb_isOpen;
    }

    public void OpenOptionsMenu()
    {
        //Bring up options
    }

    public void ReturnToMenuScreen()
    {
        Screen.sleepTimeout = SleepTimeout.SystemSetting;
        GameManager.Instance.GetComponent<AudioSource>().clip = GameManager.Instance.menuMusic;
        GameManager.Instance.GetComponent<AudioSource>().Play();
        SceneManager.LoadScene("MainMenu");
    }

    public void ExitGame()
    {
        Screen.sleepTimeout = SleepTimeout.SystemSetting;
        Application.Quit();
    }

}
