using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum SFX
{
    BUBBLE,
    WRITING,
    PAINTING,
    TENTACLE,
    WALKING
}
public class AudioHook : MonoBehaviour
{
    public List<AudioSource> AudioSources;

    static List<AudioSource> s_audioSources;

    // Start is called before the first frame update
    void Start()
    {
        s_audioSources = AudioSources;
    }
    void OnDestroy() 
    {
        s_audioSources = null;
    }


    public static float PlaySFX(SFX sfx)
    {
        Debug.Log(sfx);
        s_audioSources[(int)sfx].Play();
        return s_audioSources[(int)sfx].clip.length;
    }

    public static void StopSFX(SFX sfx)
    {
        Debug.Log("STOP" + sfx);
        s_audioSources[(int)sfx].Stop();
    }
}
