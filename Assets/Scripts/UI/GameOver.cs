﻿using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Linq;

public class GameOver : MonoBehaviour
{
    private Canvas canvas;

    // Use this for initialization
    void Start()
    {
        this.canvas = Canvas.FindObjectOfType<Canvas>();
        this.gameObject.transform.SetParent(canvas.gameObject.transform, false);
        this.gameObject.SetActive(true);

        BuildZone[] allZones = FindObjectsOfType<BuildZone>();
        if (allZones.Length > 0)
        {
            foreach (BuildZone zone in allZones)
            {
                zone.CloseOut();
            }
        }

        foreach (ZombieSpawner zs in FindObjectsOfType<ZombieSpawner>()) Destroy(zs.gameObject);
        foreach (Zombie z in FindObjectsOfType<Zombie>()) Destroy(z.gameObject);

        Screen.sleepTimeout = SleepTimeout.SystemSetting;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void BackToMenu()
    {
        Screen.sleepTimeout = SleepTimeout.SystemSetting;
        SceneManager.LoadScene("SelectLevel");
    }
}
