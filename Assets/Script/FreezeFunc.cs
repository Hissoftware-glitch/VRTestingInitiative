using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

[RequireComponent(typeof(Rigidbody), typeof(XRGrabInteractable))]
public class FreezeFunc : MonoBehaviour
{
    [Tooltip("Input Action для курка (Trigger Press)")]
    public InputActionProperty toggleAction;

    private Rigidbody rb;
    private XRGrabInteractable grabInteractable;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        grabInteractable = GetComponent<XRGrabInteractable>();
    }

    void OnEnable()
    {
        toggleAction.action.Enable();
    }

    void OnDisable()
    {
        toggleAction.action.Disable();
    }

    void Update()
    {
        // если объект сейчас в руке И нажали триггер — переключаем кинематику
        if (grabInteractable.isSelected && toggleAction.action.triggered)
        {
            rb.isKinematic = !rb.isKinematic;
            rb.useGravity = !rb.isKinematic;
        }
    }
}
