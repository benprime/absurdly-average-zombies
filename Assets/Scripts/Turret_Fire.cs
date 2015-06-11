using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Turret_Fire : MonoBehaviour {
	//public ParticleSystem fireSmokePrefab;
	public GameObject bulletPrefab;
	public int maxShots = 5;
	public List<GameObject> firedShots;
	public float shotDelay = .1f;
	private float shotTimer = 0;
	private bool shotReady = true;
	public Transform barrelTip;
	Animator animator;

	// Use this for initialization
	void Start () {
		this.animator = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
		if(!shotReady) {
			shotTimer += Time.deltaTime;
			if(shotTimer > shotDelay) { 
				shotTimer = 0;
				shotReady = true;
			}
		}
	}

	public void Fire() {
		if(firedShots.Count < maxShots) {
			if(!shotReady) return;
			ParticleSystem smokeP = GetComponent<ParticleSystem>();
			smokeP.Emit(1);
			//Instantiate(fireSmokePrefab, barrelTip.position, barrelTip.rotation);
			GameObject clone = Instantiate(bulletPrefab, barrelTip.position, barrelTip.rotation) as GameObject;
			firedShots.Add(clone);
			animator.SetTrigger("Fire");

			//AudioSource a = GetComponent<AudioSource>();
			//a.PlayOneShot(a.clip);
			shotReady = false;
		}
	}

}
