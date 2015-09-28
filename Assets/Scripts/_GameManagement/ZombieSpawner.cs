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
    public float secondsBetweenSpawn;
    public int count;
}

[System.Serializable]
public class SpawnWave
{
    [HideInInspector]
    public string name = "Wave";
    public List<ZombieConfig> zombies;
}



public class ZombieSpawner : MonoBehaviour
{
    public GameObject zombiePrefab;
    public List<SpawnWave> waves;
    private SpawnWave m_CurrentWave;
    public SpawnWave CurrentWave { get { return m_CurrentWave; } }

    IEnumerator SpawnLoop()
    {
        foreach (ZombieConfig zc in m_CurrentWave.zombies)
        {
            if (zombiePrefab != null && zc.count > 0)
            {
                for (int i = 0; i < zc.count; i++)
                {
                    Instantiate(zombiePrefab, this.transform.position, Quaternion.identity);
                    yield return new WaitForSeconds(zc.secondsBetweenSpawn);
                }
            }
            else if (zc.count == 0) //a count of 0 results in a delay between spawn sets
            {
                yield return new WaitForSeconds(zc.secondsBetweenSpawn);
            }

            yield return null;  // prevents crash if all delays are 0
        }
    }


    public void BeginSpawnWave(int waveIndex)
    {
        if (waveIndex < waves.Count())
        {
            m_CurrentWave = waves[waveIndex];
            StartCoroutine(SpawnLoop());
        }
    }

    void Update()
    {
    }

}