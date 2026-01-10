using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{

    [SerializeField] AudioManager.Source source;
    Slider slider;

    private void Awake()
    {
        slider = GetComponent<Slider>();

        float _volume = GameManager.AudioManager.GetVolume(source);
        slider.value = _volume; 
        //slider.onValueChanged.AddListener(GameManager.AudioManager.SetMasterVolume);
        slider.onValueChanged.AddListener(value =>
        {
            GameManager.AudioManager.SetVolume(source, value);
        });
    }

}
