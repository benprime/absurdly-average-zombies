using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonActions : MonoBehaviour
{

    public void LoadLevel(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }

    public void ExitTitle()
    {
        Application.Quit();
    }

    public void ToggleAudio(Text text)
    {
        GameManager.instance.ToggleAudio();
        if(GameManager.instance.mute)
        {
            text.text = "SOUND ON";
        }
        else
        {
            text.text = "SOUND OFF";
        }
    }

    public void DeleteSaveFile()
    {
        GameManager.instance.progressManager.ClearData();
    }

    public void ShowCredits(GameObject creditReel)
    {
        GameObject clone = Instantiate(creditReel);
        clone.transform.SetParent(GameObject.Find("Canvas").transform, false);
    }

    public void ClosePopUp(GameObject window)
    {
        if (window) Destroy(window);
        else Destroy(gameObject);
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
