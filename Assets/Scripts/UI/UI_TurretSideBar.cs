using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_TurretSideBar : MonoBehaviour
{

    private GameManager gm;

    // Use this for initialization
    void Start()
    {
        gm = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SelectObjectToPlace(GameObject choice)
    {
        gm.selectedObjToBuild = choice;
    }

    private bool sb_isOpen = false;
    public void ToggleSideBar()
    {
        // Fix for: side bar doesn't quite pop all the way out in 16:9 aspect ratio
        float moveDistance = 2.0f;
        if (Camera.main.aspect == ControllingCameraAspectScript.SIXTEEN_NINE)
        {
            moveDistance = 2.5f;
        }
        if (!sb_isOpen) transform.Translate(-moveDistance, 0, 0); //TODO: 126 pixels or whatever size of sidebar is
        else transform.Translate(moveDistance, 0, 0);
        sb_isOpen = !sb_isOpen;
    }

    public void OpenOptionsMenu()
    {
        //Bring up options
    }

    public void ReturnToMenuScreen()
    {
        Screen.sleepTimeout = SleepTimeout.SystemSetting;
        GameManager.Instance.GetComponent<AudioSource>().clip = GameManager.Instance.menuMusic;
        GameManager.Instance.GetComponent<AudioSource>().Play();
        SceneManager.LoadScene("MainMenu");
    }

    public void ExitGame()
    {
        Screen.sleepTimeout = SleepTimeout.SystemSetting;
        Application.Quit();
    }

}
