// C#
// WaveGenerator.cs
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

[System.Serializable]
public class WaveAction
{
	public string name;
	public float delayBeforeWave;
	public Transform prefab;
	public int spawnCount;
	public string beforeMessage;
	public string afterMessage;
	public float secondsBetweenSpawn;
}

[System.Serializable]
public class Wave
{
	public string name;
	public List<WaveAction> actions;
}



public class WaveGenerator : MonoBehaviour
{
	public List<Wave> waves;
	private Wave m_CurrentWave;
	public Wave CurrentWave { get {return m_CurrentWave;} }

	IEnumerator SpawnLoop()
	{
		foreach(Wave W in waves)
		{
			m_CurrentWave = W;
			foreach(WaveAction A in W.actions)
			{
				if (A.beforeMessage != "")
				{
					UI_WaveMessages messageBoard = FindObjectOfType<UI_WaveMessages>();
					messageBoard.SetUIWaveMessage(A.beforeMessage);
					//messageBoard.SendMessage("SetUIWaveMessage", A.beforeMessage);
				}

				if(A.delayBeforeWave > 0)
				{
					StartCoroutine(DrawTimerNumber((int)A.delayBeforeWave));
					yield return new WaitForSeconds(A.delayBeforeWave);
				}

				if (A.prefab != null && A.spawnCount > 0)
				{
					for(int i = 0; i < A.spawnCount; i++)
					{
						Instantiate(A.prefab, this.transform.position, Quaternion.identity);
						yield return new WaitForSeconds(A.secondsBetweenSpawn);
					}

					// wave it not over until all zombies are dead
					while(FindObjectsOfType<Zombie>().Length > 0)
					{
						yield return null;
					}

					UI_WaveMessages messageBoard = FindObjectOfType<UI_WaveMessages>();
					messageBoard.SendMessage("SetUIWaveMessage", A.afterMessage);

				}
			}
			yield return null;  // prevents crash if all delays are 0
		}
		yield return null;  // prevents crash if all delays are 0
	}

	IEnumerator DrawTimerNumber(int max)
	{
		GameObject g = GameObject.Find ("CountDown");
		Text t = g.GetComponent<Text>();
		t.enabled = true;
		for (int i = max - 1; i > -1; i--) {
			t.text = i.ToString();
			yield return new WaitForSeconds(1);
		}
		t.enabled = false;
	}


	void Start()
	{
		StartCoroutine(SpawnLoop());
	}

	void Update()
	{
	}
	
}