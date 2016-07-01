using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class PopZombie : Zombie
{
	public GameObject smZombiePrefab, mdZombiePrefab, lgZombiePrefab;
	public int minToPop = 3, maxToPop = 5;
	private int numToPop = -1;
	public float zombieSpaceDelay = .5f;

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

	protected override void Die ()
	{
		if (numToPop < 0) {
			numToPop = Random.Range (minToPop, maxToPop + 1);
			GetComponent<SpriteRenderer> ().enabled = false;
			StartCoroutine (PopZombies (numToPop));
		}
	}

	public IEnumerator PopZombies(int numToPop)
	{
		for (int i = 0; i < numToPop; i++) { 
			ZombieSize size;
			float rando = Random.Range (0f, 1f);
			if (rando < .6)
				size = ZombieSize.Medium;
			else if (rando < .99)
				size = ZombieSize.Small;
			else
				size = ZombieSize.Large;

			GameObject z = null;

			if (size == ZombieSize.Small && smZombiePrefab)
				z = Instantiate (smZombiePrefab, this.transform.position, Quaternion.identity) as GameObject;
			else if (size == ZombieSize.Medium && mdZombiePrefab)
				z = Instantiate (mdZombiePrefab, this.transform.position, Quaternion.identity) as GameObject;
			else if (size == ZombieSize.Large && lgZombiePrefab)
				z = Instantiate (lgZombiePrefab, this.transform.position, Quaternion.identity) as GameObject;

			if (z) {
				z.GetComponent<Zombie> ().SetPath (path, currentNodeIndex);
			}

			yield return new WaitForSeconds (zombieSpaceDelay);
		}
		base.Die ();
	}
}
