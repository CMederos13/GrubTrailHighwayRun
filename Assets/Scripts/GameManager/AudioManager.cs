using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [System.Serializable]
    public class NamedSound
    {
        public string soundName;
        public AudioClip clip;
    }

    [SerializeField] private NamedSound[] sounds;

    private Dictionary<string, AudioClip> soundLibrary;

    private void Awake()
    {
        soundLibrary = new Dictionary<string, AudioClip>();
        foreach (NamedSound sound in sounds)
        {
            soundLibrary[sound.soundName] = sound.clip;
        }
    }

    public void Play(string soundName)
    {
        if (soundLibrary.ContainsKey(soundName))
        {
            GameManager.instance.PlaySound(soundLibrary[soundName]);
        }
        else
        {
            Debug.LogWarning("No sound named " + soundName);
        }
    }
}
