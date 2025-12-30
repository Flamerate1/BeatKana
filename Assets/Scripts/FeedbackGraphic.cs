using System.Collections;
using UnityEngine;

public class FeedbackGraphic : MonoBehaviour
{
    public enum Degree
    {
        WrongTime, Miss, Bad, Okay, Good, Perfect
    }

    public void InitiateFeedback(Degree degree)
    {
        gameObject.SetActive(true);
        StartCoroutine(Graphic());
    }

    IEnumerator Graphic()
    {
        yield return new WaitForSeconds(0.4f);
        gameObject.SetActive(false);
    }
}
