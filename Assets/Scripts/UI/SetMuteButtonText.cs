using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SetMuteButtonText : MonoBehaviour {

    public string MutedText;
    public string UnmutedText;

	// Use this for initialization
	void Start () {
        GameObject textObj = gameObject.transform.FindChild("Text").gameObject;
        Text text = textObj.GetComponent<Text>();
	    if(GameManager.instance.mute)
        {
            text.text = MutedText;    
        }
        else
        {
            text.text = UnmutedText;
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
