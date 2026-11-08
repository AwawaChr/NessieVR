using UnityEngine;

public class MusicaMenu : MonoBehaviour
{
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        
        {
            audioSource.Play();
        }
    }

   
    void OnDestroy()
    {
        if (audioSource != null)
        {
            audioSource.Stop();
        }
    }
}