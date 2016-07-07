using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonActions : MonoBehaviour
{
    public GameObject LoadingPanel;

    public void LoadLevel(string levelName)
    {
        LoadingPanel.SetActive(true);
        SceneManager.LoadScene(levelName);
    }

    public void ExitTitle()
    {
        Application.Quit();
    }

    public void ToggleAudio(Text text)
    {
        GameManager.instance.ToggleAudio();
        if (GameManager.instance.mute)
        {
            text.text = "SOUND: OFF";
        }
        else
        {
            text.text = "SOUND: ON";
        }
    }

    public void DeleteSaveFile(GameObject window)
    {
        GameManager.instance.progressManager.ClearData();
        ClosePopUp(window);
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
