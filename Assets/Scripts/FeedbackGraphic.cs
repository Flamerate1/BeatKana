using System.Collections;
using UnityEngine;

public class FeedbackGraphic : MonoBehaviour
{
    public enum Degree
    {
        WrongTime, Miss, Bad, Okay, Good, Perfect
    }

    SpriteRenderer SpriteRenderer;
    public void Init()
    {
        SpriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void InitiateFeedback(Degree degree)
    {
        gameObject.SetActive(true);

        switch (degree) 
        { 
            case Degree.Miss:
                SpriteRenderer.color = Color.grey;
                break;
            case Degree.Perfect:
                SpriteRenderer.color = Color.green;
                break;
        }

        StartCoroutine(Graphic());
    }

    IEnumerator Graphic()
    {
        yield return new WaitForSeconds(0.4f);
        gameObject.SetActive(false);
    }
}
