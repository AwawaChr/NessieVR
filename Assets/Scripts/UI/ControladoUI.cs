
using UnityEngine;

public class ControladoUI : MonoBehaviour
{
    [Header("Elementos UI")]
    [SerializeField] private GameObject Menu;
    [SerializeField] private GameObject Sonido;

    public void AbrirMenu()
    {
        if (!Menu.activeSelf)
        {
            Menu.SetActive(true);
            Sonido.SetActive(false);

            Time.timeScale = 0f;
        }
        else
        {
            Menu.SetActive(false);
            Sonido.SetActive(false);

            Time.timeScale = 1f;
        }
    }

    public void AbrirMusica()
    {
        if (!Sonido.activeSelf)
        {
            Menu.SetActive(false);
            Sonido.SetActive(true);

            Time.timeScale = 0f;
        }
        else
        {
            Sonido.SetActive(false);
            Menu.SetActive(true);

            Time.timeScale = 0f;
        }
    }
}
