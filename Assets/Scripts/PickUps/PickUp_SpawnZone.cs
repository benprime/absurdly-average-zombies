using UnityEngine;
using System.Collections;
using Assets.Scripts;

public class PickUp_SpawnZone : MonoBehaviour
{

    public Transform pickUpPrefab;

    public float spawnRateMin = 2f, spawnRateMax = 5f;
    private float spawnRate = 2f;
    private float nextSpawn = 0f;

    private IWaveGenerator wg;

    private BuildZone[] buildZones;

    // Use this for initialization
    void Start()
    {
        spawnRate = Random.Range(2.0f, 5.0f);
        nextSpawn = Time.time + spawnRate;
        wg = FindObjectOfType<WaveGenerator>();
        if (wg == null)
        {
            wg = FindObjectOfType<WaveGeneratorTutorial>();
        }
        buildZones = FindObjectsOfType<BuildZone>();
    }

    // Update is called once per frame
    void Update()
    {
        if (wg.IsWaveInProgress())
        {
            if (Time.time > nextSpawn)
            {
                spawnRate = Random.Range(2.0f, 5.0f);
                nextSpawn = Time.time + spawnRate;
                Vector2 spawnBox = new Vector2(transform.localScale.x, transform.localScale.y);
                Vector2 randomPos;
                do
                {
                    randomPos = new Vector2(Random.Range(-.5f, .5f) * spawnBox.x, Random.Range(-.5f, .5f) * spawnBox.y);

                } while (!isPositionValid(randomPos));

                Instantiate(pickUpPrefab, randomPos, Quaternion.identity);
            }
        }
        else nextSpawn = Time.time + spawnRate;
    }

    private bool isPositionValid(Vector3 pos)
    {
        foreach (BuildZone bz in buildZones)
        {
            BoxCollider2D boxCollider = bz.gameObject.GetComponent<BoxCollider2D>();
            if (boxCollider.bounds.Contains(pos))
            {
                return false;
            }
        }
        return true;
    }
}