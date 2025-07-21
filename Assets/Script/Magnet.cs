using UnityEngine;

public enum MagneticPolarity { Positive, Negative }

[RequireComponent(typeof(Rigidbody))]
public class Magnet : MonoBehaviour
{
    public MagneticPolarity polarity;
    public float magneticStrength = 100f;
    public float magnetRange = 5f;

    private void FixedUpdate()
    {
        Collider[] objectsInRange = Physics.OverlapSphere(transform.position, magnetRange);

        foreach (Collider col in objectsInRange)
        {
            // Магнит притягивает другие магниты и ферромагнетики
            Ferromagnetic ferro = col.GetComponent<Ferromagnetic>();
            Magnet otherMagnet = col.GetComponent<Magnet>();

            if (ferro && col.attachedRigidbody)
            {
                ApplyMagneticForce(col.attachedRigidbody, true);
            }
            else if (otherMagnet && col.attachedRigidbody && otherMagnet != this)
            {
                bool isAttraction = otherMagnet.polarity != this.polarity;
                ApplyMagneticForce(col.attachedRigidbody, isAttraction);
            }
        }
    }

    void ApplyMagneticForce(Rigidbody rb, bool attraction)
    {
        Vector3 direction = (transform.position - rb.position).normalized;
        float force = magneticStrength / Vector3.Distance(transform.position, rb.position);

        rb.AddForce(direction * force * (attraction ? 1 : -1));
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = polarity == MagneticPolarity.Positive ? Color.red : Color.blue;
        Gizmos.DrawWireSphere(transform.position, magnetRange);
    }
}
