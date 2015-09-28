using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LockLevelButtons : MonoBehaviour
{
    public string goToLevelName;
    public Image lockIcon;

    // Use this for initialization
    void Start()
    {
        if (GameManager.instance.progressManager.IsLevelLocked(goToLevelName))
        {
            GetComponent<Button>().enabled = false;
            gameObject.GetComponentInChildren<Text>().enabled = false;
            lockIcon.enabled = true;
        }
        else
        {
            GetComponent<Button>().enabled = true;
            gameObject.GetComponentInChildren<Text>().enabled = true;
            lockIcon.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
