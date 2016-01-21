using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class EnterInputToContinue : MonoBehaviour
{
    public string nextLevel = "SelectLevel";
    public bool useInputToContinue = true;
    public bool changesAutomatically = false;
    public float autoTime = 5f;
    private float timer = 0;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (useInputToContinue && (Input.anyKey || (Input.touchCount > 0)))
        {
            SceneManager.LoadScene(nextLevel);
        }
        if (changesAutomatically)
        {
            timer += Time.deltaTime;
            if (timer >= autoTime)
            {
                SceneManager.LoadScene(nextLevel);
            }
        }
    }
}
