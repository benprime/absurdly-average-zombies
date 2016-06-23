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
    private List<float> startYPos = new List<float>();

    // Use this for initialization
    void Start()
    {
        this.transform.SetParent(Object.FindObjectOfType<Canvas>().transform);
        this.transform.SetAsFirstSibling();
        this.transform.localScale = new Vector3(1, 1, 1);
        int i = 0;

        // todo: do this right, hacked to center
        float x_pos_base = this.transform.position.x - 1.0f;
        foreach (char c in this.Text)
        {
            Text t = Instantiate(TextPrefab) as Text;
            i++;
            t.transform.SetParent(this.transform);
            t.transform.localPosition = Vector3.zero;
            t.rectTransform.position = new Vector3(x_pos_base + (i * this.CharacterSpacing), this.transform.position.y, 10);
            this.startYPos.Add(this.transform.position.y);
            t.transform.localScale = new Vector3(1, 1, 1);
            t.text = c.ToString();
            this.textElements.Add(t);
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < this.textElements.Count; i++)
        {
            float adjustment = Mathf.PingPong(Time.time, 0.08f) - 0.04f;

            if (i % 2 == 0)
            {
                adjustment *= -1;
            }

            this.textElements[i].rectTransform.position = new Vector3(
                this.textElements[i].rectTransform.position.x,
                this.startYPos[i] + adjustment
            );
        }
    }
}
