using UnityEngine;
using UnityEngine.SceneManagement;
public class MenuManager : MonoBehaviour
{
    public void IniciarPartida()
    {
        SceneManager.LoadScene("Sandbox_GroundPlane");
    }

    public void SalirDelJuego()
    {
        Application.Quit();
    }
}
