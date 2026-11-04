
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HUDController : MonoBehaviour
{
    [Header("Panels")]
    public GameObject pausePanel;
    public GameObject soundPanel;

    [Header("Pause Button Icons")]
    public Image pauseButtonImage;
    public Sprite pauseSprite;
    public Sprite playSprite;

    [Header("Audio Sources")]
    public AudioSource musicSource;
    public AudioSource effectsSource;

    [Header("Sound Sliders")]
    public Slider musicSlider;
    public Slider effectsSlider;


    // INITIALIZATION
    void Start()
    {
        Time.timeScale = 1f;

        // Start with both panels hidden
        if (pausePanel != null)
            pausePanel.SetActive(false);

        if (soundPanel != null)
            soundPanel.SetActive(false);

        // Set initial pause icon
        if (pauseButtonImage != null)
            pauseButtonImage.sprite = pauseSprite;

        // Initialize music slider
        if (musicSource != null && musicSlider != null)
        {
            musicSlider.value = musicSource.volume;
            musicSlider.onValueChanged.AddListener(ChangeMusicVolume);
        }

        // Initialize effects slider
        if (effectsSource != null && effectsSlider != null)
        {
            effectsSlider.value = effectsSource.volume;
            effectsSlider.onValueChanged.AddListener(ChangeEffectsVolume);
        }
    }


    // PAUSE / RESUME
    public void TogglePause()
    {
        if (Time.timeScale > 0f)
            PauseGame();
        else
            ContinueGame();
    }

    public void PauseGame()
    {
        if (pausePanel != null)
            pausePanel.SetActive(true);

        if (soundPanel != null)
            soundPanel.SetActive(false);

        Time.timeScale = 0f;

        if (pauseButtonImage != null)
            pauseButtonImage.sprite = playSprite;
    }

    public void ContinueGame()
    {
        if (pausePanel != null)
            pausePanel.SetActive(false);

        if (soundPanel != null)
            soundPanel.SetActive(false);

        Time.timeScale = 1f;

        if (pauseButtonImage != null)
            pauseButtonImage.sprite = pauseSprite;
    }


    // SOUND PANEL
    public void OpenSoundPanel()
    {
        if (soundPanel != null)
            soundPanel.SetActive(true);
    }

    public void CloseSoundPanel()
    {
        if (soundPanel != null)
            soundPanel.SetActive(false);

        if (pausePanel != null)
            pausePanel.SetActive(true);
    }


    // AUDIO CONTROLS
    public void ChangeMusicVolume(float value)
    {
        if (musicSource != null)
            musicSource.volume = value;
    }

    public void ChangeEffectsVolume(float value)
    {
        if (effectsSource != null)
            effectsSource.volume = value;
    }


    // RESTART GAME
    public void RestartGame()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex
        );
    }


    // RETURN TO MAIN MENU
    public void BackToMenu()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene("MenuEscena");
    }


    // CLEAN UP
    void OnDestroy()
    {
        if (musicSlider != null)
            musicSlider.onValueChanged.RemoveListener(ChangeMusicVolume);

        if (effectsSlider != null)
            effectsSlider.onValueChanged.RemoveListener(ChangeEffectsVolume);
    }
}
