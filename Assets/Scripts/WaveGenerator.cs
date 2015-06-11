// C#
// WaveGenerator.cs
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class WaveAction
{
	public string name;
	public float delayBeforeWave;
	public Transform prefab;
	public int spawnCount;
	public string message;
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
				if(A.delayBeforeWave > 0)
					yield return new WaitForSeconds(A.delayBeforeWave);

				if (A.message != "")
				{
					// TODO: print ingame message
				}
				if (A.prefab != null && A.spawnCount > 0)
				{
					for(int i = 0; i < A.spawnCount; i++)
					{
						Instantiate(A.prefab, this.transform.position, Quaternion.identity);
						yield return new WaitForSeconds(A.secondsBetweenSpawn);
					}

					// wave it not over until all zombies are dead
					if(FindObjectsOfType<Zombie>().Length > 0)
					{
						yield return null;
					}
				}
			}
			yield return null;  // prevents crash if all delays are 0
		}
		yield return null;  // prevents crash if all delays are 0
	}


	void Start()
	{
		StartCoroutine(SpawnLoop());
	}

	void Update()
	{
	}
	
}