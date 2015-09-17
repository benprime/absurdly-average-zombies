// C#
// WaveGenerator.cs
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

[System.Serializable]
public class WaveAction
{
	public string name;
	public Transform prefab;
	public int spawnCount;
	public float secondsBetweenSpawn;
}

[System.Serializable]
public class Wave
{
	public string name;
	public List<WaveAction> actions;
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
    public int startingMoney;

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

			foreach(WaveAction A in W.actions)
			{
				if (A.prefab != null && A.spawnCount > 0)
				{
					for(int i = 0; i < A.spawnCount; i++)
					{
						Instantiate(A.prefab, this.transform.position, Quaternion.identity);
						yield return new WaitForSeconds(A.secondsBetweenSpawn);
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
        GameManager.instance.player_totalCurrency = startingMoney;

        StartCoroutine(SpawnLoop());
	}

	void Update()
	{
	}
	
}