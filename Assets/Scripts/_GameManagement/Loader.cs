﻿using UnityEngine;

public class Loader : MonoBehaviour
{

    public GameObject gameManager;
    public bool menu;

    void Awake()
    {
        //Check if a GameManager has already been assigned to static variable GameManager.instance or if it's still null
        if (GameManager.Instance == null)
        {
            //Instantiate gameManager prefab
            Instantiate(gameManager);
        }

        // set whether the currently loaded level is game level or not
        GameManager.Instance.menu = this.menu;
    }
}
