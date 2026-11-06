using System.Collections.Generic;
using UnityEngine;

// Keeps a library of sounds by name so any script can play one with Play("Name")
public class AudioManager : MonoBehaviour
{
    // One row in the Inspector list: a name and the sound clip that goes with it
    [System.Serializable]
    public class NamedSound
    {
        public string soundName;
        public AudioClip clip;
    }

    // The list of named sounds filled in on the Inspector
    [SerializeField] private NamedSound[] sounds;

    // Dictionary: looks up a clip by its name (the key) instead of searching the whole list
    private Dictionary<string, AudioClip> soundLibrary;

    // Copies every name and clip from the Inspector list into the dictionary before the game starts
    private void Awake()
    {
        soundLibrary = new Dictionary<string, AudioClip>();
        foreach (NamedSound sound in sounds)
        {
            soundLibrary[sound.soundName] = sound.clip;
        }
    }

    // Plays the sound with this name (used by the menu buttons, e.g. "UIClick")
    public void Play(string soundName)
    {
        // Only plays it if the name exists in the dictionary
        if (soundLibrary.ContainsKey(soundName))
        {
            GameManager.instance.PlaySound(soundLibrary[soundName]);
        }
        else
        {
            // Warns in the Console if a name is typed wrong
            Debug.LogWarning("No sound named " + soundName);
        }
    }
}
