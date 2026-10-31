using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class InstructionsController : MonoBehaviour
{
    [Header("Estructura de Páginas/Tabs")]
    [Tooltip("Lista de GameObjects contenedores de cada página de instrucciones")]
    [SerializeField] private GameObject[] pages;

    [Header("Botones de Navegación")]
    [SerializeField] private Button btnNext;
    [SerializeField] private Button btnPrev;
    [SerializeField] private Button btnClose;

    [Header("Eventos de Ciclo de Vida")]
    [Tooltip("Se dispara cuando el usuario cierra las instrucciones")]
    public UnityEvent onClosed;

    private int currentPageIndex = 0;

    private void Awake()
    {
        // Enlazar listeners de botones
        if (btnClose != null) btnClose.onClick.AddListener(ClosePanel);
        if (btnNext != null) btnNext.onClick.AddListener(NextPage);
        if (btnPrev != null) btnPrev.onClick.AddListener(PreviousPage);
    }

    private void OnEnable()
    {
        // Al abrir el panel, siempre iniciar en la primera página
        ShowPage(0);
    }

    private void Update()
    {
        // Soporte para botón "Atrás" nativo de Android (Escape)
        if (Input.GetKeyDown(KeyCode.Escape))
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
        // Si hay una sola página o ninguna, ocultar botones de paginación
        if (pages == null || pages.Length <= 1)
        {
            if (btnPrev != null) btnPrev.gameObject.SetActive(false);
            if (btnNext != null) btnNext.gameObject.SetActive(false);
            return;
        }

        // Mostrar u ocultar según extremos
        if (btnPrev != null) btnPrev.interactable = (currentPageIndex > 0);
        if (btnNext != null) btnNext.interactable = (currentPageIndex < pages.Length - 1);
    }

    private void HandleAndroidBack()
    {
        // Si no está en la primera página, el botón "Atrás" de Android retrocede de página.
        // Si ya está en la primera página, cierra el panel.
        if (currentPageIndex > 0)
        {
            PreviousPage();
        }
        else
        {
            ClosePanel();
        }
    }

    public void ClosePanel()
    {
        gameObject.SetActive(false);
        onClosed?.Invoke();
    }
}