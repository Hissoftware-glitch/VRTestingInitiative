using UnityEngine;
using UnityEngine.Video;
using System;
using System.Runtime.InteropServices;
using System.Collections;

public class GameCloserAfterVideo : MonoBehaviour
{
    [Header("Video Settings")]
    public VideoPlayer videoPlayer;
    public VideoClip closingVideoClip;

    [Header("Exit Settings")]
    public float delayAfterVideo = 2f;
    public bool showSystemMessage = true;
    [SerializeField] public string messageTitle = "ОШИБКА 0xE3A7B";
    [SerializeField] public string messageContent = " Нарушение целостности данных \nДоступ к следующей тестовой камере заблокирован.\nОбнаружены признаки вмешательства в систему...\nПРИЧИНА: Несанкционированный доступ к протоколам\nДЕЙСТВИЕ: Инициирование протокола изоляции\nПриложение будет немедленно закрыто.";

    [DllImport("user32.dll", CharSet = CharSet.Unicode)]
    private static extern IntPtr MessageBox(IntPtr hWnd, string text, string caption, uint type);

    private bool videoFinished = false;

    void Start()
    {
        if (videoPlayer == null)
        {
            videoPlayer = GetComponent<VideoPlayer>();
            if (videoPlayer == null)
            {
                Debug.LogError("GameCloser: No VideoPlayer component found!");
                return;
            }
        }

        // Убедимся, что видео не зациклено
        videoPlayer.isLooping = false;
        videoPlayer.clip = closingVideoClip;

        // Подписываемся на событие завершения видео
        videoPlayer.loopPointReached += OnVideoEnd;

        // Начинаем воспроизведение
        videoPlayer.Play();
        Debug.Log("Video playback started");
    }

    private void OnVideoEnd(VideoPlayer source)
    {
        Debug.Log("Video finished playing");
        videoFinished = true;
        StartCoroutine(CloseGameRoutine());
    }

    private IEnumerator CloseGameRoutine()
    {
        // Ждем указанную задержку после видео
        yield return new WaitForSeconds(delayAfterVideo);

        if (showSystemMessage && videoFinished)
        {
            Debug.Log("Showing system message");
            ShowSystemMessage();
        }

        QuitGame();
    }

    private void ShowSystemMessage()
    {
        try
        {
            // Отображаем сообщение перед выходом
            MessageBox(IntPtr.Zero, messageContent, messageTitle, 0x1000); // 0x1000 = MB_SYSTEMMODAL
        }
        catch (Exception e)
        {
            Debug.LogError("System message error: " + e.Message);
        }
    }

    private void QuitGame()
    {
        Debug.Log("Quitting application...");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    void OnDestroy()
    {
        // Отписываемся от события при уничтожении объекта
        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached -= OnVideoEnd;
        }
    }
}