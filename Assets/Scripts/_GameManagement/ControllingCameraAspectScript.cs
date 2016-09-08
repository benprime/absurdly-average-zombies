using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ControllingCameraAspectScript : MonoBehaviour
{
    float newAspectRatio = 16.0f / 10.0f;

    void Awake()
    {
        if (SceneManager.GetActiveScene().buildIndex >= 4)
        {
            float variance = newAspectRatio / Camera.main.aspect;
            if (variance < 1.0)
                Camera.main.rect = new Rect((1.0f - variance) / 2.0f, 0, variance, 1.0f);
            else
            {
                variance = 1.0f / variance;
                Camera.main.rect = new Rect(0, (1.0f - variance) / 2.0f, 1.0f, variance);
            }
        }
    }
}