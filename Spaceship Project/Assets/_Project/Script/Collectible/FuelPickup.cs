using UnityEngine;
using System;
using System.Collections;

public class FuelPickup : MonoBehaviour
{
    public float fuelAmount = 100f;
    public float animDuration = 0.3f;
    public float scaleMultiplier = 1.5f;

    private bool isCollected = false;

    public void Collect(Action<float> callback)
    {
        if (isCollected) return;
        isCollected = true;

        StartCoroutine(CollectRoutine(callback));
    }

    IEnumerator CollectRoutine(Action<float> callback)
    {
        Vector3 originalScale = transform.localScale;
        Vector3 bigScale = originalScale * scaleMultiplier;

        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime / animDuration;
            transform.localScale = Vector3.Lerp(originalScale, bigScale, t);
            yield return null;
        }

        t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime / animDuration;
            transform.localScale = Vector3.Lerp(bigScale, originalScale, t);
            yield return null;
        }

        Destroy(gameObject);

        callback?.Invoke(fuelAmount);
    }
}