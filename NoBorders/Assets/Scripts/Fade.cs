using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Fade : MonoBehaviour
{
    Image img => GetComponent<Image>();
    public float duration = 1f;

    public void FadeOut()
    {
        StartCoroutine(FadeImage());
    }
 
    IEnumerator FadeImage()
    {
        for (float i = 0; i <= 1; i += Time.deltaTime / duration) {
            img.color = new Color(img.color.r, img.color.g, img.color.b, i);
            yield return null;
        }

        for (float i = 1; i >= 0; i -= Time.deltaTime / duration) {
            img.color = new Color(img.color.r, img.color.g, img.color.b, i);
            yield return null;
        }
    }
}
