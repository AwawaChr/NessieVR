using UnityEngine;
using UnityEngine.SceneManagement; // Necesario para cargar escenas

public class MainMenu : MonoBehaviour
{
    [Header("Paneles (Opcional si usas escenas para todo)")]
    [SerializeField] private GameObject panelMenu;
    [SerializeField] private GameObject panelAR;
    [SerializeField] private GameObject panelMinijuego;

    // Métodos para cargar escenas independientes
    public void IrAInstrucciones()
    {
        SceneManager.LoadScene("Sandbox_Instrucciones");
    }

    public void IrAVideoTrailer()
    {
        SceneManager.LoadScene("Sandbox_Video");
    }

    public void AbrirAR()
    {
        // Si la AR sigue siendo un panel en esta escena:
        if (panelMenu != null) panelMenu.SetActive(false);
        if (panelAR != null) panelAR.SetActive(true);
        if (panelMinijuego != null) panelMinijuego.SetActive(false);

        // Si prefieres que AR sea otra escena, cambia esto por: SceneManager.LoadScene("NombreEscenaAR");
    }

    public void AbrirMinijuego()
    {
        // Si el minijuego sigue siendo un panel:
        if (panelMenu != null) panelMenu.SetActive(false);
        if (panelAR != null) panelAR.SetActive(false);
        if (panelMinijuego != null) panelMinijuego.SetActive(true);
    }

    public void VolverAlMenu()
    {
        // Si estás en otra escena y quieres volver al menú principal:
        // SceneManager.LoadScene("MenuEscena");

        // Si es para ocultar paneles en la misma escena:
        if (panelAR != null) panelAR.SetActive(false);
        if (panelMinijuego != null) panelMinijuego.SetActive(false);
        if (panelMenu != null) panelMenu.SetActive(true);
    }

    public void SalirJuego()
    {
        Debug.Log("Saliendo del juego...");
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}