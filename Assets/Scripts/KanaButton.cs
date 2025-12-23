using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;

// Alias EnhancedTouch.Touch to "Touch" for less typing.
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

public class KanaButton : MonoBehaviour
{
    [SerializeField] string key;
    [SerializeField] string[] keys = new string[5]; // All 5 keys the single button is capable of inputting

    float maxCenterDistScale = 2f; // How much to divide the corner distance from center
    float maxCenterDist;
    Vector2 buttonBL;
    Vector2 buttonTR;
    Vector2 buttonCenter;

    RectTransform rectTransform;
    KanaKeyboard kanaKeyboard;

    private void OnEnable() { TouchManager.TouchBegan += ButtonClicked; TouchManager.TouchEnded += ButtonRelease; TouchManager.TouchCanceled += ButtonRelease; }
    private void OnDisable() { TouchManager.TouchBegan -= ButtonClicked; TouchManager.TouchEnded -= ButtonRelease; TouchManager.TouchCanceled -= ButtonRelease; }

    private void ButtonClicked(Touch touch)
    {
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
            // Figure out which case based on KeyDetect index
            string pressed_key = keys[KeyDetect(buttonCenter, touch.screenPosition)];

            // Add this key to the TMP_InputField
            kanaKeyboard.InputToField(pressed_key);
        }
    }

    private int KeyDetect(Vector2 centerPos, Vector2 touchPos)
    {
        if (Vector2.Distance(touchPos, centerPos) < maxCenterDist) 
        {
            //Debug.Log("Close");
            return 0;
        }
        else
        {
            // Use coordinate quadrant to register direction of touch

            // 1. Get relative touch vector to button center
            Vector2 relPos = touchPos - centerPos;

            // 2. Rotate relPos vector by -45 degrees
            Quaternion rot = Quaternion.AngleAxis(-45f, Vector3.forward);
            Vector2 rotatedVector = rot * (touchPos - centerPos);

            // 3. Flip the x-axis to reorder the quadrants clockwise instead of counter clockwise.
            rotatedVector.x *= -1;

            // Detect and return coordinate quadrant
            if (rotatedVector.x >= 0f) 
            {
                if (rotatedVector.y >= 0f) return 1; 
                else return 4; 
            }
            else 
            {
                if (rotatedVector.y >= 0f) return 2; 
                else return 3; 
            }
        }
    }

    void Start()
    {
        kanaKeyboard = GetComponentInParent<KanaKeyboard>();
        rectTransform = GetComponent<RectTransform>();
        //cam = GameManager.cam;
        
        rectTransform.GetPositionAndRotation(out Vector3 pos, out Quaternion quat);
        buttonCenter = (Vector2)pos;

        Vector3[] corners = new Vector3[4];
        rectTransform.GetWorldCorners(corners);
        buttonBL = corners[0];
        buttonTR = corners[2];

        maxCenterDist = Vector2.Distance(buttonCenter, buttonTR) / maxCenterDistScale;
    }
}
