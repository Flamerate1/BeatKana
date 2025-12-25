using UnityEngine;
using UnityEngine.UI;

public class SummaryScreen : MonoBehaviour
{
    Image overlayImage;
    Color overlayColor;

    public void Initialize()
    {
        // Deactivate stuff and change alpha of the primary image. 
        //overlayColor = Color.black;
        //overlayColor.a = 0f;
        overlayImage = GetComponent<Image>();
        //overlayImage.color = overlayColor;
        //overlayColor.a = 156f / 255f;
    }

    public void Activate()
    {
        // everything that happens when the summary screen is being used. 
    }
}
