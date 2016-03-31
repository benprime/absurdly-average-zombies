using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PopupMessageButton : MonoBehaviour
{
    Vector2 textPos;

    // Use this for initialization
    void Awake()
    {
        transform.SetAsLastSibling();
    }

    // Update is called once per frame
    void Update()
    {
    }
    
    public void OnMouseDown()
    {
        // little hacky way to make the text align with the "down" state when a user clicks a button
        Transform t = this.gameObject.transform.Find("Text");
        if (t != null)
        {
            RectTransform rectTransform = t.gameObject.GetComponent<RectTransform>();
            rectTransform.offsetMax = new Vector2(rectTransform.offsetMax.x, 5);
        }
    }

    public void OnMouseUp()
    {
        Transform t = this.gameObject.transform.Find("Text");
        if (t != null)
        {
            RectTransform rectTransform = t.gameObject.GetComponent<RectTransform>();
            rectTransform.offsetMax = new Vector2(rectTransform.offsetMax.x, 8);
        }
    }

    public void ButtonClick()
    {
        // whenever we dismiss a dialog, set the screen timeout to never turn off
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        this.gameObject.transform.parent.gameObject.SetActive(false);

        Transform t = this.gameObject.transform.Find("Text");
        if (t != null)
        {
            t.gameObject.GetComponent<RectTransform>().offsetMin = new Vector2(0, -8);
        }
    }
}
