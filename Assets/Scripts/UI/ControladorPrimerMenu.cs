using UnityEngine;
using UnityEngine.SceneManagement;

public class ControladorPrimerMenu : MonoBehaviour
{
    public GameObject panelMenu; 
    public GameObject panelAR;   



    public void IrAInstrucciones()
    {
        SceneManager.LoadScene("Sandbox_Instrucciones");
    }

    public void IrATrailer()
    {
        SceneManager.LoadScene("Sandbox_Video");
    }

    public void AbrirPanelAR()
    {
        if (panelAR != null) panelAR.SetActive(true);
        if (panelMenu != null) panelMenu.SetActive(false);
    }

    public void IrAInicio()
    {
        SceneManager.LoadScene("Inicio");
    }

    public void SalirDelJuego()
    {
        Application.Quit();
    }

    // --- MÉTODOS DEL PANEL AR ---

    public void IrANessieVR()
    {
        SceneManager.LoadScene("NessieVR");
    }

    public void IrAModelos()
    {
        SceneManager.LoadScene("PruebasManipular");
    }

    public void VolverAlMenu()
    {
        if (panelAR != null) panelAR.SetActive(false);
        if (panelMenu != null) panelMenu.SetActive(true);
    }
}