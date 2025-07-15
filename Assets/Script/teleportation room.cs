using UnityEngine;
using UnityEngine.SceneManagement;

public class teleportationroom : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log("prp");    
    }

    // Update is called once per frame
    public void LoadAnyScene (string sceneName)
    {
        Debug.Log("sceneName to load: "+ sceneName);
        SceneManager.LoadScene(sceneName);
    }
}
