﻿using UnityEngine;
using System.Collections;

public class ButtonActions : MonoBehaviour
{

    public void LoadLevel(string levelName)
    {
        Application.LoadLevel(levelName);
    }

    public void ExitTitle()
    {
        Application.Quit();
    }

    public void DeleteSaveFile()
    {
        GameManager.instance.progressManager.ClearData();
    }

    public void ShowCredits(GameObject creditReel)
    {
        GameObject clone = Instantiate(creditReel);
        clone.transform.SetParent(GameObject.Find("Canvas").transform, false);
    }

    public void ClosePopUp(GameObject window)
    {
        if (window) Destroy(window);
        else Destroy(gameObject);
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
