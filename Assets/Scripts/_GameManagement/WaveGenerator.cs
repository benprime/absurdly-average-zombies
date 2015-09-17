// C#
// WaveGenerator.cs
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

[System.Serializable]
public class ZombieConfig
{
    public string name = "ZombieConfig";
    public GameObject zombiePrefab;
    public float secondsBetweenSpawn;
    public int count;
}

[System.Serializable]
public class Wave
{
	public string name = "Wave";
	public List<ZombieConfig> zombies;
	public float delayBeforeWave;
	public string beforeMessage;
	public string afterMessage;
}



public class WaveGenerator : MonoBehaviour
{
	public List<Wave> waves;
	private Wave m_CurrentWave;
	public Wave CurrentWave { get {return m_CurrentWave;} }
	private UI_WaveMessages messageBoard;
	private Text waveMessageText;
	private Text countDownText;

	IEnumerator SpawnLoop()
	{
		Wave lastWave = waves.Last ();
		foreach(Wave W in waves)
		{
			m_CurrentWave = W;


			if(W.delayBeforeWave > 0)
			{
				StartCoroutine(DrawBeforeText(W));
				yield return new WaitForSeconds(W.delayBeforeWave);
			}
			else
			{
				this.countDownText.enabled = false;
			}

			foreach(ZombieConfig zc in W.zombies)
			{
				if (zc.zombiePrefab != null && zc.count > 0)
				{
					for(int i = 0; i < zc.count; i++)
					{
						Instantiate(zc.zombiePrefab, this.transform.position, Quaternion.identity);
						yield return new WaitForSeconds(zc.secondsBetweenSpawn);
					}
				}
			}

			// wave it not over until all zombies are dead
			while(FindObjectsOfType<Zombie>().Length > 0)
			{
				yield return null;
			}

			// If this is the last wave, then the "Level Complete" message will show
			// so we don't want the wave complete message showing at the same time
			if(lastWave != W)
			{
				this.waveMessageText.text = W.afterMessage;
			}
			// A wave has complete
			yield return null;  // prevents crash if all delays are 0
		}
		// a level is completed
		this.waveMessageText.enabled = true;
		this.waveMessageText.text = "Level Complete";
		GameManager.instance.progressManager.CompleteLevel (Application.loadedLevelName);
		yield return new WaitForSeconds(3);
		Application.LoadLevel ("SelectLevel");
		yield return null;  // prevents crash if all delays are 0
	}

	IEnumerator DrawBeforeText(Wave w)
	{
		int delay = (int)w.delayBeforeWave;
		if (w.beforeMessage != "")
		{
			this.waveMessageText.enabled = true;
			this.waveMessageText.text = w.beforeMessage;
		}

		this.countDownText.enabled = true;
		for (int i = delay - 1; i > -1; i--) {
			this.countDownText.text = i.ToString();
			yield return new WaitForSeconds(1);
		}
		this.countDownText.enabled = false;
		this.waveMessageText.enabled = false;
	}


	void Start()
	{
		this.countDownText = GameObject.Find ("CountDown").GetComponent<Text>();
		this.waveMessageText = GameObject.Find ("WaveMessageBox").GetComponent<Text>();

		StartCoroutine(SpawnLoop());
	}

	void Update()
	{
	}
	
}