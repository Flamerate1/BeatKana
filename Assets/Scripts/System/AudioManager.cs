using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UIElements;
using static UnityEngine.Rendering.DebugUI;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioMixer AudioMixer;

    public enum Source { Master, Tick, Kana }

    [Serializable]
    public struct AudioChannel
    {
        public AudioChannel(Source source, string name, AudioSource audioSource)
        {
            this.source = source;
            this.name = name;
            this.audioSource = audioSource;
        }
        public Source source;
        public string name;
        public AudioSource audioSource;
    }
    [SerializeField] AudioChannel[] channels = new AudioChannel[3];
    Dictionary<Source, AudioChannel> sources = new Dictionary<Source, AudioChannel>();

    public AudioClip tickClip;
    public void Init()
    {

        tickClip = Resources.Load<AudioClip>("Synth_Tick_A_hi");
        foreach (AudioChannel channel in channels)
        {
            sources.Add(channel.source, channel);
        }
    }

    // Helper method for volume calculation
    float LinearToDB(float value) { return Mathf.Log10(Mathf.Max(value, 0.0001f)) * 20f; }

    public void SetMasterVolume(float value) { SetVolume(Source.Master, value); }
    public void SetTickVolume(float value) { SetVolume(Source.Tick, value); }
    public void SetKanaVolume(float value) { SetVolume(Source.Kana, value); }

    public void SetVolume(Source source, float value)
    {
        string name = sources[source].name;
        AudioMixer.SetFloat(name, LinearToDB(value));
        PlayerPrefs.SetFloat(name, value);
    }

    public float GetVolume(Source source)
    {
        string name = sources[source].name;
        float volume = PlayerPrefs.GetFloat(name, 1f);
        return volume;
    }

    public void PlayOneShot(Source source, AudioClip audioClip)
    {
        sources[source].audioSource.PlayOneShot(audioClip);
    }

    public void PlayTick()
    {
        PlayOneShot(Source.Tick, tickClip);
    }
}
