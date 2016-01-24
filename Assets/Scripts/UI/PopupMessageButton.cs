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
        this.gameObject.transform.parent.gameObject.SetActive(false);
    }
}
