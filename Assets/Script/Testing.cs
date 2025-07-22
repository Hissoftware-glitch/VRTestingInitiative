using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class FreezeObject : MonoBehaviour
{
    private Rigidbody rb;
    private XRGrabInteractable grab;

    private bool isFrozen = false;
    private Vector3 frozenPosition;
    private Quaternion frozenRotation;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        grab = GetComponent<XRGrabInteractable>();
        grab.selectExited.AddListener(OnReleased);
    }

    public void ActivateFreeze()
    {
        isFrozen = !isFrozen;

        if (isFrozen)
        {
            // Запоминаем позицию и поворот при заморозке
            frozenPosition = transform.position;
            frozenRotation = transform.rotation;

            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        rb.isKinematic = isFrozen;
    }

    private void OnReleased(SelectExitEventArgs args)
    {
        // Восстанавливаем состояние после отпускания
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.isKinematic = isFrozen;
    }

    void LateUpdate()
    {
        if (isFrozen && grab.isSelected)
        {
            // Возвращаем объект на место каждый кадр
            transform.position = frozenPosition;
            transform.rotation = frozenRotation;
        }
    }
}
