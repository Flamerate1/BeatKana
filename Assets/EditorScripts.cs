#if UNITY_EDITOR

using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor; // Required for AssetDatabase
using UnityEngine;

public class EditorScripts : Editor
{


    #region  Update BeatWord's with pitch data from dataPitch.csv
    public static BeatWord[] GrabOrderedBeatWords()
    {
        return Resources.LoadAll<BeatWord>("ScriptableObjects/BeatWord")
            .OrderBy(c => c.name)
            .ToArray();
    }
    
    [System.Serializable] 
    public class PitchRecords 
    { 
        public int[] items; 
        public string[] names;
        public PitchRecords(int[] items, string[] names)
        {
            this.items = items;
            this.names = names;
        }
    }
    [MenuItem("Custom/Find word records for pitch from dataPitch.csv")]
    public static void FindBeatWordRecord()
    {
        var dataPitch = CSVReader.Read("dataPitch");
        var beatWords = GrabOrderedBeatWords();
        int[] indices = new int[beatWords.Length]; // index corresponds to beatWords index. int is the dataPitch index.
        string[] names = new string[beatWords.Length];
        Debug.Log("beatWords.Length: " + beatWords.Length.ToString());
        for (int i = 0;  i < beatWords.Length; i++)
        {
            string kana = "";
            foreach (BeatChar beatChar in beatWords[i].beatChars)
            {
                kana += beatChar.text;
            }

            //  ---------------------------------------------------------------------- 
            //  ---------------------------------------------------------------------- 
            //  ---------------------------------------------------------------------- 
            /*int index = -1;

            bool kanaFound = FindWordPitchRecord(dataPitch, kana, out int kanaIndex);
            if (beatWords[i].name != kana)
            {
                bool kanjiFound = FindWordPitchRecord(dataPitch, beatWords[i].name, out int kanjiIndex);

                if (kanaIndex != kanjiIndex)
                {
                    index = kanjiIndex;
                }

            }
            else
            {

            }*/

            if (!FindWordPitchRecord(dataPitch, beatWords[i].name, kana, out int index))
            {
                indices[i] = -1;
                Debug.Log("Couldn't find record for " + beatWords[i].name + " at index " + i.ToString());
                continue;
            }

            //  ---------------------------------------------------------------------- 
            //  ---------------------------------------------------------------------- 
            //  ---------------------------------------------------------------------- 

            /*
            if (!FindWordPitchRecord(dataPitch, kana, out int index))
            {
                if (!FindWordPitchRecord(dataPitch, beatWords[i].name, out index))
                {
                    indices[i] = -1;
                    Debug.Log("Couldn't find record for " + beatWords[i].name + " at index " + i.ToString());
                    continue;
                }
            }*/


            indices[i] = index;
            names[i] = beatWords[i].name;
        }

        string json = JsonUtility.ToJson(new PitchRecords(indices, names));
        File.WriteAllText("Assets/pitch.json", json);

        Debug.Log("Successfully created pitch record at Assets/pitch.json");
    }
    public static bool FindWordPitchRecord(List<Dictionary<string, object>> data, string text, string kana, out int index)
    {
        index = 0;
        //int shortest_length = int.MaxValue;

        // Find word in first column. Keep looking until can't find anymore. Save length and index when shorter than shortest found. 
        int kana_index = 0;
        int kanji_index = 0;
        int both_index = 0;
        int kana_shortest_length = int.MaxValue;
        int kanji_shortest_length = int.MaxValue;
        int both_shortest_length = int.MaxValue;
        for (int i = 0; i < data.Count; i++)
        {
            string word = data[i]["word"].ToString();
            string katakana = data[i]["pitch"].ToString();
            if (word.Contains(kana) && katakana.Length < kana_shortest_length)
            {
                kana_shortest_length = katakana.Length;
                kana_index = i;
            }
            if (word.Contains(text) && katakana.Length < kanji_shortest_length)
            {
                kanji_shortest_length = katakana.Length;
                kanji_index = i;
            }
            if (word.Contains(kana) && word.Contains(text) && katakana.Length < both_shortest_length)
            {
                both_shortest_length = katakana.Length;
                both_index = i;
            }

            /*
            if (word.Contains(text) && kana.Length < shortest_length)
            {
                shortest_length = kana.Length;
                index = i;
            }
            */
        }

        if (both_shortest_length != int.MaxValue)
        {
            index = both_index;
            return true;
        }
        else if (kanji_shortest_length != int.MaxValue)
        {
            index = kanji_index;
            return true;
        }
        else if (kana_shortest_length != int.MaxValue)
        {
            index = kana_index;
            return true;
        }



        //if (shortest_length != int.MaxValue) 
        //    return true;

        /*
        Dictionary<string, string> exceptions = new Dictionary<string, string>();
        exceptions.Add("風邪","かぜ");
        exceptions.Add("賑やか","にぎやか");
        exceptions.Add("ご飯","ごはん");
        exceptions.Add("朝ご飯","あさごはん");
        exceptions.Add("昼ご飯","ひるごはん");
        exceptions.Add("晩ご飯", "ばんごはん");

        if (exceptions.TryGetValue(text, out string newText) &&
            FindWordPitchRecord(data, newText, out index))
        {
            Debug.Log(text + " was an exception successfully replaced by " + newText);
            return true;
        }*/

        return false;
    }


