
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToMenu : MonoBehaviour
{
    [Header("Navigation")]
    [SerializeField] private string sceneToLoad = "MenuEscena";

    public void ReturnToMenu()
    {
        Debug.Log("BACK BUTTON CLICKED");
        SceneManager.LoadScene(sceneToLoad);
    }
}

