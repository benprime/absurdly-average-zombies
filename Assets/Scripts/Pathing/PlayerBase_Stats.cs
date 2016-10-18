using UnityEngine;

public class PlayerBase_Stats : MonoBehaviour
{
    public float maxHitPoints = 100f;
    public float currentHitPoints = 100f;
    public GameObject gameOverPopup;
    public Sprite noDamage, thirdDamage, twoThirdDamage, destroyed;
    private bool dead = false;
    public static PlayerBase_Stats Instance = null;
	WaveGenerator wg = null;
	WaveGeneratorTutorial wgt = null;

	public AudioClip damageSound, destructionSound;

    // guarantee no one can instantiate this
    protected PlayerBase_Stats()
    {
        // hacky, but we're just trying to expose a public reference.
        Instance = this;
    }

    // Use this for initialization
    void Start()
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = noDamage;
		wg = FindObjectOfType<WaveGenerator>();
		wgt = FindObjectOfType<WaveGeneratorTutorial>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TakeDamage(float damage)
    {
		UI_FloatingHealthBar hb = GetComponent<UI_FloatingHealthBar>();

		AudioSource aud = GetComponent<AudioSource>();
        if (!dead)
        {
            if (currentHitPoints <= 0)
            {
                Transform uiCanvas = FindObjectOfType<Canvas>().transform;
                var gameOverMsg = Instantiate(gameOverPopup) as GameObject;
                gameOverMsg.transform.SetParent(uiCanvas);
				gameOverMsg.transform.localScale = new Vector3(1, 1, 1);

				if (wg) {
					wg.EndWaves ();
				}
				else if (wgt) {
					wgt.EndWaves ();
				}

                Destroy(hb.healthBar.gameObject);
                gameObject.GetComponent<SpriteRenderer>().sprite = destroyed;
                dead = true;
				Instantiate(destroyed, transform.position, transform.rotation);

				aud.Stop();
				aud.PlayOneShot (destructionSound, .5f);
            }
            else if (currentHitPoints <= maxHitPoints / 3)
            {
                hb.healthBar.color = Color.red;
				gameObject.GetComponent<SpriteRenderer>().sprite = twoThirdDamage;

            }
            else if (currentHitPoints <= 2 * (maxHitPoints / 3))
            {
                hb.healthBar.color = Color.yellow;
				gameObject.GetComponent<SpriteRenderer>().sprite = thirdDamage;

            }
		}

		if (currentHitPoints > 0)
		{
			currentHitPoints -= damage;
			if (hb.healthBar) hb.healthBar.rectTransform.localScale = new Vector3(Mathf.Max(0, (currentHitPoints / maxHitPoints)), 1, 1);

			//Make this play through before playing the next clip
			//aud.Stop();
			if(!aud.isPlaying)
				aud.PlayOneShot (damageSound, Random.Range(.6f,1.0f));
		}
    }
}
