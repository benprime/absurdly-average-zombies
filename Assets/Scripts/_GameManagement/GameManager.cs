using UnityEngine;
using System;
using UnityEngine.Advertisements;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance = null;

    public ProgressManager progressManager;

    public int player_totalCurrency = 0;
    public GameObject selectedObjToBuild;

    // true if a level that require all "normal" game level code
    public bool menu;
    public bool mute = false;
    public AudioClip menuMusic;

    public int previousLevel = 0;
    public int bonusAmount = 0;

	public GameObject confirmExitPop;

    // Awake is always called before any Start functions
    void Awake()
    {
        // Check if instance already exists
        if (Instance == null)
        {
            // if not, set instance to this
            Instance = this;
            this.progressManager = new ProgressManager();
        }
        // If instance already exists and it's not this:
        else if (Instance != this)
        {
            // Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);
        }
        // Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);
    }

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
		if (Input.GetKeyUp (KeyCode.Escape)) {
			int currentScene = SceneManager.GetActiveScene ().buildIndex;
			if (currentScene > 3 || currentScene == 1) {	//At Main Menu or in a level
				if (confirmExitPop && GameObject.FindGameObjectsWithTag ("ConfirmScreen").Length == 0) {	//create a confirmation pop up
					GameObject clone = Instantiate (confirmExitPop);
					clone.transform.SetParent (GameObject.Find ("Canvas").transform, false);
				} 
				//else {
				//	foreach(GameObject x in GameObject.FindGameObjectsWithTag ("ConfirmScreen")) {
				//		Debug.Log (x);
				//		x.GetComponent<ButtonActions> ().ClosePopUp (x);
				//	}
				//}
			} else if (currentScene == 2 || currentScene == 3) {	//levels 2-3 are sub menus
				SceneManager.LoadScene("MainMenu");	//go back to main menu
			}
		}
    }

    public void PlayerCurrencyTransaction(int amount)
    {
        player_totalCurrency += amount;

        int currLevel = SceneManager.GetActiveScene().buildIndex;
        // limit player money based on game levels
        if (player_totalCurrency > 50 && currLevel <= 9 /*level 5*/)
            player_totalCurrency = 50;
        else if (player_totalCurrency > 75 && currLevel <= 14 /*level 10*/)
            player_totalCurrency = 75;
        else if (player_totalCurrency > 100 && currLevel <= 19 /*level 15*/)
            player_totalCurrency = 100;
        else if (player_totalCurrency > 125)
            player_totalCurrency = 125;
    }

    public int GetPlayerTotalCurrency()
    {
        return player_totalCurrency;
    }

    public void ToggleAudio()
    {
        mute = !mute;
        AudioListener.volume = Convert.ToInt32(!mute);
    }

    void OnLevelWasLoaded(int level)
    {
        AudioListener.volume = Convert.ToInt32(!mute);

        // only display ad if player goes from the game back to menu or retries level
        if (previousLevel > 3)
        {
            if (Advertisement.IsReady())
            {
                Advertisement.Show();
            }
        }
        previousLevel = level;
    }


}
