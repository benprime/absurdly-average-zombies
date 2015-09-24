// C#
// WaveGenerator.cs
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

[System.Serializable]
public class Wave
{
    [HideInInspector]
    public string name = "Wave";
    public float delayBeforeWave;
    public string beforeMessageHeader;
    public string beforeMessage;
}



public class WaveGenerator : MonoBehaviour
{
    public int startingMoney;
    public List<Wave> waves;
    private Wave m_CurrentWave;
    public Wave CurrentWave { get { return m_CurrentWave; } }
    //private UI_WaveMessages messageBoard;
    private Text waveHeaderText;
    private Text waveMessageText;
    private Text countDownText;
    private GameObject MessagePopup;
    private int currentWaveIndex = 0;

    void ShowMessage(string headerText, string messageText)
    {
        this.MessagePopup.SetActive(true);

        this.waveHeaderText.text = headerText;
        this.waveMessageText.text = messageText;
    }

    IEnumerator SpawnLoop()
    {
        foreach (Wave W in waves)
        {
            m_CurrentWave = W;

            if (!string.IsNullOrEmpty(W.beforeMessage))
            {
                this.ShowMessage(W.beforeMessageHeader, W.beforeMessage);
            }

            // do nothing while the popup message is up
            while(this.MessagePopup.activeSelf)
            {
                yield return null;
            }

            //Display countdown to wave start
            if (W.delayBeforeWave > 0)
            {
                StartCoroutine(DrawBeforeText(W));
                yield return new WaitForSeconds(W.delayBeforeWave);
            }
            this.countDownText.enabled = false;

            //Spawners start
            ZombieSpawner[] spawns = FindObjectsOfType<ZombieSpawner>();
            foreach (ZombieSpawner zs in spawns)
            {
                zs.BeginSpawnWave(currentWaveIndex);
            }

            //allow 1 second for spawners to start
            yield return new WaitForSeconds(1);

            // wave is not over until all zombies are dead
            while (FindObjectsOfType<Zombie>().Length > 0)
            {
                yield return null;
            }

            // A wave has complete
            currentWaveIndex++;
            yield return null;  // prevents crash if all delays are 0
        }

        // a level is completed
        this.ShowMessage("Congratulations!", "Level Complete!");

        // do nothing while the popup message is up
        while (this.MessagePopup.activeSelf)
        {
            yield return null;
        }

        GameManager.instance.progressManager.CompleteLevel(Application.loadedLevelName);

        Application.LoadLevel("SelectLevel");
        yield return null;  // prevents crash if all delays are 0
    }

    IEnumerator DrawBeforeText(Wave w)
    {
        int delay = (int)w.delayBeforeWave;

        this.countDownText.enabled = true;
        for (int i = delay - 1; i > -1; i--)
        {
            this.countDownText.text = i.ToString();
            yield return new WaitForSeconds(1);
        }
        this.countDownText.enabled = false;
    }

    void Start()
    {
        // get references to everything up front
        this.countDownText = GameObject.Find("CountDown").GetComponent<Text>();

        this.MessagePopup = GameObject.Find("MessagePopup");

        this.waveHeaderText = this.MessagePopup.transform.FindChild("HeaderPanel").transform.FindChild("HeaderText").GetComponent<Text>();
        this.waveMessageText = this.MessagePopup.transform.FindChild("BodyPanel").transform.FindChild("MessageText").GetComponent<Text>();

        GameManager.instance.player_totalCurrency = startingMoney;

        currentWaveIndex = 0;
        StartCoroutine(SpawnLoop());
    }

    void Update()
    {
    }

}