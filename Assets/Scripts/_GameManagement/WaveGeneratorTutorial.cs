using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WaveGeneratorTutorial : MonoBehaviour
{
    public int startingMoney;
    public List<Wave> waves;
    private Wave m_CurrentWave;
    public Wave CurrentWave { get { return m_CurrentWave; } }
    private Text waveHeaderText;
    private Text waveMessageText;
    private Image popupImage;
    private Text countDownText;
    private GameObject PopupMessage;
    private int currentWaveIndex = 0;
    public AudioClip levelMusic;
    float timer = 0f;
    public AudioClip[] zombieSounds;

    // images for popup window
    public Sprite spriteMachineGun;

    // TODO:
    // 1. disable and enable menu items.
    // 2. button glow

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
        foreach (Wave W in waves)
        {
            // update current wave
            m_CurrentWave = W;

            this.ShowMessage("Welcome to Zombie Towers!", "This is the machine gun.  It is your most basic turret type.", this.spriteMachineGun);

            UI_WeaponRadial.buttonDisabled["N"] = false; // machine gun
            UI_WeaponRadial.buttonDisabled["S"] = true;
            UI_WeaponRadial.buttonDisabled["E"] = true;
            UI_WeaponRadial.buttonDisabled["W"] = true;
            UI_UpgradeRadial.buttonDisabled["N"] = true;
            UI_UpgradeRadial.buttonDisabled["S"] = true;
            UI_UpgradeRadial.buttonDisabled["E"] = true;
            UI_UpgradeRadial.buttonDisabled["W"] = true;

        

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
            ZombieSpawner[] spawns = FindObjectsOfType<ZombieSpawner>();
            foreach (ZombieSpawner zs in spawns)
            {
                zs.BeginSpawnWave(currentWaveIndex);
            }

            //allow 1 second for spawners to start
            yield return new WaitForSeconds(1);
            float sounder = Random.Range(2f, 5f);
            // wave is not over until all zombies are dead
            while (FindObjectsOfType<Zombie>().Length > 0)
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

        // a level is completed
        //this.ShowMessage("Congratulations!", "Level Complete!");

        // do nothing while the popup message is up
        while (this.PopupMessage.activeSelf)
        {
            yield return null;
        }

        GameManager.instance.progressManager.CompleteLevel(SceneManager.GetActiveScene().name);
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
            this.countDownText.text = i.ToString();
            yield return new WaitForSeconds(1);
        }
        this.countDownText.enabled = false;
    }

    void Start()
    {
        // get references to everything up front
        this.countDownText = GameObject.Find("CountDown").GetComponent<Text>();

        this.PopupMessage = GameObject.Find("PopupWithPicture");

        this.waveHeaderText = this.PopupMessage.transform.FindChild("HeaderPanel").transform.FindChild("HeaderText").GetComponent<Text>();
        this.waveMessageText = this.PopupMessage.transform.FindChild("BodyPanel").transform.FindChild("MessageText").GetComponent<Text>();
        this.popupImage = this.PopupMessage.transform.FindChild("BodyPanel").transform.FindChild("Image").GetComponent<Image>();

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