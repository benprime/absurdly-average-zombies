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
    public float AnimationMagnitude;
    public Text TextPrefab;
    public AnimationType AnimationType;

    private List<Text> textElements = new List<UnityEngine.UI.Text>();

    // Use this for initialization
    void Start()
    {

        this.transform.SetParent(Object.FindObjectOfType<Canvas>().transform);
        this.transform.localScale = new Vector3(1, 1, 1);
        int i = 0;
        foreach (char c in this.Text)
        {
            //Text t = Instantiate(TextPrefab, this.transform.position + new Vector3(i * this.CharacterSpacing, 0), Quaternion.identity) as Text;

            Text t = Instantiate(TextPrefab) as Text;
            //t.transform.position = new Vector3(i * this.CharacterSpacing, 0);
            //t.rectTransform.position = new Vector3(i * this.CharacterSpacing, 0);
            i++;
            t.transform.SetParent(this.transform);
            //TODO: figure out length of string (and size of font) and center with a calculation
            t.transform.localPosition = new Vector3(i * this.CharacterSpacing - 35, 0);
            //t.transform.localPosition = Vector3.zero;
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
            float adjustment = Mathf.PingPong(Time.time, 0.08f) - 0.04f;

            if (i % 2 == 0)
            {
                adjustment *= -1;
            }
            i++;

            t.rectTransform.position = new Vector3(t.rectTransform.position.x, adjustment + 1.2f);
        }
    }
}
