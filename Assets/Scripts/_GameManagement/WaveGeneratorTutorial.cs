using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[System.Serializable]
public class WeaponButtonStates
{
    public bool N = true;
    public bool S = true;
    public bool E = true;
    public bool W = true;
}

[System.Serializable]
public class UpgradeButtonStates
{
    public bool E = true;
    public bool W = true;
}

[System.Serializable]
public class MessageData
{
    [HideInInspector]
    public string name = "Message";

    public string header;
    [TextArea]
    public string message;
    public Sprite sprite;
}

[System.Serializable]
public class TutorialWave
{
    [HideInInspector]
    public string name = "Wave";
    public float delayBeforeWave;

    public MessageData[] Messages;

    // additional fields for tutorial waves
    public int startingMoney;
    public WeaponButtonStates weaponRadial;
    public UpgradeButtonStates upgradeRadial;
    public bool clearTurrets;
}

public class WaveGeneratorTutorial : MonoBehaviour, IWaveGenerator
{
    public List<TutorialWave> waves;
    private TutorialWave m_CurrentWave;
    public TutorialWave CurrentWave { get { return m_CurrentWave; } }

    private Text waveHeaderText;
    private Text waveMessageText;
    private Image popupImage;
    private Text countDownText;
    private GameObject PopupMessage;
    private int currentWaveIndex = 0;
    public AudioClip levelMusic;
    public bool isWaveActive = false;

    public MessageData SuccessMessage;

    void ShowMessage(MessageData messageData)
    {
        Screen.sleepTimeout = SleepTimeout.SystemSetting;
        this.PopupMessage.SetActive(true);

        this.waveHeaderText.text = messageData.header;
        this.waveMessageText.text = messageData.message;
        this.popupImage.sprite = messageData.sprite;

        BuildZone[] allZones = FindObjectsOfType<BuildZone>();
        if (allZones.Length > 0)
        {
            foreach (BuildZone zone in allZones)
            {
                zone.CloseOut();
            }
        }
    }

    IEnumerator SpawnLoop()
    {
        foreach (TutorialWave W in waves)
		{
			if (currentWaveIndex >= waves.Count)
				break;
			
            // update current wave
            m_CurrentWave = W;

            GameManager.Instance.player_totalCurrency = m_CurrentWave.startingMoney;

            foreach (MessageData messageData in W.Messages)
            {
                this.ShowMessage(messageData);

                // do nothing while the popup message is up
                while (this.PopupMessage.activeSelf)
                {
                    yield return null;
                }
            }

            UI_WeaponRadial.buttonDisabled["N"] = !W.weaponRadial.N;
            UI_WeaponRadial.buttonDisabled["S"] = !W.weaponRadial.S;
            UI_WeaponRadial.buttonDisabled["E"] = !W.weaponRadial.E;
            UI_WeaponRadial.buttonDisabled["W"] = !W.weaponRadial.W;
            UI_UpgradeRadial.buttonDisabled["E"] = !W.upgradeRadial.E;
            UI_UpgradeRadial.buttonDisabled["W"] = !W.upgradeRadial.W;

            if (W.clearTurrets)
            {
                BuildZone[] bzs = FindObjectsOfType<BuildZone>();
                foreach (BuildZone bz in bzs)
                {
                    bz.Clear();
                }
            }

            // do nothing while the popup message is up
            while (this.PopupMessage.activeSelf)
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
            isWaveActive = true;
            ZombieSpawner[] spawns = FindObjectsOfType<ZombieSpawner>();
            foreach (ZombieSpawner zs in spawns)
            {
                //possibly add a yield return here
                StartCoroutine(zs.BeginSpawnWave(currentWaveIndex));
            }

            //allow 1 second for spawners to start
            yield return new WaitForSeconds(1);
            
            // wave is not over until all zombies are dead
            while (isWaveActive || FindObjectsOfType<Zombie>().Length > 0)
            {
                yield return new WaitForSeconds(0.1f);
            }

            // A wave has complete
            currentWaveIndex++;
            yield return null;  // prevents crash if all delays are 0
		}

		if(PlayerBase_Stats.Instance.currentHitPoints > 0)
		{
			ShowMessage(this.SuccessMessage);

	        // do nothing while the popup message is up
	        while (this.PopupMessage.activeSelf)
	        {
	            yield return null;
	        }

	        GameManager.Instance.GetComponent<AudioSource>().clip = GameManager.Instance.menuMusic;
	        GameManager.Instance.GetComponent<AudioSource>().Play();
	        SceneManager.LoadScene("SelectLevel");
	        yield return null;  // prevents crash if all delays are 0
		}
    }

    IEnumerator DrawBeforeText(TutorialWave w)
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

        this.PopupMessage = GameObject.Find("PopupMessageWithPicture");

        GameObject headerPanel = this.PopupMessage.transform.Find("HeaderPanel").gameObject;
        GameObject bodyPanel = headerPanel.transform.Find("BodyPanel").gameObject;
        this.waveHeaderText = bodyPanel.transform.Find("HeaderText").GetComponent<Text>();
        this.waveMessageText = bodyPanel.transform.Find("MessageText").GetComponent<Text>();
        this.popupImage = bodyPanel.transform.Find("Image").GetComponent<Image>();

        if (levelMusic != null || GameManager.Instance.GetComponent<AudioSource>().clip != levelMusic)
        {
            GameManager.Instance.GetComponent<AudioSource>().clip = levelMusic;
            GameManager.Instance.GetComponent<AudioSource>().Play();
        }

        currentWaveIndex = 0;
        StartCoroutine(SpawnLoop());
    }

    void Update()
    {
    }

    public bool IsWaveInProgress()
    {
        return (isWaveActive || GameObject.FindGameObjectsWithTag("enemy").Length > 0);
	}

	public void EndWaves() {
		isWaveActive = false;
		currentWaveIndex = 100;
	}

}