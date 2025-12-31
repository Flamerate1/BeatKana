using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;
using static UnityEngine.Rendering.DebugUI;

public class KanaData
{
    //private static KanaData _instance;
    //public static KanaData Instance => _instance ??= new KanaData();
    private static KanaData _instance = new KanaData();

    private static Dictionary<char, char> komojiData;
    private static Dictionary<char, char> dakutenData;
    private static Dictionary<char, char> handakutenData;
    private static Dictionary<char, char> switchData;


    private static BiDictionary<char, char> katakanaSwitchData;

    public static bool ToFromKomoji(char kana, out char opposite)
    {
        // Check small data
        return komojiData.TryGetValue(kana, out opposite);
    }
    public static bool ToFromDakuten(char kana, out char opposite)
    {
        // Chekc dakuten data
        return dakutenData.TryGetValue(kana, out opposite);
    }
    public static bool ToFromHandakuten(char kana, out char opposite)
    {
        // Check handakuten data
        return handakutenData.TryGetValue(kana, out opposite);
    }
    public static bool SwitchKana(char kana, out char next)
    {
        // Check Switch data
        return switchData.TryGetValue(kana, out next);
    }


    static KanaData()
    {
        // Between big and small letters ONE WAY
        string big =
            "あいえおやゆよつわ";
        string small =
            "ぁぃぇぉゃゅょっゎ";
        for (int i = 0; i < big.Length; i++)
        {
            komojiData.Add(big[i], small[i]);
        }

        // Between dakuten and non dakuten letters ONE WAY
        string basic =
            "かきくけこさしすせそたちつってとはひふへほぱぴぷぺぽ";
        string dakuten =
            "がぎぐげござじずぜぞだぢづづでどばびぶべぼばびぶべぼ";
        for (int i = 0; i < basic.Length; i++)
        {
            dakutenData.Add(basic[i], dakuten[i]);
        }

        // Between handakuten and non handakuten letters ONE WAY
        string h_normal =
            "はひふへほばびぶべぼ";
        string h_handakuten =
            "ぱぴぷぺぽぱぴぷぺぽ"; 
        for (int i = 0; i < h_normal.Length; i++)
        {
            handakutenData.Add(h_normal[i], h_handakuten[i]);
        }

        // Between all of the different types (dakuten, handakuten, small)
        string[] switchArray = new string[4];
        switchArray[0] =
            "あいうえおかきくけこさしすせそたちつてとはひふへほやゆよわ";
        switchArray[1] =
            "ぁぃぅぇぉ　　　　　　　　　　　　っ　　　　　　　ゃゅょゎ";
        switchArray[2] =
            "　　ゔ　　がぎぐげござじずぜぞだぢづでどばびぶべぼ　　　　";
        switchArray[3] =
            "　　　　　　　　　　　　　　　　　　　　ぱぴぷぺぽ　　　　";

        // algorithm for switchData
        /*  1. Iterate over string.Length
            2. Loop over array.Length
            3. Once found non empty section, put that in dictionary
            4. Loop again but starting with the last Value. 
            5. If can't find anymore, new value is section from level 0. 
         */

        char space = '　';
        for (int i = 0; i < switchArray[0].Length; i++) 
        {
            char key = switchArray[0][i];
            for (int j = 1; j < switchArray.Length; j++)
            {
                char value = switchArray[j][i];
                if (value != space)
                {
                    switchData.Add(key, value);
                    key = value;
                }
            }
            switchData.Add(key, switchArray[0][i]);
        }

        // Between hiragana and katakana TWO WAY
        string hiragana =
            "あいうえおかきくけこさしすせそたちつてとなにぬねのはひふへほまみむめもやゆよらりるれろわをんぁぃぅぇぉゃゅょっゎがぎぐげござじずぜぞだぢづでどばびぶべぼぱぴぷぺぽゔ";
        string katakana =
            "アイウエオカキクケコサシスセソタチツテトナニヌネノハヒフヘホマミムメモヤユヨラリルレロワヲンァィゥェォャュョッヮガギグゲゴザジズゼゾダヂヅデドバビブベボパピプペポヴ";
        for (int i = 0; i < hiragana.Length; i++)
        {
            katakanaSwitchData.Add(hiragana[i], katakana[i]);
            katakanaSwitchData.Add(katakana[i], hiragana[i]);
        }
    }



    // To prevent external creation
    private KanaData() { }
}
