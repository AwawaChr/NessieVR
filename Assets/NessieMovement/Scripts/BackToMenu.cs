
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToMenu : MonoBehaviour
{
    [Header("Navigation")]
    [SerializeField] private string sceneToLoad = "MainMenu";

    public void ReturnToMenu()
    {
        SceneManager.LoadScene(sceneToLoad);
    }
}

