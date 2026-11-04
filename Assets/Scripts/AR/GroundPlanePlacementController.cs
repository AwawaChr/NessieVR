using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using Vuforia;

public class GroundPlanePlacementController : MonoBehaviour
{
    public static event Action OnScenarioAnchoredAndReady;

    [Header("Referencias de Vuforia")]
    [SerializeField] private PlaneFinderBehaviour planeFinder;
    [SerializeField] private ContentPositioningBehaviour contentPositioning;
    [SerializeField] private GameObject groundPlaneStage;

    [Header("Objetivo del Escenario")]
    [Tooltip("El modelo del archipiélago (inicialmente FUERA del Stage)")]
    [SerializeField] private Transform scenarioContentRoot;
    [SerializeField] private PlacementPreviewFeedback previewFeedback;

    [Header("Referencias de UI")]
    [SerializeField] private GameObject placementPanelUI;
    [SerializeField] private Button confirmButton;
    [SerializeField] private GameObject gameplayPanelUI;

    [Header("Configuración de Entrada")]
    [SerializeField] private bool allowTapToPlace = true;

    private bool isPlaced = false;
    private bool hasValidSurface = false;
    private Pose lastHitPose;

    private void Awake()
    {
        if (confirmButton != null)
        {
            confirmButton.onClick.AddListener(ConfirmPlacement);
            confirmButton.interactable = false;
        }

        if (placementPanelUI != null) placementPanelUI.SetActive(true);
        if (gameplayPanelUI != null) gameplayPanelUI.SetActive(false);
    }

    private void OnEnable()
    {
        if (planeFinder != null)
        {
            planeFinder.OnAutomaticHitTest.AddListener(HandleAutomaticHitTest);
            planeFinder.OnInteractiveHitTest.AddListener(HandleInteractiveHitTest);
        }
    }

    private void OnDisable()
    {
        if (planeFinder != null)
        {
            planeFinder.OnAutomaticHitTest.RemoveListener(HandleAutomaticHitTest);
            planeFinder.OnInteractiveHitTest.RemoveListener(HandleInteractiveHitTest);
        }

        if (confirmButton != null)
        {
            confirmButton.onClick.RemoveListener(ConfirmPlacement);
        }
    }

    private void Start()
    {
        if (previewFeedback != null)
        {
            previewFeedback.SetPreviewMode(true);
        }

        if (scenarioContentRoot != null)
        {
            scenarioContentRoot.gameObject.SetActive(false); // Oculto hasta el primer hit
        }
    }

    private void Update()
    {
        if (isPlaced) return;

        if (allowTapToPlace && Touchscreen.current != null)
        {
            var touch = Touchscreen.current.primaryTouch;
            if (touch.press.wasPressedThisFrame)
            {
                Vector2 touchPosition = touch.position.ReadValue();

                if (!IsPointerOverUI(touchPosition))
                {
                    planeFinder.PerformHitTest(touchPosition);
                }
            }
        }
    }

    private void HandleAutomaticHitTest(HitTestResult result)
    {
        if (isPlaced) return;

        hasValidSurface = (result != null);

        if (hasValidSurface)
        {
            lastHitPose = new Pose(result.Position, result.Rotation);

            if (scenarioContentRoot != null)
            {
                if (!scenarioContentRoot.gameObject.activeSelf)
                {
                    scenarioContentRoot.gameObject.SetActive(true);
                }

                scenarioContentRoot.position = result.Position;
                scenarioContentRoot.rotation = result.Rotation;
            }

            if (confirmButton != null && !confirmButton.interactable)
            {
                confirmButton.interactable = true;
            }
        }
    }

    private void HandleInteractiveHitTest(HitTestResult result)
    {
        if (isPlaced || result == null) return;

        lastHitPose = new Pose(result.Position, result.Rotation);
        ConfirmPlacement();
    }

    public void ConfirmPlacement()
    {
        if (isPlaced || !hasValidSurface) return;

        // 1. Posicionar el Stage de Vuforia exactamente en la posición detectada
        if (groundPlaneStage != null)
        {
            groundPlaneStage.transform.SetPositionAndRotation(lastHitPose.position, lastHitPose.rotation);
        }

        // 2. Emparentar el archipiélago al Stage para que herede el Positional Device Tracker de Vuforia
        if (scenarioContentRoot != null && groundPlaneStage != null)
        {
            scenarioContentRoot.SetParent(groundPlaneStage.transform, true);
        }

        CompleteAnchoringSequence();
    }

    private void CompleteAnchoringSequence()
    {
        isPlaced = true;

        if (planeFinder != null)
        {
            planeFinder.gameObject.SetActive(false);
        }

        if (previewFeedback != null)
        {
            previewFeedback.SetPreviewMode(false);
        }

        if (placementPanelUI != null) placementPanelUI.SetActive(false);
        if (gameplayPanelUI != null) gameplayPanelUI.SetActive(true);

        OnScenarioAnchoredAndReady?.Invoke();
    }

    private bool IsPointerOverUI(Vector2 screenPosition)
    {
        if (EventSystem.current == null) return false;

        PointerEventData eventData = new PointerEventData(EventSystem.current)
        {
            position = screenPosition
        };

        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);
        return results.Count > 0;
    }
}