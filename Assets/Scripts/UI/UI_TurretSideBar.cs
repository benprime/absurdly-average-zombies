using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_TurretSideBar : MonoBehaviour
{

	private GameManager gm;

	public GameObject confirmExitPop;

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
		if (!sb_isOpen) {
			Vector3 newPos = new Vector3 (transform.localPosition.x - 100, transform.localPosition.y, transform.localPosition.z);
			transform.localPosition = newPos;
		} else {
			Vector3 twoPos = new Vector3 (transform.localPosition.x + 100, transform.localPosition.y, transform.localPosition.z);
			transform.localPosition = twoPos;
		}
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
		if (confirmExitPop && GameObject.FindGameObjectsWithTag("ConfirmScreen").Length == 0) {	//create a confirmation pop up
			GameObject clone = Instantiate (confirmExitPop);
			clone.transform.SetParent (GameObject.Find ("Canvas").transform, false);
		}
    }

}
