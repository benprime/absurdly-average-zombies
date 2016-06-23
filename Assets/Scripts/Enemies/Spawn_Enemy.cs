using UnityEngine;

public class Spawn_Enemy : MonoBehaviour
{
    public GameObject prefab;
    public bool spawnReady = true;
    public float spawnDelayMin = 1f, spawnDelayMax = 3f;
    public int maxZombies = 10;
    private int numSpawned = 0;
    private float spawnDelay;
    private float timer = 0;

    // Use this for initialization
    void Start()
    {
        spawnDelay = Random.Range(spawnDelayMin, spawnDelayMax);
    }

    // Update is called once per frame
    void Update()
    {
        if (numSpawned <= maxZombies)
        {
            if (spawnReady)
            {
                GameObject z = Instantiate(prefab, this.transform.position, Quaternion.identity) as GameObject;
                z.GetComponent<Zombie>().path = GetComponent<Path_Create>().path;
                spawnReady = false;
                spawnDelay = Random.Range(spawnDelayMin, spawnDelayMax);
                numSpawned++;
            }
            timer += Time.deltaTime;
            if (timer >= spawnDelay)
            {
                spawnReady = true;
                timer = 0;
            }
        }
        else
        { //done spawning
            Destroy(gameObject);
        }
    }

}
