using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SetMuteButtonText : MonoBehaviour
{

    public string MutedText;
    public string UnmutedText;

    // Use this for initialization
    void Start()
    {
        Transform textParentObj = gameObject.transform.Find("Text");
        GameObject textObj = textParentObj != null ? textParentObj.gameObject : null;
        Text text = textObj != null ? textObj.GetComponent<Text>() : null;
        if (text != null)
        {
            if (GameManager.Instance.mute)
            {
                text.text = MutedText;
            }
            else
            {
                text.text = UnmutedText;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
