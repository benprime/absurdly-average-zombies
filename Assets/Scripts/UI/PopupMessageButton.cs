using UnityEngine;
using System.Collections;

public class PopupMessageButton : MonoBehaviour
{

    // Use this for initialization
    void Awake()
    {
        transform.SetAsLastSibling();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ButtonClick()
    {
        // whenever we dismiss a dialog, set the screen timeout to never turn off
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        this.gameObject.transform.parent.gameObject.SetActive(false);
    }
}
