using UnityEngine;

[CreateAssetMenu(fileName = "AudioManager", menuName = "Singltons/Game Managers/Audio Manager")]
public class AudioManager : ScriptableObject
{
    private AudioSource _audio;

    public AudioSource Audio { get => _audio; set => _audio = value; }


    /// <summary>
    /// Using this to play the given audio  clip
    /// </summary>
    /// <param name="clip"></param>
    public void PlaySound(AudioClip clip)
    {
        _audio.PlayOneShot(clip);
    }

}