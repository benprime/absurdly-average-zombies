using UnityEngine;
using System.Collections;
using System.Collections.Generic;
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
public class TutorialWave
{
    [HideInInspector]
    public string name = "Wave";
    public float delayBeforeWave;
    public string beforeMessageHeader;
    [TextArea]
    public string beforeMessage;
    public Sprite messageImage;

    // additional fields for tutorial waves
    public int startingMoney;
    public WeaponButtonStates weaponRadial;
    public UpgradeButtonStates upgradeRadial;
    public bool clearTurrets;
}

public class WaveGeneratorTutorial : MonoBehaviour
{
    public int startingMoney;
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
    float timer = 0f;
    public AudioClip[] zombieSounds;
    public bool isWaveActive = false;

    void ShowMessage(string headerText, string messageText, Sprite sprite)
    {
        Screen.sleepTimeout = SleepTimeout.SystemSetting;
        this.PopupMessage.SetActive(true);

        this.waveHeaderText.text = headerText;
        this.waveMessageText.text = messageText;
        this.popupImage.sprite = sprite;

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
            // update current wave
            m_CurrentWave = W;

            this.ShowMessage(W.beforeMessageHeader, W.beforeMessage, W.messageImage);

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
            float sounder = Random.Range(2f, 5f);
            // wave is not over until all zombies are dead
            while (isWaveActive || FindObjectsOfType<Zombie>().Length > 0)
            {
                timer += Time.deltaTime;
                if (timer >= sounder)
                {
                    AudioSource aud = GetComponent<AudioSource>();
                    int randSound = Random.Range(0, zombieSounds.Length);
                    aud.clip = zombieSounds[randSound];
                    aud.Stop();
                    aud.Play();
                    timer = 0;
                    sounder = Random.Range(3f, 8f);
                }
                yield return null;
            }

            // A wave has complete
            currentWaveIndex++;
            yield return null;  // prevents crash if all delays are 0
        }

        // do nothing while the popup message is up
        while (this.PopupMessage.activeSelf)
        {
            yield return null;
        }

        //GameManager.instance.progressManager.CompleteLevel(SceneManager.GetActiveScene().name);
        GameManager.instance.GetComponent<AudioSource>().clip = GameManager.instance.menuMusic;
        GameManager.instance.GetComponent<AudioSource>().Play();
        SceneManager.LoadScene("SelectLevel");
        yield return null;  // prevents crash if all delays are 0
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

        GameObject headerPanel = this.PopupMessage.transform.FindChild("HeaderPanel").gameObject;
        GameObject bodyPanel = headerPanel.transform.FindChild("BodyPanel").gameObject;
        this.waveHeaderText = bodyPanel.transform.FindChild("HeaderText").GetComponent<Text>();
        this.waveMessageText = bodyPanel.transform.FindChild("MessageText").GetComponent<Text>();
        this.popupImage = bodyPanel.transform.FindChild("Image").GetComponent<Image>();

        GameManager.instance.player_totalCurrency = startingMoney;
        if (levelMusic != null || GameManager.instance.GetComponent<AudioSource>().clip != levelMusic)
        {
            GameManager.instance.GetComponent<AudioSource>().clip = levelMusic;
            GameManager.instance.GetComponent<AudioSource>().Play();
        }

        currentWaveIndex = 0;
        StartCoroutine(SpawnLoop());
    }

    void Update()
    {
    }

}