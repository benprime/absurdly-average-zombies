using UnityEngine;
using System;
using UnityEngine.Advertisements;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance = null;

    public float FireDamage = 0.0f;
    public float BulletDamage = 0.0f;
    public float RocketDamage = 0.0f;

    public float FireMoney = 0.0f;
    public float BulletMoney = 0.0f;
    public float RocketMoney = 0.0f;

    public ProgressManager progressManager;

    public int player_totalCurrency = 0;
    public GameObject selectedObjToBuild;

    // true if a level that require all "normal" game level code
    public bool menu;
    public bool mute = false;
    public AudioClip menuMusic;

    public int previousLevel = 0;
    public int bonusAmount = 0;

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
        GameManager.Instance.FireDamage = 0.0f;
        GameManager.Instance.BulletDamage = 0.0f;
        GameManager.Instance.RocketDamage = 0.0f;

        GameManager.Instance.FireMoney = 0.0f;
        GameManager.Instance.BulletMoney = 0.0f;
        GameManager.Instance.RocketMoney = 0.0f;

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
