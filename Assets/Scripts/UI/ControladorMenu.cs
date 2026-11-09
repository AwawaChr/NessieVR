using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class ControladorMenu : MonoBehaviour
{
    public GameObject canvasPrincipal;
    public GameObject canvasMenu;
    private AudioSource audioSource;

    void Start()
    {
        
        if (canvasPrincipal != null) canvasPrincipal.SetActive(true);
        if (canvasMenu != null) canvasMenu.SetActive(false);
    }


    public void AbrirMenu()
    {
        if (canvasPrincipal != null) canvasPrincipal.SetActive(false);
        if (canvasMenu != null) canvasMenu.SetActive(true);

       
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.ReproducirMusica();
        }
    }

    public void VolverAlMenuInicio()
{
    if (AudioManager.Instance != null)
    {
        AudioManager.Instance.DetenerMusica();
    }

    SceneManager.LoadScene("Inicio");
}
}