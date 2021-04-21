using System;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField] private Sound[] sounds;
    
    private Dictionary<string, Sound> soundDict = new Dictionary<string, Sound>();
    
    public bool IsOn { get; private set; }

    private void Awake()
    {
        Instance = this;
        IsOn = true;

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;

            soundDict.Add(s.name, s);
            s.actualVolume = s.volume;
        }
    }
    
    public void Play(string name)
    {
        Sound s = soundDict[name];
        if (s == null)
        {
            Debug.LogError($"Sound {name} not found in AudioManager");
            return;
        }
        s.source.Play();
    }
    
    public void MuteAll()
    {
        foreach(KeyValuePair<string, Sound> entry in soundDict)
        {
            entry.Value.source.Stop();
        }
    }
    
    
    public void MuteAllLooped()
    {
        foreach(KeyValuePair<string, Sound> entry in soundDict)
        {
            if(entry.Value.loop)
                entry.Value.source.Stop();
        }
    }

}

