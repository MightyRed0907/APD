using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicController : MonoBehaviour
{
    public bool isSingleton;
    public AudioSource aSource;
    public static MusicController mControllerInstance;
    public Button mSkipButton;
    public List<AudioClip> AudioClips = new List<AudioClip>();
         
    public void Awake()
    {
        if (isSingleton)
        {
            DontDestroyOnLoad(gameObject);
        }
    }
    public void Start()
    {
        if (isSingleton)
        {
            if(mControllerInstance != null && mControllerInstance != this)
            {
                //mSkipButton.onClick.AddListener(() => mControllerInstance.ScrollClip());
                Destroy(this.gameObject);
            }
            else
            {
                mControllerInstance = this;
            }
            
        }
    }

    public void ScrollClip()
    {
        if(mSkipButton == null)
        {
            GameObject msButton = GameObject.Find("MusicButton");
            if (msButton)
            {
                mSkipButton = msButton.GetComponent<Button>();
            }
        }
        int currentTrack = 0;
        int nextTrack = 0;
        aSource.Stop();

        if (AudioClips.Contains(aSource.clip))
        {
            currentTrack = AudioClips.IndexOf(aSource.clip);

            if(currentTrack + 1 > AudioClips.Count - 1)
            {
                nextTrack = 0;
            }
            else
            {
                nextTrack = currentTrack + 1;
            }

            aSource.clip = AudioClips[nextTrack];
            aSource.Play();
        }
        else 
        {
            if (AudioClips.Count > 0)
            {
                currentTrack = 0;
                aSource.clip = AudioClips[currentTrack];
                aSource.Play();
            }
        }
    }
}
