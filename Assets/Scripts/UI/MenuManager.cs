using UnityEngine;
using UnityEngine.SceneManagement;
public class MenuManager : MonoBehaviour
{
    public void IniciarPartida()
    {
        SceneManager.LoadScene("CondicionesGanarPerder");
    }

    public void SalirDelJuego()
    {
        Application.Quit();
    }
}
