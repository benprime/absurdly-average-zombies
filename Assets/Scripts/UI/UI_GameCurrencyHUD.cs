using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_GameCurrencyHUD : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        var t = gameObject.GetComponent<Text>();
        if (t)
        {
            t.text = "$" + GameManager.instance.GetPlayerTotalCurrency();
        }
    }
}
