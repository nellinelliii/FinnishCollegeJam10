using UnityEngine;
using TMPro;
using System.Collections;

public class CreditsController : MonoBehaviour
{
    public TextMeshProUGUI line1;
    public TextMeshProUGUI line2;
    public TextMeshProUGUI line3;

    void Start()
    {
        StartCoroutine(PlayCredits());
    }

    IEnumerator PlayCredits()
    {
        yield return new WaitForSeconds(2f);
        yield return FadeIn(line1);
        yield return new WaitForSeconds(3f);
        yield return FadeIn(line2);
        yield return new WaitForSeconds(3f);
        yield return FadeIn(line3);
        yield return new WaitForSeconds(4f);
    }

    IEnumerator FadeIn(TextMeshProUGUI text)
    {
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * 0.5f;
            text.alpha = Mathf.Lerp(0f, 1f, t);
            yield return null;
        }
        text.alpha = 1f;
    }
}