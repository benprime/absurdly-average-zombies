using UnityEngine;
using System.Collections;
using System.IO;

public class GameManager : MonoBehaviour {

	public static GameManager instance = null;
	//private LevelManager 
	public int highestCompletedScene;
	public int HighestCompletedScene {
		get {
			return this.highestCompletedScene;
		}
		set {
			if(value > this.highestCompletedScene)
			{
				this.highestCompletedScene = value;
			}
		}
	}

	public int player_totalCurrency = 0;


	//Awake is always called before any Start functions
	void Awake()
	{
		//Check if instance already exists
		if (instance == null)
			
			//if not, set instance to this
			instance = this;
		
		//If instance already exists and it's not this:
		else if (instance != this)
			
			//Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
			Destroy(gameObject);    
		
		//Sets this to not be destroyed when reloading scene
		DontDestroyOnLoad(gameObject);
	}

	// Use this for initialization
	void Start () {
		this.LoadData ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void SaveData()
	{
		using(var writer = new BinaryWriter(File.OpenWrite("FileName")))
		{
			writer.Write(this.HighestCompletedScene);
			writer.Close();
		}
	}
	
	void LoadData()
	{
		try
		{
			using(var reader = new BinaryReader(File.OpenRead("FileName")))
			{
				this.HighestCompletedScene = reader.ReadInt32();
				reader.Close();
			}
		}
		catch
		{
			// if loading game data fails for any reason, just initialize it to blank
			this.HighestCompletedScene = 0;
		}
		
	}

	
	public void PlayerCurrencyTransaction(int amount) {
		player_totalCurrency += amount;
	}
	
	public int GetPlayerTotalCurrency() {
		return player_totalCurrency;
	}



}
