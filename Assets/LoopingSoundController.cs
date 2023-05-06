using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class LoopingSoundController : MonoBehaviour
{
    public EventReference fmodEvent;
    public string parameterName;

    private EventInstance soundInstance;
    private AudioSource audio;
    private void Start()
    {
        //soundInstance = RuntimeManager.CreateInstance(fmodEvent);
        audio = GetComponent<AudioSource>();
    }

    // private void Update()
    // {
    //     if (Input.GetKeyDown(KeyCode.Space))
    //     {
    //         UpdateParameterValue(parameterValue);
    //     }
    //
    //     if (Input.GetKeyDown(KeyCode.S))
    //     {
    //         StopSound();
    //     }
    // }

    public  void PlaySound()
    {
        //soundInstance.start();
        audio.Play();
    }

    // public void UpdateParameterValue(float value)
    // {
    //     soundInstance.setParameterByName(parameterName, value);
    // }

    public void StopSound()
    {
        audio.Stop();
        // soundInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        // soundInstance.release();
    }
}