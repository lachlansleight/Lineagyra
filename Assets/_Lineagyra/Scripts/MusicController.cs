using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicController : MonoBehaviour
{

    public float MaxVolume = 1f;
    private AudioSource _audio;

    private float _targetVolume;
    
    void Start()
    {
        _audio = GetComponent<AudioSource>();
        _audio.volume = 0f;
        _audio.Play();
        _targetVolume = MaxVolume;
    }

    void Update()
    {
        _audio.volume = Mathf.Lerp(_audio.volume, _targetVolume, Time.deltaTime);
        
        if (Input.GetKeyDown(KeyCode.M)) {
            _targetVolume = _targetVolume < MaxVolume ? MaxVolume : 0f;
        }        
    }
}
