using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

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

	public void ToggleAudio()
	{
		GameManager.instance.ToggleAudio ();
	}

	public void DeleteSaveFile(GameObject window)
    {
		GameManager.instance.progressManager.ClearData();
		if (window) Destroy(window);
		else Destroy(gameObject);
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
