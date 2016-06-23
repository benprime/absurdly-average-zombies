using UnityEngine;

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

        if (currentHitPoints > 0)
        {
            currentHitPoints -= damage;
            if (hb.healthBar) hb.healthBar.rectTransform.localScale = new Vector3(Mathf.Max(0, (currentHitPoints / maxHitPoints)), 1, 1);
        }
        if (!dead)
        {
            if (currentHitPoints <= 0)
            {
                Transform uiCanvas = FindObjectOfType<Canvas>().transform;
                var gameOverMsg = Instantiate(gameOverPopup) as GameObject;
                gameOverMsg.transform.SetParent(uiCanvas);
                gameOverMsg.transform.localScale = new Vector3(1, 1, 1);

                Destroy(hb.healthBar.gameObject);
                gameObject.GetComponent<SpriteRenderer>().sprite = destroyed;
                dead = true;
                Instantiate(destroyed, transform.position, transform.rotation);
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
    }
}
