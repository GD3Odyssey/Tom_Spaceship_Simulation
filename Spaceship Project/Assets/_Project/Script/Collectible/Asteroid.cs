using System;
using System.Collections;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [Header("Identité")]
    [SerializeField] private int uniqueID = -1;

    [Header("Animation de destruction")]
    [SerializeField] private float animDuration = 0.25f;
    [SerializeField] private float bigScaleMultiplier = 1.25f;

    private bool isDestroyed;

    public int UniqueID => uniqueID;

    public void SetUniqueID(int id)
    {
        uniqueID = id;
    }

    public void DestroyAsteroid()
    {
        if (isDestroyed)
            return;

        isDestroyed = true;

        if (GameManager.instance != null)
            GameManager.instance.AddDestroyedAsteroid();

        Collider[] colliders = GetComponentsInChildren<Collider>();

        foreach (Collider col in colliders)
            col.enabled = false;

        StartCoroutine(DestroyRoutine());
    }

    private IEnumerator DestroyRoutine()
    {
        Vector3 originalScale = transform.localScale;
        Vector3 targetScale = originalScale * bigScaleMultiplier;

        float duration = Mathf.Max(0.01f, animDuration);
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime / duration;
            transform.localScale = Vector3.Lerp(originalScale, targetScale, Mathf.Clamp01(t));

            yield return null;
        }

        Destroy(gameObject);
    }
}