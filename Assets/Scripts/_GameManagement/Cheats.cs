using UnityEngine;
using UnityEngine.SceneManagement;

public class Cheats : MonoBehaviour
{

    public void UnlockAllLevels()
    {
        foreach (LevelProgressData data in GameManager.Instance.progressManager.LevelProgress)
        {
            data.Locked = false;
        }
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(sceneIndex);
    }


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
