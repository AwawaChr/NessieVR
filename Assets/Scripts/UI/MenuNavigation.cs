
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuNavigation : MonoBehaviour
{
    public void BackToMenu()
    {
        SceneManager.LoadScene("MenuEscena");
    }
}
