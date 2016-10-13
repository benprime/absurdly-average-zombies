using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts;
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

public class WaveGenerator : MonoBehaviour, IWaveGenerator
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

    void ShowMessage(string headerText, string messageText)
    {
        this.PopupMessage.SetActive(true);

        this.waveHeaderText.text = headerText;
        this.waveMessageText.text = messageText;

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
        foreach (Wave W in waves)
        {
            if (currentWaveIndex >= waves.Count())
                break;

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
            // Display countdown to wave start
            if (W.delayBeforeWave > 0)
            {
                StartCoroutine(DrawBeforeText(W));
                yield return new WaitForSeconds(W.delayBeforeWave);
            }
            this.countDownText.enabled = false;

            // Spawners start
            isWaveActive = true;
            ZombieSpawner[] spawns = FindObjectsOfType<ZombieSpawner>();
            foreach (ZombieSpawner zs in spawns)
            {
                // possibly add a yield return here
                StartCoroutine(zs.BeginSpawnWave(currentWaveIndex));
            }

            //allow 1 second for spawners to start
            yield return new WaitForSeconds(1);
            float sounder = Random.Range(2f, 5f);
            // wave is not over until all zombies are dead
            while (IsWaveInProgress())
            {
                timer += Time.deltaTime;
                if (timer >= sounder)
                {
                    AudioSource aud = GetComponent<AudioSource>();
                    int randSound = Random.Range(0, zombieSounds.Count());
                    aud.clip = zombieSounds[randSound];
                    aud.Stop();
                    aud.Play();
                    timer = 0;
                    sounder = Random.Range(3f, 8f);
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
        if (PlayerBase_Stats.Instance.currentHitPoints > 0)
        {
            string levelStats = string.Format("Machine Guns: {0:F2} DMG / ${1} / {2:F2} POW\n",
                GameManager.Instance.BulletDamage,
                GameManager.Instance.BulletMoney,
                GameManager.Instance.BulletDamage / GameManager.Instance.BulletMoney);

            levelStats += string.Format("Flamethrowers: {0:F2} DMG / ${1} / {2:F2} POW\n",
                GameManager.Instance.FireDamage,
                GameManager.Instance.FireMoney,
                GameManager.Instance.FireDamage / GameManager.Instance.FireMoney);

            levelStats += string.Format("Rockets: {0:F2} DMG / ${1} / {2:F2} POW\n",
                GameManager.Instance.RocketDamage,
                GameManager.Instance.RocketMoney,
                GameManager.Instance.RocketDamage / GameManager.Instance.RocketMoney);

            if (PlayerBase_Stats.Instance.currentHitPoints == PlayerBase_Stats.Instance.maxHitPoints)
            {
                levelStats += "Your base didn't take any damage. Have five bucks!\n";
                GameManager.Instance.bonusAmount = 5;
            }

            this.ShowMessage("Congratulations!", "Level Completed.\n" + levelStats);

            // do nothing while the popup message is up
            while (this.PopupMessage.activeSelf)
            {
                yield return null;
            }

            GameManager.Instance.progressManager.CompleteLevel(SceneManager.GetActiveScene().name);
            Screen.sleepTimeout = SleepTimeout.SystemSetting;
            GameManager.Instance.GetComponent<AudioSource>().clip = GameManager.Instance.menuMusic;
            GameManager.Instance.GetComponent<AudioSource>().Play();
            SceneManager.LoadScene("SelectLevel");
            yield return null;  // prevents crash if all delays are 0
        }
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

        GameObject headerPanel = this.PopupMessage.transform.FindChild("HeaderPanel").gameObject;
        GameObject bodyPanel = headerPanel.transform.FindChild("BodyPanel").gameObject;
        this.waveHeaderText = bodyPanel.transform.FindChild("HeaderText").GetComponent<Text>();
        this.waveMessageText = bodyPanel.transform.FindChild("MessageText").GetComponent<Text>();

        if (GameManager.Instance.bonusAmount != 0)
        {
            startingMoney += GameManager.Instance.bonusAmount;
            GameManager.Instance.bonusAmount = 0;
        }
        GameManager.Instance.player_totalCurrency = startingMoney;
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

    public void EndWaves()
    {
        isWaveActive = false;
        currentWaveIndex = 100;
    }

}