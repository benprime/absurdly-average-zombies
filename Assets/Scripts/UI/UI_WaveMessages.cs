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
        //gm = FindObjectOfType<GameManager_Stats>();
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

    // TODO: put in start() of wave generator
    //UI_WaveMessages messageBoard = FindObjectOfType<UI_WaveMessages>();

    // TODO: put wherever the message needs to be called
    //messageBoard.SendMessage("SetUIWaveMessage", "wave compl333te");

    public void SetUIWaveMessage(string mess, int timeout)
    {
        gameObject.GetComponent<Text>().text = mess;
        timer = timeout;
    }
}
