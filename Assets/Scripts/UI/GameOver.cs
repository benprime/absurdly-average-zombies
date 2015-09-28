using UnityEngine;
using System.Collections;
using System.Linq;

public class GameOver : MonoBehaviour
{
    //public GameObject popupMessagePrefab;
    private Canvas canvas;
    //private GameObject popupMessageInstance;

    // Use this for initialization
    void Start()
    {
        this.canvas = Canvas.FindObjectOfType<Canvas>();
        this.gameObject.transform.SetParent(canvas.gameObject.transform, false);
        this.gameObject.SetActive(true);

        //this.popupMessageInstance = Instantiate(this.popupMessagePrefab);
        //this.popupMessageInstance.transform.SetParent(canvas.transform);
        Screen.sleepTimeout = SleepTimeout.SystemSetting;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void Retry()
    {
        Application.LoadLevel(Application.loadedLevelName);
    }

    public void BackToMenu()
    {
        Application.LoadLevel("MainMenu");
    }
}
