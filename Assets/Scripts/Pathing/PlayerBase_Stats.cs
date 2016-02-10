using UnityEngine;
using System.Collections;

public class PlayerBase_Stats : MonoBehaviour
{
    public float maxHitPoints = 100f;
    public float currentHitPoints = 100f;
    public GameObject gameOverPopup;
    public Sprite noDamage, thirdDamage, twoThirdDamage, destroyed;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void TakeDamage(float damage)
    {
        currentHitPoints -= damage;
        //if(currentHitPoints < (maxHitPoints / 3)) {
        //	gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        //}
        UI_FloatingHealthBar hb = GetComponent<UI_FloatingHealthBar>();
        if (hb.healthBar) hb.healthBar.rectTransform.localScale = new Vector3(Mathf.Max(0, (currentHitPoints / maxHitPoints)), 1, 1); //TODO: Make healthbar scale from left pivot point

        if (currentHitPoints <= 0)
        {
            Instantiate(gameOverPopup);
            //Transform uiCanvas = FindObjectOfType<Canvas>().transform;
            //clone.transform.SetParent(uiCanvas, false);
            Destroy(hb.healthBar.gameObject);
            gameObject.GetComponent<SpriteRenderer>().sprite = destroyed;
			Instantiate (destroyed, transform.position, transform.rotation);
            Destroy(gameObject);  //TODO:  Make the destroyed house stay on ground for game over (possibly use instantiate to leave a plain sprite there)
        }
        if (currentHitPoints <= maxHitPoints / 3)
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
}
