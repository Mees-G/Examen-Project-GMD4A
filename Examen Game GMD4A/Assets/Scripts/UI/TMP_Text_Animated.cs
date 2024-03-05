using System.Collections;
using TMPro;
using UnityEngine;

public static class TMP_Text_Animated
{

    public static float animationSpeed = 0.3f;

    public static void SetValue(this TMP_Text text, MonoBehaviour caller, int value)
    {
        caller.StartCoroutine(DoTextAnimation(text, value));
    }

    private static IEnumerator DoTextAnimation(this TMP_Text text, int newValue)
    {

        float startTime = Time.time;
        if (int.TryParse(text.text, out int oldValue))
        {
            int i = oldValue;
            while (i != newValue)
            {
                float factor = Mathf.Max(Time.time - startTime, 0) * animationSpeed;
                i = (int) Mathf.SmoothStep(oldValue, newValue, factor);
                text.text = i.ToString();
                yield return new WaitForSeconds(Time.deltaTime);
            }
        }
    }

}
