using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonActions : MonoBehaviour
{
    public GameObject LoadingPanel;

    public void LoadLevel(string levelName)
    {
        if (LoadingPanel)
        {
            LoadingPanel.SetActive(true);
        }
        SceneManager.LoadScene(levelName);
    }

    public void ExitTitle()
	{
		Screen.sleepTimeout = SleepTimeout.SystemSetting;
        Application.Quit();
    }

    public void ToggleAudio(Text text)
    {
        GameManager.Instance.ToggleAudio();
        if (GameManager.Instance.mute)
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
        GameManager.Instance.progressManager.ClearData();
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

	public void BackOut(GameObject window) {
		int currentScene = SceneManager.GetActiveScene ().buildIndex;
		if (currentScene > 3) {	//In a level = go back to select level
			Screen.sleepTimeout = SleepTimeout.SystemSetting;
			SceneManager.LoadScene("SelectLevel");
		} else if (currentScene == 1) {	//At main menu = exit game
			ExitTitle();
		}
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
