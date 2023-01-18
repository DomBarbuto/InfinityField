using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class msuicManager : MonoBehaviour
{
    public static msuicManager instance;

    [SerializeField] public Slider musicVolumeSlider;
    [SerializeField] public AudioSource aud;

    public void setMusicVolume()
    {
        float musicSliderValue = musicVolumeSlider.value;
        aud.volume = musicSliderValue;
    }

    private void Start()
    {
        instance = this;
    }

}
