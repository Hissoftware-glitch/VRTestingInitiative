using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class KeyController : MonoBehaviour
{
    public LockController targetLock;
    public float attachDistance = 0.1f;

    private XRGrabInteractable grabInteractable;
    private Rigidbody rb;
    private bool isInLock = false;
    private Transform originalParent;

    void Start()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        rb = GetComponent<Rigidbody>();
        originalParent = transform.parent;

        // Подписываемся на события захвата и отпускания
        grabInteractable.selectEntered.AddListener(OnGrabbed);
        grabInteractable.selectExited.AddListener(OnReleased);
    }

    void Update()
    {
        if (isInLock || grabInteractable.isSelected) return;

        float distance = Vector3.Distance(transform.position, targetLock.keySocket.position);

        if (distance < attachDistance)
        {
            AttachToLock();
        }
    }

    private void OnGrabbed(SelectEnterEventArgs args)
    {
        // Если ключ был в замке - извлекаем его
        if (isInLock)
        {
            RemoveFromLock();
        }
    }

    private void OnReleased(SelectExitEventArgs args)
    {
        // После отпускания проверяем можно ли вставить ключ
        TryAttachToLock();
    }

    private void TryAttachToLock()
    {
        float distance = Vector3.Distance(transform.position, targetLock.keySocket.position);
        if (distance < attachDistance)
        {
            AttachToLock();
        }
    }

    private void AttachToLock()
    {
        isInLock = true;

        // Фиксируем позицию и вращение
        transform.position = targetLock.keySocket.position;
        transform.rotation = targetLock.keySocket.rotation;
        transform.SetParent(targetLock.keySocket);

        // Отключаем физику
        rb.isKinematic = true;

        // Сообщаем замку что ключ вставлен
        targetLock.KeyInserted();
    }

    private void RemoveFromLock()
    {
        isInLock = false;

        // Включаем физику обратно
        rb.isKinematic = false;
        transform.SetParent(originalParent);

        // Сообщаем замку что ключ извлечен
        targetLock.KeyRemoved();
    }

    void OnDestroy()
    {
        // Отписываемся от событий
        grabInteractable.selectEntered.RemoveListener(OnGrabbed);
        grabInteractable.selectExited.RemoveListener(OnReleased);
    }
}