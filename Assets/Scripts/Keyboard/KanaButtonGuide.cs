using TMPro;
using UnityEngine;

public class KanaButtonGuide : MonoBehaviour
{
    Transform OneBig;
    TMP_Text OneBigTMP;
    Transform Five;
    TMP_Text[] FiveTMP;

    string[] strings;
    public int prevIndex = -1;

    Vector3 position;
    Vector3 relUpOffset = new Vector3(0f, 50f, 0f);

    public void Initialize()
    {
        OneBig = transform.GetChild(0);
        OneBigTMP = OneBig.GetComponent<TMP_Text>();

        Five = transform.GetChild(1);
        FiveTMP = new TMP_Text[5];
        for (int i = 0; i < 5; i++)
        {
            FiveTMP[i] = Five.GetChild(i).GetComponent<TMP_Text>();
        }
    }

    public void SetIndex(int index)
    {
        if (index != prevIndex)
        {
            if (index == 0)
            {
                SetToFive();
                transform.position = position;
            }
            else
            {
                SetToOneBig();
                if (index == 2)
                    transform.position = position + relUpOffset;
                else
                    transform.position = position;

                // Set text of OneBig
                OneBigTMP.text = strings[index];
            }
        }
    }

    void SetToOneBig()
    {
        OneBig.gameObject.SetActive(true);
        Five.gameObject.SetActive(false);
    }
    void SetToFive()
    {
        OneBig.gameObject.SetActive(false);
        Five.gameObject.SetActive(true);
    }

    public void SetActivate(Vector3 pos, float y_diff, string[] keys)
    {
        pos.y += y_diff;
        relUpOffset.y = y_diff / 2f;

        position = pos;
        transform.position = position;
        this.strings = keys;

        SetToFive();

        // Change text aspects
        for (int i = 0; i < 5; i++)
        {
            FiveTMP[i].text = strings[i];
        }
    }
}
