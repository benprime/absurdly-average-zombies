using UnityEngine;
using System.Collections;

public class PopupMessageButton : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

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
