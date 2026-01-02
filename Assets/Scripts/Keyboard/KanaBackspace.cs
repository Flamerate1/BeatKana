using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;

// Alias EnhancedTouch.Touch to "Touch" for less typing.
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

public class KanaBackspace : MonoBehaviour
{
    string key = "backspace";
    Vector2 buttonBL;
    Vector2 buttonTR;

    RectTransform rectTransform;
    KanaKeyboard KanaKeyboard;

    private void OnEnable()
    {
        GameManager.OnResolutionChanged += ResetCorners;
        TouchManager.TouchBegan += ButtonClicked; TouchManager.TouchEnded += ButtonRelease; TouchManager.TouchCanceled += ButtonRelease; 
    }
    private void OnDisable() 
    {
        GameManager.OnResolutionChanged -= ResetCorners;
        TouchManager.TouchBegan -= ButtonClicked; TouchManager.TouchEnded -= ButtonRelease; TouchManager.TouchCanceled -= ButtonRelease; 
    }

    private void ButtonClicked(Touch touch)
    {
        if (GameManager.gamePaused) return;
        if (touch.screenPosition.x > buttonBL.x &&
            touch.screenPosition.y > buttonBL.y &&
            touch.screenPosition.x < buttonTR.x &&
            touch.screenPosition.y < buttonTR.y)
        {
            TouchManager.Claim(key, touch.touchId);
        }
    }

    private void ButtonRelease(Touch touch)
    {
        if (TouchManager.IsPressed(key))
        {
            KanaKeyboard.BackspaceOnField();
        }
    }

    void Start()
    {
        KanaKeyboard = GetComponentInParent<KanaKeyboard>();
        rectTransform = GetComponent<RectTransform>();
    }

    void ResetCorners()
    {
        //rectTransform.GetPositionAndRotation(out Vector3 pos, out Quaternion quat);

        Vector3[] corners = new Vector3[4];
        rectTransform.GetWorldCorners(corners);
        buttonBL = corners[0];
        buttonTR = corners[2];
    }
}
