using UnityEngine;
using UnityEngine.Video;

public class DialogueTrigger : MonoBehaviour
{
    [Header("Settings")]
    public string videoKey;
    public VideoPlayer targetVideoPlayer;

    private bool isTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (isTriggered || !other.CompareTag("Player")) return;

        PlayVideo();
        isTriggered = true;
    }

    private void PlayVideo()
    {
        VideoClip clip = VideoManager.Instance.GetVideoClip(videoKey);

        if (clip == null)
        {
            Debug.LogError($"Video clip missing for key: {videoKey}");
            gameObject.SetActive(false);
            return;
        }

        if (targetVideoPlayer == null)
        {
            Debug.LogError("Video Player reference is missing!");
            gameObject.SetActive(false);
            return;
        }

        targetVideoPlayer.clip = clip;
        targetVideoPlayer.loopPointReached += EndVideo;
        targetVideoPlayer.Play();
    }

    private void EndVideo(VideoPlayer vp)
    {
        vp.loopPointReached -= EndVideo;
        gameObject.SetActive(false);
    }
}