using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

[System.Serializable]
public class ZombieConfig
{
    public ZombieSize zombieType;
    public int count;
    public float secondsBetweenSpawn;
    public float startDelay;
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
    public GameObject smZombiePrefab, mdZombiePrefab, lgZombiePrefab, bsZombiePrefab;
    public List<SpawnWave> waves;
    private SpawnWave m_CurrentWave;
    public SpawnWave CurrentWave { get { return m_CurrentWave; } }

    IEnumerator SubSpawnLoop(ZombieConfig zc)
    {
        for (int i = 0; i < zc.count; i++)
        {
            SpawnZombie(zc.zombieType);
            yield return new WaitForSeconds(zc.secondsBetweenSpawn);
        }
    }

    IEnumerator SpawnLoop()
    {
        foreach (ZombieConfig zc in m_CurrentWave.zombies)
        {
            yield return new WaitForSeconds(zc.startDelay);

            if (zc.count > 0)
            {
                if (zc == m_CurrentWave.zombies.Last())
                    yield return StartCoroutine(SubSpawnLoop(zc));
                else
                    StartCoroutine(SubSpawnLoop(zc));
            }
        }
    }

    public void SpawnZombie(ZombieSize size)
    {
        GameObject z = null;

        if (size == ZombieSize.Small && smZombiePrefab) z = Instantiate(smZombiePrefab, this.transform.position, Quaternion.identity) as GameObject;
        if (size == ZombieSize.Medium && mdZombiePrefab) z = Instantiate(mdZombiePrefab, this.transform.position, Quaternion.identity) as GameObject;
        if (size == ZombieSize.Large && lgZombiePrefab) z = Instantiate(lgZombiePrefab, this.transform.position, Quaternion.identity) as GameObject;
        if (size == ZombieSize.Boss && bsZombiePrefab) z = Instantiate(bsZombiePrefab, this.transform.position, Quaternion.identity) as GameObject;

        if (z) z.GetComponent<Zombie>().path = this.GetComponentInParent<Path_Create>().path;
    }


    public IEnumerator BeginSpawnWave(int waveIndex)
    {
        if (waveIndex < waves.Count())
        {
            m_CurrentWave = waves[waveIndex];
            yield return StartCoroutine(SpawnLoop());

            // Little hacky, but it'll do for now
            WaveGenerator w = FindObjectOfType<WaveGenerator>();
            if (w != null)
            {
                w.isWaveActive = false;
            }
            else
            {
                WaveGeneratorTutorial t = FindObjectOfType<WaveGeneratorTutorial>();
                t.isWaveActive = false;
            }
        }
    }

    void Update()
    {
    }

}