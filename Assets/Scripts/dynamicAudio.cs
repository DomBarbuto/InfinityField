using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dynamicAudio : MonoBehaviour
{
    [Header("---- Audio Components ----")]
    [SerializeField] AudioSource speaker;
    [SerializeField] List<AudioClip> trackList;

    //Private Variables-----------------------------

    int upNext; //next track to play
    bool doChangeTrack; //Whether the track needs to change
    Collider col;

    //----------------------------------------------

    //----- Functions -----

    // Update is called once per frame
    private void Start()
    {
        col = GetComponent<Collider>();
    }
    void Update()
    {
        if (doChangeTrack)
        {
            changeTrack();
        }
    }

    void changeTrack()
    {
        speaker.loop = false;
        if (!speaker.isPlaying)
        {
            speaker.clip = trackList[upNext];
            speaker.loop = true;
            speaker.Play();
            doChangeTrack = false;
        }
    }

    public void setTrack(int track)
    {
        upNext = track;
        doChangeTrack = true;
    }
}
