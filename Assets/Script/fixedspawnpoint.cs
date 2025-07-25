using UnityEngine;
using UnityEngine.XR;

public class VRSpawnPoint : MonoBehaviour
{
    [Header("Настройки спавна")]
    public GameObject objectToSpawn; // Префаб объекта для спавна
    public Transform spawnPoint;

    void Start()
    {
        SpawnObject();
    }

    public void SpawnObject()
    {
        if (objectToSpawn == null)
        {
            Debug.LogError("Не назначен объект для спавна!");
            return;
        }
        Instantiate(objectToSpawn, transform.position, transform.rotation);
    }
}