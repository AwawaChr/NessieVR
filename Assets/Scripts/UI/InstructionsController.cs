using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

public class InstructionsController : MonoBehaviour
{
    [Header("Estructura de Páginas/Tabs")]
    [Tooltip("Lista de GameObjects contenedores de cada página de instrucciones")]
    [SerializeField] private GameObject[] pages;

    [Header("Botones de Navegación")]
    [SerializeField] private Button btnNext;
    [SerializeField] private Button btnPrev;
    [SerializeField] private Button btnClose;

    [Header("Navegación de Escenas")]
    [Tooltip("Nombre exacto de la escena a cargar al salir (ej. MainMenu). Si se deja vacío, solo ocultará el panel.")]
    [SerializeField] private string returnSceneName = "MainMenu";

    [Header("Eventos de Ciclo de Vida")]
    [Tooltip("Se dispara cuando el usuario cierra las instrucciones o cambia de escena")]
    public UnityEvent onClosed;

    private int currentPageIndex = 0;

    private void Awake()
    {
        if (btnClose != null) btnClose.onClick.AddListener(CloseOrReturn);
        if (btnNext != null) btnNext.onClick.AddListener(NextPage);
        if (btnPrev != null) btnPrev.onClick.AddListener(PreviousPage);
    }

    private void OnEnable()
    {
        ShowPage(0);
    }

    private void Update()
    {
        CheckBackInput();
    }

    private void CheckBackInput()
    {
        bool backPressed = false;

#if ENABLE_INPUT_SYSTEM
        // Nuevo Input System: tecla Escape (PC) y botón Atrás en Android
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            backPressed = true;
        }
#elif ENABLE_LEGACY_INPUT_MANAGER
        // Input Manager antiguo
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            backPressed = true;
        }
#endif

        if (backPressed)
        {
            HandleAndroidBack();
        }
    }

    public void ShowPage(int index)
    {
        if (pages == null || pages.Length == 0) return;

        currentPageIndex = Mathf.Clamp(index, 0, pages.Length - 1);

        for (int i = 0; i < pages.Length; i++)
        {
            if (pages[i] != null)
            {
                pages[i].SetActive(i == currentPageIndex);
            }
        }

        UpdateNavigationButtons();
    }

    public void NextPage()
    {
        if (currentPageIndex < pages.Length - 1)
        {
            ShowPage(currentPageIndex + 1);
        }
    }

    public void PreviousPage()
    {
        if (currentPageIndex > 0)
        {
            ShowPage(currentPageIndex - 1);
        }
    }

    private void UpdateNavigationButtons()
    {
        if (pages == null || pages.Length <= 1)
        {
            if (btnPrev != null) btnPrev.gameObject.SetActive(false);
            if (btnNext != null) btnNext.gameObject.SetActive(false);
            return;
        }

        if (btnPrev != null) btnPrev.interactable = (currentPageIndex > 0);
        if (btnNext != null) btnNext.interactable = (currentPageIndex < pages.Length - 1);
    }

    private void HandleAndroidBack()
    {
        if (currentPageIndex > 0)
        {
            PreviousPage();
        }
        else
        {
            CloseOrReturn();
        }
    }

    public void CloseOrReturn()
    {
        onClosed?.Invoke();

        if (!string.IsNullOrEmpty(returnSceneName))
        {
            SceneManager.LoadScene(returnSceneName);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}