using UnityEngine;
using TMPro;

// Makes a text slowly fade in and out (used on "Press Any Key")
public class BlinkText : MonoBehaviour
{
    public float blinkSpeed = 1.5f;   // how fast it fades in and out
    public float minAlpha = 0.15f;    // how faded it gets (0 = invisible)

    private TMP_Text text;

    public void Start()
    {
        text = GetComponent<TMP_Text>();
    }

    public void Update()
    {
        // PingPong counts up from 0 to 1 and back down to 0 over and over
        float t = Mathf.PingPong(Time.unscaledTime * blinkSpeed, 1f);

        // Lerp turns that 0-1 value into an alpha between minAlpha and fully visible
        Color color = text.color;
        color.a = Mathf.Lerp(minAlpha, 1f, t);
        text.color = color;
    }
}