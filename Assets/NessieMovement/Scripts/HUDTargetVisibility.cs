
using UnityEngine;
using Vuforia;

public class HUDTargetVisibility : MonoBehaviour
{
    [SerializeField] private GameObject arHUD;

    private ObserverBehaviour observer;

    void Start()
    {
        observer = GetComponent<ObserverBehaviour>();

        // Hide the complete HUD at startup
        if (arHUD != null)
            arHUD.SetActive(false);

        if (observer != null)
            observer.OnTargetStatusChanged += OnTargetStatusChanged;
    }

    private void OnTargetStatusChanged(
        ObserverBehaviour behaviour,
        TargetStatus status)
    {
        bool detected =
            status.Status == Status.TRACKED ||
            status.Status == Status.EXTENDED_TRACKED;

        if (arHUD != null)
            arHUD.SetActive(detected);
    }

    private void OnDestroy()
    {
        if (observer != null)
            observer.OnTargetStatusChanged -= OnTargetStatusChanged;
    }
}