    [MenuItem("Custom/Update BeatWord SO's pitch from pitch.json")]
    public static void UpdateBeatWord()
    {
        if (!File.Exists("Assets/pitch.json")) return;

        var dataPitch = CSVReader.Read("dataPitch");
        var beatWords = GrabOrderedBeatWords();
        string wordPath = "Assets/Updated BeatWord";

        string json = File.ReadAllText("Assets/pitch.json");
        PitchRecords records = JsonUtility.FromJson<PitchRecords>(json);


        AssetDatabase.CreateFolder("Assets", "Updated BeatWord");

        for (int i = 0; i < beatWords.Length; i++)
        {
            BeatWord newWord = ScriptableObject.CreateInstance<BeatWord>();
            newWord.text = beatWords[i].text;
            newWord.level = beatWords[i].level;
            //newWord.pitch = beatWords[i].pitch;
            newWord.definition = beatWords[i].definition;
            newWord.beatChars = beatWords[i].beatChars;

            // Find index within pitch.json
            int index = records.items[i]; 

            // Find pitch from the original dataPitch.csv file
            newWord.pitch = WhichPitch(dataPitch[index]["pitch"].ToString());


            string levelPath = Path.Combine(wordPath, newWord.level.ToString());
            if (!AssetDatabase.IsValidFolder(levelPath))
            {
                AssetDatabase.CreateFolder("Assets/Updated BeatWord", newWord.level.ToString());
            }
            // Define the specific file path for the new asset
            string wordFinalPath = Path.Combine(wordPath, newWord.level.ToString() + "/" + newWord.text + ".asset");

            AssetDatabase.CreateAsset(newWord, wordFinalPath);
        }

        // Save all pending asset changes and refresh the Project window to show the new files
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log("Successfully created new BeatWord SO's at " + wordPath);
    }
    public static int WhichPitch(string text)
    {
        //string[] exceptions = { "ゃ", "ゅ", "ょ", "・", "ャ", "ュ", "ョ" };
        string exceptions = "ゃゅょ・ャュョ";
        string heiban = "￣";
        if (text.Contains(heiban)) { return 0; }
        string naka = "＼";

        // Return 0 if found heiban symbol. 
        if (!text.Contains(naka)) 
        {
            Debug.Log(text + " lacks a pitch marker");
            return 0;
        }

        int index = text.IndexOf(naka); // If not, look for the placement of nakadaka symbol. 
        int count = 0;
        for (int i = 0; i < index; i++) // Check for the substring up til that point for exceptions. 
        {
            string character = text[i].ToString();
            if (exceptions.Contains(character))
            {
                count++;
            }
        }

        int pitch = index - count;

        if (pitch <= 0)
        {
            Debug.Log("Calculated " + pitch.ToString() + " for " + text);
            return 0;
        }

        return pitch;
    }

    #endregion


    #region Generate BeatChar's and BeatWord's from dataWord and dataKana
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
    #endregion



}
#endif