using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public enum AnimationType
{
    AlternatingFloat
}

public class TextAnimation : MonoBehaviour
{
    public string Text;
    public float Timeout;
    public float CharacterSpacing;
    public Text TextPrefab;
    public AnimationType AnimationType;

    private List<Text> textElements = new List<UnityEngine.UI.Text>();

    // Use this for initialization
    void Start()
    {

        healthBar.rectTransform.position = new Vector3(transform.position.x, transform.position.y + barOffset, transform.position.z);

        this.transform.SetParent(Object.FindObjectOfType<Canvas>().transform, true);
        this.transform.localScale = new Vector3(1, 1, 1);
        int i = 0;
        foreach (char c in this.Text)
        {
            Text t = Instantiate(TextPrefab, this.transform.position + new Vector3(i * this.CharacterSpacing, 0), Quaternion.identity) as Text;
            i++;
            t.transform.SetParent(this.transform, true);
            t.transform.localScale = new Vector3(1, 1, 1);
            t.text = c.ToString();
            this.textElements.Add(t);
        }
    }

    // Update is called once per frame
    void Update()
    {
        int i = 0;
        foreach (Text t in this.textElements)
        {
            float adjustment = Mathf.PingPong(Time.time, 0.1f) - 0.05f;

            if (i % 2 == 0)
            {
                adjustment *= -1;
            }
            i++;

            t.transform.position = new Vector3(t.transform.position.x, adjustment);
            //t.transform.localScale = new Vector3(t.transform.localScale.x + adjustment, t.transform.localScale.y + adjustment);
        }
    }
}
