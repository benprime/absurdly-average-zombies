using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ControllingCameraAspectScript : MonoBehaviour
{
    public const float SIXTEEN_NINE = 16.0f / 9.0f; // 1.777777
    public const float SIXTEEN_TEN = 16.0f / 10.0f; // 1.6
    public const float THREE_TWO = 3.0f / 2.0f; // 1.5
    public const float FOUR_THREE = 4.0f / 3.0f; // 1.33333

    void Start()
    {
        //float aspect = (float)Screen.width / (float)Screen.height;
        //Debug.Log("Camera aspect: " + Camera.main.aspect);
        //Debug.Log("Screen width: " + Screen.width);
        //Debug.Log("Screen height: " + Screen.height);
        //Debug.Log("Aspect value: " + Camera.main.aspect);

        if (Camera.main.aspect == SIXTEEN_NINE)
        {
            //Debug.Log("Aspect: 16:9");
            Camera.main.orthographicSize = 5.65f;
        }
        else if (Camera.main.aspect == SIXTEEN_TEN)
        {
            //Debug.Log("Aspect: 16:10");
            Camera.main.orthographicSize = 5.65f;
        }
        else if (Camera.main.aspect == THREE_TWO)
        {
            //Debug.Log("Aspect: 3:2");
            Camera.main.orthographicSize = 6.00f;
        }
        else if (Camera.main.aspect == FOUR_THREE)
        {
            //Debug.Log("Aspect: 4:3");
            Camera.main.orthographicSize = 6.60f;
        }
        else
        {
            //Debug.Log("Aspect: Default");
            Camera.main.orthographicSize = 5.65f;
        }

    }
}