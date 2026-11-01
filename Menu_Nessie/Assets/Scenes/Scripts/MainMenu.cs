using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [Header("Paneles")]
    [SerializeField] private GameObject panelMenu;
    [SerializeField] private GameObject panelAR;
    [SerializeField] private GameObject panelMinijuego; // Actualizado con tu nombre

    public void AbrirAR()
    {
        if (panelMenu != null) panelMenu.SetActive(false);
        if (panelAR != null) panelAR.SetActive(true);
        if (panelMinijuego != null) panelMinijuego.SetActive(false);
    }

    public void AbrirMinijuego()
    {
        if (panelMenu != null) panelMenu.SetActive(false);
        if (panelAR != null) panelAR.SetActive(false);
        if (panelMinijuego != null) panelMinijuego.SetActive(true);
    }

    public void VolverAlMenu()
    {
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