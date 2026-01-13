using System.Collections.Generic;
using UnityEngine;

public class KanaData
{
    //private static KanaData _instance;
    //public static KanaData Instance => _instance ??= new KanaData();
    private static KanaData _instance = new KanaData();

    private static BiDictionary<char, char> toKomojiData = new BiDictionary<char, char>();
    private static BiDictionary<char, char> toDakutenData = new BiDictionary<char, char>();
    private static BiDictionary<char, char> toHandakutenData = new BiDictionary<char, char>();
    private static BiDictionary<char, char> switchData = new BiDictionary<char, char>();

    /*
    private static Dictionary<char, char> toKomojiData = new Dictionary<char, char>();
    private static Dictionary<char, char> toDakutenData = new Dictionary<char, char>();
    private static Dictionary<char, char> toHandakutenData = new Dictionary<char, char>();
    private static Dictionary<char, char> switchData = new Dictionary<char, char>();
    */
    private static Dictionary<char, char> katakanaSwitchData = new Dictionary<char, char>();
    //private static BiDictionary<char, char> katakanaSwitchData = new BiDictionary<char, char>();

    private struct data
    {
        public string[] komoji, dakuten, handakuten, switchArray;
        public string hiragana, katakana;
    }
    private static data _data = new data()
    {
        // Between big and small letters ONE WAY
        komoji = new string[2] { // big, small
            "あいえおやゆよつわ",
            "ぁぃぇぉゃゅょっゎ"
        },
        dakuten = new string[2] { // normal, dakuten
            "かきくけこさしすせそたちつてとはひふへほ",    // removed っぱぴぷぺぽ
            "がぎぐげござじずぜぞだぢづでどばびぶべぼ"     // removed づばびぶべぼ
        },
        handakuten = new string[2] { // h_normal, h_handakuten
            "はひふへほ",   // removed ばびぶべぼ
            "ぱぴぷぺぽ"    // removed ぱぴぷぺぽ
        },
        switchArray = new string[4] { // normal, small, dakuten, handakuten
            "あいうえおかきくけこさしすせそたちつてとはひふへほやゆよわ",
            "ぁぃぅぇぉ　　　　　　　　　　　　っ　　　　　　　ゃゅょゎ",
            "　　ゔ　　がぎぐげござじずぜぞだぢづでどばびぶべぼ　　　　",
            "　　　　　　　　　　　　　　　　　　　　ぱぴぷぺぽ　　　　"
        },
        hiragana = "あいうえおかきくけこさしすせそたちつてとなにぬねのはひふへほまみむめもやゆよらりるれろわをんぁぃぅぇぉゃゅょっゎがぎぐげござじずぜぞだぢづでどばびぶべぼぱぴぷぺぽゔ",
        katakana = "アイウエオカキクケコサシスセソタチツテトナニヌネノハヒフヘホマミムメモヤユヨラリルレロワヲンァィゥェォャュョッヮガギグゲゴザジズゼゾダヂヅデドバビブベボパピプペポヴ"
    };

    #region Kana Processing Methods
    
    // Keyboard Specific 
    public static bool ToKomoji(char kana, out char opposite) => toKomojiData.TryGetValue(kana, out opposite); 
    public static bool ToDakuten(char kana, out char opposite) => toDakutenData.TryGetValue(kana, out opposite); 
    public static bool ToHandakuten(char kana, out char opposite) => toHandakutenData.TryGetValue(kana, out opposite); 
    public static bool SwitchKana(char kana, out char next) => switchData.TryGetValue(kana, out next);
    // END Keyboard Specific 

    // Beat Processing
    public static bool IsKomoji(char kana) => _data.komoji[1].Contains(kana);
    public static bool IsDakuten(char kana) => _data.dakuten[1].Contains(kana); 
    public static bool IsHandakuten(char kana) => _data.handakuten[1].Contains(kana);

    public static bool FromKomoji(char kana, out char opposite) => toKomojiData.TryGetKey(kana, out opposite);
    public static bool FromDakuten(char kana, out char opposite) => toDakutenData.TryGetKey(kana, out opposite);
    public static bool FromHandakuten(char kana, out char opposite) => toHandakutenData.TryGetKey(kana, out opposite);


    private static BiDictionary<char, char> testing = new BiDictionary<char, char>();
    #endregion

    static KanaData()
    {

        // Between big and small letters ONE WAY
        for (int i = 0; i < _data.komoji[0].Length; i++)
        {
            toKomojiData.Add(_data.komoji[0][i], _data.komoji[1][i]);
        }

        // Between dakuten and non dakuten letters ONE WAY
        for (int i = 0; i < _data.dakuten[0].Length; i++)
        {
            toDakutenData.Add(_data.dakuten[0][i], _data.dakuten[1][i]);
        }

        // Between handakuten and non handakuten letters ONE WAY
        for (int i = 0; i < _data.handakuten[0].Length; i++)
        {
            toHandakutenData.Add(_data.handakuten[0][i], _data.handakuten[1][i]);
        }
        
        // algorithm for switchData
            //1. Iterate over string.Length
            //2. Loop over array.Length
            //3. Once found non empty section, put that in dictionary
            //4. Loop again but starting with the last Value. 
            //5. If can't find anymore, new value is section from level 0. 

        char space = '　';
        for (int i = 0; i < _data.switchArray[0].Length; i++)
        {
            char key = _data.switchArray[0][i];
            for (int j = 1; j < _data.switchArray.Length; j++)
            {
                char value = _data.switchArray[j][i];
                if (value != space)
                {
                    switchData.Add(key, value);
                    key = value;
                }
            }
            switchData.Add(key, _data.switchArray[0][i]);
        }

        // Between hiragana and katakana TWO WAY
        for (int i = 0; i < _data.hiragana.Length; i++)
        {
            katakanaSwitchData.Add(_data.hiragana[i], _data.katakana[i]);
            katakanaSwitchData.Add(_data.katakana[i], _data.hiragana[i]);
        }
    }
    /*static KanaData()
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
            //1. Iterate over string.Length
            //2. Loop over array.Length
            //3. Once found non empty section, put that in dictionary
            //4. Loop again but starting with the last Value. 
            //5. If can't find anymore, new value is section from level 0. 
         

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
    }*/



    // To prevent external creation
    private KanaData() { }
}
