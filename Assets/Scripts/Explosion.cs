using UnityEngine;

public class Explosion : MonoBehaviour
{
    public AudioSource explosionAudio;
    
    void Start()
    {
        explosionAudio.Play();
        
    }
    void Update()
    {
        if (explosionAudio.isPlaying) return;
        
        Destroy(this);
    }
}
