using UnityEngine;
using UnityEngine.UI;
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

    /// <summary>
    /// Used to make the text move up and down when the button is clicked.
    /// </summary>
    /// <param name="offset"></param>
    private void SetTextOffset(int top, int bottom)
    {
        // little hacky way to make the text align with the "down" state when a user clicks a button
        Transform t = this.gameObject.transform.Find("Text");
        if (t != null)
        {
            RectTransform rectTransform = t.gameObject.GetComponent<RectTransform>();
            rectTransform.offsetMin = new Vector2(rectTransform.offsetMin.x, bottom);
            rectTransform.offsetMax = new Vector2(rectTransform.offsetMax.x, top);
        }
    }

    /// <summary>
    /// If the popup is shown multiple times, need to reset it to "up" state.
    /// </summary>
    void OnEnable()
    {
        this.SetTextOffset(-3, 0);
    }

    public void OnMouseDown()
    {
        this.SetTextOffset(-7, 0);
    }

    public void OnMouseUp()
    {
        this.SetTextOffset(-3, 0);
    }

    public void ButtonClick()
    {
        // whenever we dismiss a dialog, set the screen timeout to never turn off
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        this.gameObject.transform.parent.gameObject.SetActive(false);

        this.SetTextOffset(-7, 0);
    }
}
