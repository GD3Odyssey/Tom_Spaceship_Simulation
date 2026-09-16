using System.Collections;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public TextMeshProUGUI fuelText;
    public Scrollbar fuelScrollbar;
    private int decimalPlaces = 2;
    private float bumpScale = 1.2f;
    private float bumpDuration = 0.25f;
    private bool fueled = false;
    private Coroutine bumpRoutine;
    private Coroutine fuelRoutine;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    void Start()
    {
        fuelText.text = GameManager.instance.fuel.ToString("F0");
        fuelScrollbar.size = GameManager.instance.fuel / 1000f;
    }

    public void UpdateFuel(float fuel, System.Action<bool> callback)
    {
        if (fuelRoutine != null)
            StopCoroutine(fuelRoutine);

        fueled = true;

        fuelRoutine = StartCoroutine(UpdateFuelCoroutine(fuel, callback));

        if (bumpRoutine == null)
            bumpRoutine = StartCoroutine(BumpLoopCoroutine());
    }

    IEnumerator UpdateFuelCoroutine(float fuel, System.Action<bool> callback)
    {
        float startFuel = float.Parse(fuelText.text);
        float targetFuel = fuel;
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime * 3f;
            float currentFuel = Mathf.Lerp(startFuel, targetFuel, t);

            fuelText.text = currentFuel.ToString("F" + decimalPlaces);
            fuelScrollbar.size = currentFuel / 1000f;

            yield return null;
        }

        fueled = false;

        callback?.Invoke(true);
    }

    IEnumerator BumpLoopCoroutine()
    {
        RectTransform textRect = fuelText.rectTransform;
        RectTransform barRect = fuelScrollbar.GetComponent<RectTransform>();

        while (true)
        {
            if (!fueled)
            {
                textRect.localScale = Vector3.one;
                barRect.localScale = Vector3.one;
                bumpRoutine = null;
                yield break;
            }

            float t = 0f;
            while (t < 1f)
            {
                if (!fueled) goto STOP_BUMP;

                t += Time.deltaTime / bumpDuration;
                float s = Mathf.Lerp(1f, bumpScale, t);

                textRect.localScale = new Vector3(s, s, 1f);
                barRect.localScale = new Vector3(s, s, 1f);

                yield return null;
            }

            t = 0f;
            while (t < 1f)
            {
                if (!fueled) goto STOP_BUMP;

                t += Time.deltaTime / bumpDuration;
                float s = Mathf.Lerp(bumpScale, 1f, t);

                textRect.localScale = new Vector3(s, s, 1f);
                barRect.localScale = new Vector3(s, s, 1f);

                yield return null;
            }
        }

    STOP_BUMP:
        textRect.localScale = Vector3.one;
        barRect.localScale = Vector3.one;
        bumpRoutine = null;
    }
}