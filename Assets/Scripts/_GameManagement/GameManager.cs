using UnityEngine;
using System.Collections;
using System.IO;

public class GameManager : MonoBehaviour
{

    public static GameManager instance = null;

    public ProgressManager progressManager;


    public int player_totalCurrency = 0;
    public GameObject selectedObjToBuild;

    // true if a level that require all "normal" game level code
    public bool menu;


    //Awake is always called before any Start functions
    void Awake()
    {
        //Check if instance already exists
        if (instance == null)
        {

            //if not, set instance to this
            instance = this;
            this.progressManager = new ProgressManager();
        }
        //If instance already exists and it's not this:
        else if (instance != this)
        {
            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);
        }
        //Sets this to not be destroyed when reloading scene
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
    }

    public int GetPlayerTotalCurrency()
    {
        return player_totalCurrency;
    }



}
