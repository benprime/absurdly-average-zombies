using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_WaveMessages : MonoBehaviour
{
    //private GameManager_Stats gm;
    private float timer = 0f;

    // Use this for initialization
    void Awake()
    {
        gameObject.GetComponent<Text>().text = "";
    }

    // Update is called once per frame
    void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            if (timer <= 0) gameObject.GetComponent<Text>().text = "";
        }
    }

    public void SetUIWaveMessage(string mess, int timeout)
    {
        gameObject.GetComponent<Text>().text = mess;
        timer = timeout;
    }
}
