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
    private Collider2D baseCollider;

    // Use this for initialization
    void Start()
    {
        spawnRate = Random.Range(spawnRateMin, spawnRateMax);
        nextSpawn = Time.time + spawnRate;
        wg = FindObjectOfType<WaveGenerator>();
        if (wg == null)
        {
            wg = FindObjectOfType<WaveGeneratorTutorial>();
        }
        buildZones = FindObjectsOfType<BuildZone>();
        baseCollider = PlayerBase_Stats.Instance.gameObject.GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (wg.IsWaveInProgress())
        {
            if (Time.time > nextSpawn)
            {
                spawnRate = Random.Range(spawnRateMin, spawnRateMax);
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
        // dimensions of "would-be" survival pack
        Bounds packBounds = new Bounds(pos, new Vector2(1, 1));

        // check each build zone for collisions
        foreach (BuildZone bz in buildZones)
        {
            BoxCollider2D bzCollider = bz.gameObject.GetComponent<BoxCollider2D>();
            if (bzCollider.bounds.Contains(pos) || bzCollider.bounds.Intersects(packBounds))
            {
                return false;
            }
        }

        // check house for collisions
        if (baseCollider.bounds.Contains(pos) || baseCollider.bounds.Intersects(packBounds))
        {
            return false;
        }

        return true;
    }
}