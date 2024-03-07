using System.Collections;
using TMPro;
using UnityEngine;

public static class TMP_Text_Animation_Extension
{

    public static void SetValue(this TMP_Text text, MonoBehaviour caller, int value, float animationSpeed)
    {
        caller.StartCoroutine(DoTextAnimation(text, value, animationSpeed));
    }

    private static IEnumerator DoTextAnimation(this TMP_Text text, int newValue, float animationSpeed)
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
