using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class LoopingSoundController : MonoBehaviour
{
    public EventReference fmodEvent;
    public string parameterName;

    private EventInstance soundInstance;

    private void Start()
    {
        soundInstance = RuntimeManager.CreateInstance(fmodEvent);
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
        soundInstance.start();
    }

    public void UpdateParameterValue(float value)
    {
        soundInstance.setParameterByName(parameterName, value);
    }

    public void StopSound()
    {
        soundInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        soundInstance.release();
    }
}