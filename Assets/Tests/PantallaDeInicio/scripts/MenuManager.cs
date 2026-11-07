using UnityEngine;
using UnityEngine.SceneManagement;
public class MenuManager : MonoBehaviour
{
    public void IniciarPartida()
    {
        SceneManager.LoadScene("Cambiar A Nombre Escena");
    }

    public void SalirDelJuego()
    {
        Application.Quit();
    }
}


/* SI ESTA TODO EN LA MISMA ESCENA, USAR ESTO ARRIBA Y LUEGO METER LOS EVENTOS DE LA TAREA SI NO ESTÁ EN OTRA ESCENA.
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject hudInicio;
    [SerializeField] private GameObject hudJuego;

    public void IniciarPartida()
    {
        if (hudInicio != null) hudInicio.SetActive(false);
        if (hudJuego != null) hudJuego.SetActive(true);

       
    }
}*/