using UnityEngine;
using UnityEditor; // Required for AssetDatabase
using System.IO;
using NUnit.Framework;
using System.Collections.Generic;

public class EditorScripts : Editor
{

    [MenuItem("Custom/Generate BeatWord SO's from dataWord.csv")]
    public static void CreateBeatWords()
    {
        // Load Kana Data
        var dataWords = CSVReader.Read("dataWords");
        var beatChars = Resources.LoadAll<BeatChar>("ScriptableObjects/BeatChar");
        string wordPath = "Assets/New BeatWord";

        AssetDatabase.CreateFolder("Assets", "New BeatWord");

        int debug_k = 0;
        for (int i = 0;  i < dataWords.Count; i++)
        {
            BeatWord newWord = ScriptableObject.CreateInstance<BeatWord>();
            string kanji = dataWords[i]["kanji"].ToString();
            string kana = dataWords[i]["kana"].ToString();
            string definition = dataWords[i]["definition"].ToString();

            newWord.text = kana;
            if (kanji != string.Empty)
                newWord.text = kanji;

            int highestLevel = 0;

            List<BeatChar> beatCharList = new List<BeatChar>();
            for (int j = kana.Length - 1; j >= 0; j--) 
            {
                if (!FindBeatChar(beatChars, kana[j].ToString(), out BeatChar beatChar)) // Find character
                {
                    string problematic = kana.Substring(j - 1, 2);
                    if (FindBeatChar(beatChars, problematic, out beatChar)) // Find combination character
                    {
                        Debug.Log(problematic + " at " + kana + " " + i.ToString());
                        //continue; // Skip a number;
                        j--;
                    }
                    else
                    {
                        Debug.LogWarning("Couldn't find BeatChar for " + problematic + " at " + kana + " " + i.ToString());
                        return;
                    }
                } 

                beatCharList.Insert(0, beatChar);

                if (beatChar.level > highestLevel)
                {
                    highestLevel = beatChar.level;
                }
            }

            newWord.pitch= 0;
            newWord.beatChars = beatCharList.ToArray();
            newWord.level = highestLevel;
            newWord.definition = definition;

            string levelPath = Path.Combine(wordPath, newWord.level.ToString());
            if (!AssetDatabase.IsValidFolder(levelPath))
            {
                AssetDatabase.CreateFolder("Assets/New BeatWord", newWord.level.ToString());
            }
            // Define the specific file path for the new asset
            string wordFinalPath = Path.Combine(wordPath, newWord.level.ToString() + "/" + newWord.text + ".asset");

            AssetDatabase.CreateAsset(newWord, wordFinalPath);
            debug_k = i;
        }
        Debug.Log(debug_k.ToString());


        // Save all pending asset changes and refresh the Project window to show the new files
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log("Successfully created new BeatWord SO's at " + wordPath);
    }
    public static bool FindBeatChar(BeatChar[] beatChars, string text, out BeatChar beatChar)
    {
        beatChar = null;
        for (int i = 0; i < beatChars.Length; i++) 
        { 
            if (beatChars[i].text == text)
            {beatChar = beatChars[i];
                return true;
            }
        }

        return false;
    }

    [MenuItem("Custom/Generate BeatChar SO's from dataKana.csv")]
    public static void CreateBeatChars()
    {
        // Load Kana Data
        var dataKana = CSVReader.Read("dataKana");

        // Load Audio
        AudioClip[] arrayClips = Resources.LoadAll<AudioClip>("KanaSounds/");
        List<AudioClip> clips = new List<AudioClip>(arrayClips);

        Dictionary<string, string> exceptions = new Dictionary<string, string>();
        exceptions.Add("ぢ","じ");
        exceptions.Add("づ","ず");
        exceptions.Add("ぢゃ","じゃ");
        exceptions.Add("ぢゅ","じゅ");
        exceptions.Add("ぢょ","じょ");


        // Define the path where the new asset will be saved
        string charPath = "Assets/New BeatChar";
        
        AssetDatabase.CreateFolder("Assets", "New BeatChar");
        AssetDatabase.CreateFolder(charPath, "Hiragana");
        AssetDatabase.CreateFolder(charPath, "Katakana");

        // Loop to create multiple assets, for example
        for (int i = 0; i < dataKana.Count; i++)
        {
            // Find AudioClip based on name. 
            if (!AudioClipIndexFromName(clips, dataKana[i]["hiragana"].ToString(), out int index))
            {
                string problematic = dataKana[i]["hiragana"].ToString();
                if (exceptions.ContainsKey(problematic))
                {
                    string alt = exceptions[problematic].ToString();
                    AudioClipIndexFromName(clips, alt, out index);
                    Debug.Log("Alternate for " + problematic + " used");
                }
                else
                {
                    Debug.LogWarning("Couldn't find audioclip for " + problematic);
                    return;
                } 
            }

            // Create an instance of the ScriptableObject in memory
            BeatChar newHiragana = CreateKanaSO(dataKana[i], "hiragana", clips[index]);
            BeatChar newKatakana = CreateKanaSO(dataKana[i], "katakana", clips[index]);

            //clips.Remove(clips[i]);

            // Define the specific file path for the new asset
            string hiraganaPath = Path.Combine(charPath, "Hiragana/" + newHiragana.text + ".asset");
            string katakanaPath = Path.Combine(charPath, "Katakana/" + newKatakana.text + ".asset");

            // Create the asset file in the project
            AssetDatabase.CreateAsset(newHiragana, hiraganaPath);
            AssetDatabase.CreateAsset(newKatakana, katakanaPath);
        }
        Debug.Log("Remaining AudioClips: " + clips.Count.ToString());

        // Save all pending asset changes and refresh the Project window to show the new files
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log("Successfully created hiragana and katakana SO at " + charPath);
    }
    public static BeatChar CreateKanaSO(Dictionary<string, object> data, string kana, AudioClip clip)
    {
        int additional = 0; if (kana == "katakana") additional = 25;

        BeatChar beatChar = ScriptableObject.CreateInstance<BeatChar>();
        string name = data[kana].ToString();
        beatChar.text = name;
        beatChar.romaji = data["romaji"].ToString();
        beatChar.level = (int)data["level"] + additional;
        beatChar.name = name; 
        beatChar.clip = clip;

        return beatChar;
    }

    public static bool AudioClipIndexFromName(List<AudioClip> clips, string name, out int index)
    {
        index = -1;
        for (int i = 0; i < clips.Count; i++) 
        {
            if (clips[i].name == name)
            {
                index = i;
                return true;
            }
        }
        return false;
    }


    

}