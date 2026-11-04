using UnityEngine;
using UnityEngine.UI;
using TMPro; // Use this if your Progress_Text is TextMeshPro

public class FakeProgressBarPreview : MonoBehaviour
{
    [Header("UI References")]
    [Tooltip("Drag your Progress_Fill object here")]
    public Image fillImage;
    
    [Tooltip("Drag your Progress_Text object here (TextMeshPro or standard UI Text)")]
    public TMP_Text progressText; // Change to 'Text' if you are using standard Unity UI Text

    [Header("Settings")]
    [Tooltip("Time in seconds it takes to fill the bar completely")]
    public float fillDuration = 3f;
    
    [Tooltip("If true, the progress will loop continuously when playing")]
    public bool loop = true;

    private float timer = 0f;

    void Start()
    {
        // Auto-assign references if left empty in the inspector
        if (fillImage == null)
            fillImage = GetComponent<Image>();

        if (progressText == null)
            progressText = GetComponentInChildren<TMP_Text>();

        Debug.Log("FAKE PROGRESS STARTED", this);
    }

    void Update()
    {
        if (fillDuration <= 0f) return;

        // Increment timer
        timer += Time.deltaTime;
        float progress = timer / fillDuration;

        if (progress >= 1f)
        {
            progress = 1f;
            if (loop)
            {
                timer = 0f; // Reset for looping preview
            }
        }

        // Apply fill amount to the Image component
        if (fillImage != null)
        {
            fillImage.fillAmount = progress;
        }

        // Update the percentage text
        if (progressText != null)
        {
            int percentage = Mathf.RoundToInt(progress * 100f);
            progressText.text = percentage + "%";
        }
    }
}