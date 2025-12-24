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

    public static void AddEmptyBeats(ref List<Beat> beatList, int iterations)
    {
        for (int j = 0; j < iterations; j++)
        {
            beatList.Add(new Beat());
        }
    }

    // Method to turn a BeatElement scriptable object into a series of Beat class instances
    public static void ProcessElement(ref List<Beat> beatList, BeatElement element)
    {
        switch (element.GetElementType())
        {
            case BeatElement.ElementType.Char:
                BeatChar charElement = (BeatChar)element; // Cast to BeatChar
                beatList.Add(new Beat(charElement.text, charElement.romaji, charElement.clip)); // Add character to beatlist 

                break;

            case BeatElement.ElementType.Word:
                BeatWord wordElement = (BeatWord)element; // Cast to BeatWord
                int beatListStartIndex = beatList.Count; // Get the starting point. 
                for (int i = 0; i < wordElement.beatChars.Length; i++) // Iterate through characters and add them to beatlist. 
                {
                    ProcessElement(ref beatList, wordElement.beatChars[i]); // Add each character to beatlist. Populate list
                    beatList[beatListStartIndex + i].word = wordElement.text;
                }

                if (wordElement.pitch == 0) // heiban word
                {
                    beatList[beatListStartIndex].pitchIsHigh = false;
                    for (int i = 1; i < wordElement.beatChars.Length; i++)
                    {
                        beatList[beatListStartIndex + i].pitchIsHigh = true;
                    }
                }
                else // nakadaka or oodaka
                {
                    for (int i = 0; i < wordElement.beatChars.Length; i++)
                    {
                        bool isHigh = (i + 1) < wordElement.pitch; // Switches to low 
                        beatList[beatListStartIndex + i].pitchIsHigh = isHigh;
                    }
                }

                break;
            case BeatElement.ElementType.Phrase: // NOTHING YET
                // Cast to BeatPhrase
                // processe element for each word
                break;
        }
    }
}
