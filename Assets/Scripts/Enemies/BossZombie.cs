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
				spawners.Add (zs);
		}
	}
	
	// Update is called once per frame
	protected override void Update () {
		base.Update ();
		if (spawners.Count > 0) {
			if (bossTimer >= bossSpawnInterval) {
				foreach (ZombieSpawner zx in spawners) {
					float rando = Random.Range(0f, 1f);
					if (rando < .6)
						zx.SpawnZombie (ZombieSize.Medium);
					else if (rando < .9)
						zx.SpawnZombie (ZombieSize.Small);
					else if (rando < .999)
						zx.SpawnZombie (ZombieSize.Large);
					else
						zx.SpawnZombie (ZombieSize.Boss);
				}
				bossTimer = 0;
			}
			bossTimer += Time.deltaTime;
		}
	}
}
