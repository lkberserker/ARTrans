using UnityEngine;

public class MicrophoneInput : MonoBehaviour
{
    public AudioSource[] audioSources;

    void Start()
    {
        var micDevices = Microphone.devices;
        foreach (string deviceM in micDevices)
        {
            Debug.Log("MIC: " + deviceM);
        }

        if (micDevices.Length < audioSources.Length)
        {
            Debug.LogError("Available microphones are less than required.");
            return;
        }

        for (int i = 0; i < audioSources.Length; i++)
        {
            audioSources[i].gameObject.name = "Microphone_" + i; 
            audioSources[i].clip = Microphone.Start(micDevices[i], true, 10, 44100);
            audioSources[i].loop = true;
            while (!(Microphone.GetPosition(micDevices[i]) > 0)) { }
            audioSources[i].Play();
        }
    }

    void Update()
    {
        foreach (AudioSource audio in audioSources)
        {
            if (IsTalking(audio, 0.04f))
            {
                Debug.Log("Active Microphone: " + audio.gameObject.name); 
            }
        }
    }

    bool IsTalking(AudioSource source, float threshold = 0.02f)
    {
        float[] samples = new float[256];
        source.GetOutputData(samples, 0);
        float sum = 0;

        foreach (var s in samples)
        {
            sum += s * s; 
        }
        return sum > threshold;
    }
}
