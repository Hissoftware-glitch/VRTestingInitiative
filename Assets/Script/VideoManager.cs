using UnityEngine;
using UnityEngine.Video;
using System.Collections.Generic;

public class VideoManager : MonoBehaviour
{
    public static VideoManager Instance;

    [System.Serializable]
    public struct VideoData
    {
        public string key;
        public VideoClip clip;
    }

    public List<VideoData> videoDatabase = new List<VideoData>();
    private Dictionary<string, VideoClip> videoDictionary = new Dictionary<string, VideoClip>();

    void Awake()
    {
        Instance = this;
        InitializeDictionary();
    }

    private void InitializeDictionary()
    {
        foreach (var item in videoDatabase)
        {
            if (!videoDictionary.ContainsKey(item.key))
            {
                videoDictionary.Add(item.key, item.clip);
            }
            else
            {
                Debug.LogWarning($"Duplicate video key: {item.key}");
            }
        }
    }

    public VideoClip GetVideoClip(string key)
    {
        if (videoDictionary.TryGetValue(key, out VideoClip clip))
        {
            return clip;
        }

        Debug.LogError($"Video clip not found for key: {key}");
        return null;
    }
}