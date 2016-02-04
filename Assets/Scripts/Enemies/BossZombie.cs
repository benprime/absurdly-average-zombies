using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class BossZombie : Zombie {

	private List<ZombieSpawner> spawners;
	public int bossSpawnInterval = 2;
	private float bossTimer = 0f;

	void Awake()
	{
		//zSize = ZombieSize.Boss;
		spawners = new List<ZombieSpawner> ();
	}

	// Use this for initialization
	protected override void Start () {
		base.Start ();
		foreach (ZombieSpawner zs in FindObjectsOfType<ZombieSpawner> ()) {
			if (zs.spawnType == ZombieSize.Small || zs.spawnType == ZombieSize.Medium) {
				spawners.Add (zs);
			}
		}
	}
	
	// Update is called once per frame
	protected override void Update () {
		base.Update ();
		if (spawners.Count > 0) {
			if (bossTimer >= bossSpawnInterval) {
				spawners [Random.Range (0, spawners.Count)].SpawnZombie (ZombieSize.Medium);
				bossTimer = 0;
			}
			bossTimer += Time.deltaTime;
		}
	}
}
