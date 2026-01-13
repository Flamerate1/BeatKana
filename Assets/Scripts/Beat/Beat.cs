using System.Collections.Generic;
using UnityEngine;

public class Beat
{
    public string text; // Base hiragana text
    public string word; // kanji or kana
    public string romaji; // english letters
    public Sprite sprite; // image like the object the word or phrase represents
    public AudioClip clip; // individual sound clip 
    public bool pitchIsHigh; // high or low pitch of individual sound clip
    
    // Character addition Beat
    public Beat(string text, string romaji, AudioClip clip) 
    {
        this.text = text;
        this.word = string.Empty;
        this.romaji = romaji;
        sprite = null;
        this.clip = clip;
        this.pitchIsHigh = false;
    }
    public Beat(string text, string word, string romaji, Sprite sprite, AudioClip clip, bool pitchIsHigh)
    {
        this.text = text;
        this.word = word; 
        this.romaji = romaji;
        this.sprite = sprite;
        this.clip = clip;
        this.pitchIsHigh = pitchIsHigh;
    }

    // Empty Beat
    public Beat()
    {
        this.text = string.Empty;
        this.word = string.Empty;
        this.romaji = string.Empty;
        this.sprite = null;
        this.clip = null;
        this.pitchIsHigh = false;
    }

    public static void AddEmptyBeats(ref List<Beat> beatList, int count)
    {
        for (int j = 0; j < count; j++)
        {
            beatList.Add(new Beat());
        }
    }
}
