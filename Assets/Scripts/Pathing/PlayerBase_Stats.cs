using UnityEngine;
using System.Collections;

public class PlayerBase_Stats : MonoBehaviour
{
    public float maxHitPoints = 100f;
    public float currentHitPoints = 100f;
    public GameObject gameOverPopup;
    public Sprite noDamage, thirdDamage, twoThirdDamage, destroyed;
	private bool dead = false;

    // Use this for initialization
    void Start()
	{
		gameObject.GetComponent<SpriteRenderer>().sprite = noDamage;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void TakeDamage(float damage)
	{
		UI_FloatingHealthBar hb = GetComponent<UI_FloatingHealthBar>();

		if(currentHitPoints > 0) {
			currentHitPoints -= damage;
	        if (hb.healthBar) hb.healthBar.rectTransform.localScale = new Vector3(Mathf.Max(0, (currentHitPoints / maxHitPoints)), 1, 1); //TODO: Make healthbar scale from left pivot point
		}
		if (!dead) {
			if (currentHitPoints <= 0) {
				Transform uiCanvas = FindObjectOfType<Canvas> ().transform;
				var gameOverMsg = Instantiate (gameOverPopup) as GameObject;
				gameOverMsg.transform.SetParent (uiCanvas);
				gameOverMsg.transform.localScale = new Vector3 (1, 1, 1);

				//Transform uiCanvas = FindObjectOfType<Canvas>().transform;
				//clone.transform.SetParent(uiCanvas, false);
				Destroy (hb.healthBar.gameObject);
				gameObject.GetComponent<SpriteRenderer> ().sprite = destroyed;
				dead = true;
				Instantiate (destroyed, transform.position, transform.rotation);
				//Destroy(gameObject);  //TODO:  Make the destroyed house stay on ground for game over (possibly use instantiate to leave a plain sprite there)
			} else if (currentHitPoints <= maxHitPoints / 3) {
				hb.healthBar.color = Color.red;
				gameObject.GetComponent<SpriteRenderer> ().sprite = twoThirdDamage;
			} else if (currentHitPoints <= 2 * (maxHitPoints / 3)) {
				hb.healthBar.color = Color.yellow;
				gameObject.GetComponent<SpriteRenderer> ().sprite = thirdDamage;
			}
		}
    }
}
