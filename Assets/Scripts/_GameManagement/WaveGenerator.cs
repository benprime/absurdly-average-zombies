using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[System.Serializable]
public class Wave
{
    [HideInInspector]
    public string name = "Wave";
    public float delayBeforeWave;
	public string beforeMessageHeader;
	public string waveCountdownText;
    [TextArea]
    public string beforeMessage;
}

public class WaveGenerator : MonoBehaviour
{
    public int startingMoney;
    public List<Wave> waves;
    private Wave m_CurrentWave;
    public Wave CurrentWave { get { return m_CurrentWave; } }
    private Text waveHeaderText;
    private Text waveMessageText;
    private Text countDownText;
    private GameObject PopupMessage;
    private int currentWaveIndex = 0;
	public AudioClip levelMusic;
	float timer = 0f;
	public AudioClip[] zombieSounds;
	public bool isWaveActive = false;
    private PlayerBase_Stats playerBase;

    void ShowMessage(string headerText, string messageText)
	{
		this.PopupMessage.SetActive (true);

		this.waveHeaderText.text = headerText;
		this.waveMessageText.text = messageText;

		BuildZone[] allZones = FindObjectsOfType<BuildZone> ();
		if (allZones.Length > 0) {
			foreach (BuildZone zone in allZones) {
				zone.CloseOut ();
			}
		}
    }

    IEnumerator SpawnLoop()
    {
        foreach (Wave W in waves)
        {
            m_CurrentWave = W;

            if (!string.IsNullOrEmpty(W.beforeMessage))
            {
                Screen.sleepTimeout = SleepTimeout.SystemSetting;
                this.ShowMessage(W.beforeMessageHeader, W.beforeMessage);
            }

            // do nothing while the popup message is up
            while (this.PopupMessage.activeSelf)
            {
                yield return null;
            }

            Screen.sleepTimeout = SleepTimeout.NeverSleep;
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
			float sounder = Random.Range (2f, 5f);
            // wave is not over until all zombies are dead
			while (IsWaveInProgress())
			{
				timer += Time.deltaTime;
				if (timer >= sounder) {
					AudioSource aud = GetComponent<AudioSource> ();
					int randSound = Random.Range (0,zombieSounds.Count());			
					aud.clip = zombieSounds [randSound];
					aud.Stop ();
					aud.Play ();
					timer = 0;
					sounder = Random.Range (3f, 8f);
				}
                yield return null;
			}
			//wait a second before next wave / end level
			yield return new WaitForSeconds(1);

            // A wave has complete
            currentWaveIndex++;
            yield return null;  // prevents crash if all delays are 0
        }

        // a level is completed
        this.ShowMessage("Congratulations!", "Level Complete!");
        if (playerBase && playerBase.currentHitPoints == playerBase.maxHitPoints)
        {
            ShowMessage("Bonus!", "Your base didn't take any damage. Have five bucks!");
            GameManager.instance.bonusAmount = 5;
        }

        // do nothing while the popup message is up
        while (this.PopupMessage.activeSelf)
        {
            yield return null;
        }

        GameManager.instance.progressManager.CompleteLevel(SceneManager.GetActiveScene().name);
		Screen.sleepTimeout = SleepTimeout.SystemSetting;
		GameManager.instance.GetComponent<AudioSource>().clip = GameManager.instance.menuMusic;
		GameManager.instance.GetComponent<AudioSource>().Play();
        SceneManager.LoadScene("SelectLevel");
        yield return null;  // prevents crash if all delays are 0
    }

    IEnumerator DrawBeforeText(Wave w)
    {
        int delay = (int)w.delayBeforeWave;

        this.countDownText.enabled = true;
        for (int i = delay - 1; i > -1; i--)
        {
			this.countDownText.text = w.waveCountdownText + "\nIncoming\n" + i.ToString();
            yield return new WaitForSeconds(1);
        }
        this.countDownText.enabled = false;
    }

    void Awake()
    {
        // The normal wave generator (this class) doesn't currently
        // disable any weapon/hud buttons.  We reset them all to enabled
        // here.  If in the future, the normal non-tutorial game disables
        // any buttons, this will have to be removed.
        UI_UpgradeRadial.buttonDisabled["E"] = false;
        UI_UpgradeRadial.buttonDisabled["W"] = false;
        UI_WeaponRadial.buttonDisabled["E"] = false;
        UI_WeaponRadial.buttonDisabled["W"] = false;
        UI_WeaponRadial.buttonDisabled["N"] = false;
        UI_WeaponRadial.buttonDisabled["S"] = false;
    }

    void Start()
    {
        // get references to everything up front
        this.countDownText = GameObject.Find("CountDown").GetComponent<Text>();
        this.PopupMessage = GameObject.Find("PopupMessage");

        this.playerBase = GameObject.Find("Base_House").GetComponent<PlayerBase_Stats>();
        GameObject headerPanel = this.PopupMessage.transform.FindChild("HeaderPanel").gameObject;
        GameObject bodyPanel = headerPanel.transform.FindChild("BodyPanel").gameObject;
        this.waveHeaderText = bodyPanel.transform.FindChild("HeaderText").GetComponent<Text>();
        this.waveMessageText = bodyPanel.transform.FindChild("MessageText").GetComponent<Text>();

        if (GameManager.instance.bonusAmount != 0)
        {
            startingMoney += GameManager.instance.bonusAmount;
            GameManager.instance.bonusAmount = 0;
        }
        GameManager.instance.player_totalCurrency = startingMoney;
		if(levelMusic != null || GameManager.instance.GetComponent<AudioSource>().clip != levelMusic) {
			GameManager.instance.GetComponent<AudioSource>().clip = levelMusic;
			GameManager.instance.GetComponent<AudioSource>().Play();
		}

        currentWaveIndex = 0;
        StartCoroutine(SpawnLoop());
    }

    void Update()
    {
    }

	public bool IsWaveInProgress() {
		return (isWaveActive || GameObject.FindGameObjectsWithTag("enemy").Length > 0);
	}

}