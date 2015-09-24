// C#
// WaveGenerator.cs
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

[System.Serializable]
public class Wave
{
    [HideInInspector]
	public string name = "Wave";
	public float delayBeforeWave;
	public string beforeMessage;
	public string afterMessage;
}



public class WaveGenerator : MonoBehaviour
{
    public int startingMoney;
    public List<Wave> waves;
	private Wave m_CurrentWave;
	public Wave CurrentWave { get {return m_CurrentWave;} }
	private UI_WaveMessages messageBoard;
	private Text waveMessageText;
	private Text countDownText;
    private int currentWaveIndex = 0;

    IEnumerator SpawnLoop()
	{
		Wave lastWave = waves.Last ();
		foreach(Wave W in waves)
		{
			m_CurrentWave = W;

            //Display countdown to wave start
			if(W.delayBeforeWave > 0)
			{
				StartCoroutine(DrawBeforeText(W));
				yield return new WaitForSeconds(W.delayBeforeWave);
			}
            this.countDownText.enabled = false;

            //Spawners start
            ZombieSpawner[] spawns = FindObjectsOfType<ZombieSpawner>();
            foreach(ZombieSpawner zs in spawns)
            {
                zs.BeginSpawnWave(currentWaveIndex);
            }

            //allow 1 second for spawners to start
            yield return new WaitForSeconds(1);

            // wave is not over until all zombies are dead
            while (FindObjectsOfType<Zombie>().Length > 0)
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
            currentWaveIndex++;
            yield return null;  // prevents crash if all delays are 0
		}
        // a level is completed
        this.waveMessageText.enabled = true;
        this.waveMessageText.text = "Level Complete";
        GameManager.instance.progressManager.CompleteLevel(Application.loadedLevelName);
        yield return new WaitForSeconds(3);
        Application.LoadLevel("SelectLevel");
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
        this.countDownText = GameObject.Find("CountDown").GetComponent<Text>();
        this.waveMessageText = GameObject.Find("WaveMessageBox").GetComponent<Text>();
        GameManager.instance.player_totalCurrency = startingMoney;

        currentWaveIndex = 0;
        StartCoroutine(SpawnLoop());
	}

	void Update()
	{
	}
	
}