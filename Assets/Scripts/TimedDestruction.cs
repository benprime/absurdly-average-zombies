using UnityEngine;
using System.Collections;

public class TimedDestruction : MonoBehaviour
{
    public float lifespan = 1f;
    private float timer = 0;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= lifespan) Destroy(this.gameObject);
    }
}
