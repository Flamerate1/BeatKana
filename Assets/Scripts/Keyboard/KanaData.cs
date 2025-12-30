using System.Collections.Generic;
using UnityEngine;

public class KanaData
{

    private static KanaData _instance;
    public static KanaData Instance => _instance ??= new KanaData();


    private static Dictionary<string, string> hiraganaData;

    private static Dictionary<string, string> komoji;
    private static Dictionary<string, string> dakutenData;
    private static Dictionary<string, string> handakutenData;

    private static BiDictionary<string, string> switchData;


    public static string GrabDakutenrui(string kana)
    {
        string big =
            "あいえおやゆよ";
        string small =
            "ぁぃぇぉゃゅょ";

        string basic = 
            "かきくけこさしすせそたちつてと";
        string dakuten =
            "がぎぐげござじずぜぞだぢづでど";

        string h_normal =
            "はひふへほ";
        string h_dakuten =
            "ばびぶべぼ";
        string h_handakuten =
            "ぱぴぷぺぽ";

        return "";
    }

    public static string SwitchKana(string kana)
    {
        // Check Switch data
        return "";
    }
    public static string ToSmall(string kana)
    {
        // Check small data
        return "";
    }
    public static string ToDakuten(string kana)
    {
        // Chekc dakuten data
        return "";
    }
    public static string ToHandakuten(string kana)
    {
        // Check handakuten data
        return "";
    }


    static KanaData()
    {

    }



    // To prevent external creation
    private KanaData() { }
}
