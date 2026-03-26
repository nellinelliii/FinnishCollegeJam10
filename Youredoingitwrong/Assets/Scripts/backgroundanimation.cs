using UnityEngine;
using UnityEngine.Video;

public class BackgroundAnimation : MonoBehaviour
{
    void Start()
    {
        VideoPlayer vp = GetComponent<VideoPlayer>();
        vp.Play();
    }
}