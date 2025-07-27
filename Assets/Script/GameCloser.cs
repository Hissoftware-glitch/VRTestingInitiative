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

    // Импорты WinAPI для системных сообщений
    [DllImport("user32.dll", CharSet = CharSet.Unicode)]
    private static extern IntPtr MessageBox(IntPtr hWnd, string text, string caption, uint type);

    void Start()
    {
        if (videoPlayer == null)
        {
            videoPlayer = FindAnyObjectByType<VideoPlayer>();
            if (videoPlayer == null)
            {
                Debug.LogError("GameCloser: No VideoPlayer found in scene!");
                return;
            }
        }

        // Назначаем видео и начинаем воспроизведение
        videoPlayer.clip = closingVideoClip;
        videoPlayer.loopPointReached += OnVideoEnd;
        videoPlayer.Play();
    }

    private void OnVideoEnd(VideoPlayer source)
    {
        StartCoroutine(CloseGameRoutine());
    }

    private IEnumerator CloseGameRoutine()
    {
        // Задержка после видео
        yield return new WaitForSeconds(delayAfterVideo);

        // Показ системного сообщения
        if (showSystemMessage)
        {
            ShowSystemMessage();

            // Даем время на прочтение сообщения
            yield return new WaitForSeconds(5f);
        }

        QuitGame();
    }

    private void ShowSystemMessage()
    {
        try
        {
            // Показываем системное окно сообщения
            MessageBox(IntPtr.Zero, messageContent, messageTitle, 0);
        }
        catch (Exception e)
        {
            Debug.LogWarning("Failed to show system message: " + e.Message);
        }
    }

    private void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    void OnDestroy()
    {
        // Отписываемся от события
        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached -= OnVideoEnd;
        }
    }
}