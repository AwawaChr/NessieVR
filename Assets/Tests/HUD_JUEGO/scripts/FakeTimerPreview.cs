
using UnityEngine;
using TMPro;

public class FakeTimerPreview : MonoBehaviour
{
    [Header("UI References")]
    public TMP_Text timerText;

    [Header("Settings")]
    public float totalTime = 60f;

    private float remainingTime;

    void Start()
    {
        remainingTime = totalTime;

        if (timerText == null)
            timerText = GetComponent<TMP_Text>();

        UpdateTimerDisplay();

        Debug.Log("FAKE TIMER STARTED");
    }

    void Update()
    {
        if (remainingTime <= 0f)
            return;

        remainingTime -= Time.deltaTime;
        remainingTime = Mathf.Max(remainingTime, 0f);

        UpdateTimerDisplay();
    }

    void UpdateTimerDisplay()
    {
        int minutes = Mathf.FloorToInt(remainingTime / 60f);
        int seconds = Mathf.FloorToInt(remainingTime % 60f);

        timerText.text = minutes.ToString("00") + ":" +
                         seconds.ToString("00");
    }
}
