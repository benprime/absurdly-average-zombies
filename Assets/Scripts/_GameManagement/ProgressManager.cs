using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;


[Serializable]
public class LevelProgressData {
	public string LevelName;
	public bool Completed;
	public bool Locked;
}

// This is just a utility class
// Not sure where we should put classes like this
public class ProgressManager {
	
	private string saveFile;

	public List<LevelProgressData> LevelProgress {
		get;
		set;
	}

	public ProgressManager()
	{
		this.saveFile = Path.Combine(Application.persistentDataPath, "SaveData.bin");

		this.LevelProgress = new List<LevelProgressData> ();
		this.LevelProgress.Add(new LevelProgressData() { LevelName = "Level1", Locked = false, Completed = false });
		this.LevelProgress.Add(new LevelProgressData() { LevelName = "Level2", Locked = true, Completed = false });
		this.LevelProgress.Add(new LevelProgressData() { LevelName = "Level3", Locked = true, Completed = false });
		this.LevelProgress.Add(new LevelProgressData() { LevelName = "Level4", Locked = true, Completed = false });
		this.LevelProgress.Add(new LevelProgressData() { LevelName = "Level5", Locked = true, Completed = false });

		this.LoadData ();
	}

	public bool IsLevelLocked(string levelName)
	{
		LevelProgressData lpd = this.LevelProgress.FirstOrDefault(x => x.LevelName == levelName);
		if (lpd == null) {
			throw new Exception("The level name \"" + levelName + "\" was not found in the LevelProgress list.");
		}
		return lpd.Locked;
		//return this.LevelProgress.First (x => x.LevelName == levelName).Locked;
	}

	public void CompleteLevel(string levelName)
	{
		//TODO: used a linked list for levels instead?
		int index = this.LevelProgress.FindIndex (x => x.LevelName == levelName);
		this.LevelProgress [index].Completed = true;
		if (index + 1 <= this.LevelProgress.Count - 1) {
			this.LevelProgress[index+1].Locked = false;
		}
		this.SaveData ();
	}

	public void SaveData()
	{
		using (Stream stream = File.Open(this.saveFile, FileMode.Create))
		{
			var bformatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
			
			bformatter.Serialize(stream, this.LevelProgress);
		}
	}
	
	public void LoadData()
	{
		try
		{
			//deserialize
			using (Stream stream = File.Open(this.saveFile, FileMode.Open))
			{
				var bformatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
				
				this.LevelProgress = (List<LevelProgressData>)bformatter.Deserialize(stream);
			}
		}
		catch
		{
			// if loading game data fails for any reason, just initialize it to blank
			Debug.Log("Loading save file failed, defaulting to intial state");
		}

		
	}
}
