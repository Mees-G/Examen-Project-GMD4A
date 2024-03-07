using System;
using System.Collections;
using System.ComponentModel.Design;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;

public static class Camera_Extension
{

    public static void SmoothToTransform(this Camera camera, MonoBehaviour monoBehaviour, Transform to, AnimationCurve curve, float animationSpeed = 2.0F, Action action = null)
    {
        monoBehaviour.StartCoroutine(SmoothToTransform(camera, to, curve, animationSpeed, action));
    }

    private static IEnumerator SmoothToTransform(this Camera camera, Transform to, AnimationCurve curve, float animationSpeed, Action action = null)
    {
        float factor = 0;
        
        Vector3 startPosition = camera.transform.position;
        Vector3 endPosition = to.position;

        Quaternion startRotation = camera.transform.rotation;
        Quaternion endRotation = to.rotation;
        while (factor < 1)
        {
            camera.transform.position = Vector3.Lerp(startPosition, endPosition, curve.Evaluate(factor));
            camera.transform.rotation = Quaternion.Lerp(startRotation, endRotation, curve.Evaluate(factor));
            float add = Time.deltaTime * animationSpeed;
            yield return new WaitForSeconds(Time.deltaTime);
            factor += add;
        }
        Debug.Log("DONE W");
        if (action != null)
        {
            Debug.Log("DONE!");
            action.Invoke();
        }
    }

}
